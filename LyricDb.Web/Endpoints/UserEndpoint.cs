using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LyricDb.Contracts.Messages;
using LyricDb.Contracts.Models;
using LyricDb.Web.Extensions;
using LyricDb.Web.Interfaces;
using LyricDb.Web.Models.Constants;
using LyricDb.Web.Models.Dao;
using LyricDb.Web.Models.Dto.Requests;
using LyricDb.Web.Models.Dto.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolverine;

namespace LyricDb.Web.Endpoints;

public class UserEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole<Guid>>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<PostgresDbContext>()
            .AddDefaultTokenProviders()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddErrorDescriber<ZhCNIdentityErrorDescriber>();
        builder.Services.AddAuthorization();
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = OnNeedLoginRedirect;
            options.Events.OnRedirectToAccessDenied = OnNoPermissionRedirect;
        });
    }

    private static Task OnNeedLoginRedirect(RedirectContext<CookieAuthenticationOptions> context)
    {
        context.Response.StatusCode = 401;
        context.Response.Headers.Remove("Location");
        return Task.CompletedTask;
    }

    private static Task OnNoPermissionRedirect(RedirectContext<CookieAuthenticationOptions> context)
    {
        context.Response.StatusCode = 403;
        context.Response.Headers.Remove("Location");
        return Task.CompletedTask;
    }

    public static void ConfigureApp(WebApplication app)
    {
        var group = app.MapGroup("/user");
        group.MapPost("/register", PostRegister)
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .WithName(nameof(PostRegister));
        group.MapPost("/login", PostLogin)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status200OK, typeof(UserInfoResponse))
            .WithName(nameof(PostLogin));
        group.MapPost("/{userId:guid}/lyrics", GetUserPagedLyrics)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces<PagedResponseBase<LyricInfoResponse>>()
            .WithName(nameof(GetUserPagedLyrics));
        group.MapGet("/{userId:guid}", GetUserInfo)
            .Produces(StatusCodes.Status200OK, typeof(UserInfoResponse))
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithName(nameof(GetUserInfo));
        group.MapGet("", GetMyUserInfo)
            .Produces(StatusCodes.Status200OK, typeof(UserInfoResponse))
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithName(nameof(GetMyUserInfo));
        group.MapPut("", UpdateUserInfo)
            .Produces(StatusCodes.Status200OK, typeof(UserInfoResponse))
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status304NotModified)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .WithName(nameof(UpdateUserInfo));
        group.MapGet("/{id:guid}/verify/email", ConfirmEmail)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName(nameof(ConfirmEmail));
    }
    
    private static async Task<IResult> GetUserPagedLyrics(
        [FromServices] IRepository<Lyric> repository,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        [FromQuery] Guid userId,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = await repository.GetQueryableAsync(cancellationToken);
        query = query.Where(t => t.Submitter.Id == userId);
        var total = await query.CountAsync(cancellationToken);
        var lyrics = await query
            .OrderByDescending(t => t.CreateTime)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return Results.Ok(new PagedResponseBase<LyricInfoResponse>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = (int) Math.Ceiling(total / (double) pageSize),
            Items = lyrics.Select(mapper.Map).ToList()
        });
    }

    private static async Task<IResult> ConfirmEmail(
        [FromQuery] string token,
        [FromServices] RoleManager<IdentityRole<Guid>> roleManager,
        [FromRoute] Guid id,
        [FromServices] UserManager<User> userManager,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
        if (user is null)
        {
            return Results.NotFound();
        }

        var result = await userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            if (user.Role == UserRole.None)
            {
                user.Role = UserRole.User;
                await userManager.UpdateAsync(user);
            }

            // add role
            var roles = UserRoleMapper.Map(user.Role).Split(',');
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        return result.Succeeded
            ? Results.Ok()
            : Results.Problem("验证失败", statusCode: StatusCodes.Status400BadRequest);
    }

    [Authorize]
    private static async Task<IResult> UpdateUserInfo([FromServices] UserManager<User> userManager,
        [FromBody] UserPutRequest request, ClaimsPrincipal principal, IMapper<User, UserInfoResponse> mapper)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        if (user.Role == UserRole.Admin && request.Role.HasValue)
        {
            await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
            user.Role = request.Role.Value;
            var roles = UserRoleMapper.Map(user.Role).Split(',');
            await userManager.AddToRolesAsync(user, roles);
        }

        if (user.Role == UserRole.Admin || user.Id == request.Id)
        {
            var targetUser = await userManager.Users.FirstOrDefaultAsync(t => t.Id == request.Id);
            if (targetUser is null)
            {
                return Results.NotFound();
            }

            if (!string.IsNullOrWhiteSpace(request.UserName))
                targetUser.UserName = request.UserName;
            if (!string.IsNullOrWhiteSpace(request.Email))
                targetUser.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Password) && !string.IsNullOrWhiteSpace(request.OldPassword))
            {
                var pwresetResult =
                    await userManager.ChangePasswordAsync(targetUser, request.OldPassword, request.Password);
                if (!pwresetResult.Succeeded)
                {
                    return Results.Problem(
                        $"密码重置失败: {string.Join(' ', pwresetResult.Errors.Select(t => t.Description))}",
                        statusCode: StatusCodes.Status400BadRequest);
                }
            }

            if (user.Role == UserRole.Admin && !string.IsNullOrWhiteSpace(request.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(targetUser);
                var pwresetResult = await userManager.ResetPasswordAsync(targetUser, token, request.Password);
                if (!pwresetResult.Succeeded)
                {
                    return Results.Problem(
                        $"管理员密码重置失败: {string.Join(' ', pwresetResult.Errors.Select(t => t.Description))}",
                        statusCode: StatusCodes.Status400BadRequest);
                }
            }

            var result = await userManager.UpdateAsync(user);
            return result.Succeeded
                ? Results.Ok(mapper.Map(user))
                : Results.StatusCode(StatusCodes.Status304NotModified);
        }

        return Results.Unauthorized();
    }

    [Authorize]
    private static async Task<IResult> GetMyUserInfo(
        [FromServices] UserManager<User> userManager,
        [FromServices] IMapper<User, UserInfoResponse> mapper,
        ClaimsPrincipal principal
    )
    {
        var user = await userManager.GetUserAsync(principal);
        var roles = await userManager.GetRolesAsync(user!);
        return user is null ? Results.Unauthorized() : Results.Ok(mapper.Map(user));
    }

    [Authorize(Roles = AuthRole.User)]
    private static async Task<IResult> GetUserInfo(Guid userId, UserManager<User> userManager,
        IMapper<User, UserInfoResponse> mapper)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is null
            ? Results.NotFound()
            : Results.Ok(mapper.Map(user));
    }

    private static async Task<IResult> PostLogin(
        [FromBody] UserLoginRequest request,
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IMapper<User, UserInfoResponse> mapper)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(t =>
            t.Email == request.Account || t.UserName == request.Account);
        if (user is null)
        {
            return Results.Problem("账号或密码错误", statusCode: 401);
        }

        var result = await signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (result.Succeeded)
        {
            // add role
            var roles = UserRoleMapper.Map(user.Role).Split(',');
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            await userManager.AddToRolesAsync(user, roles);
        }

        return !result.Succeeded
            ? Results.Problem(result.IsLockedOut ? "尝试次数过多, 账号被停用" : "账号或密码错误", null, 401)
            : Results.Ok(mapper.Map(user));
    }

    private static async Task<IResult> PostRegister(
        [FromServices] UserManager<User> userManager,
        [FromBody] UserRegisterRequest request,
        [FromServices] IMapper<User, UserInfoResponse> mapper,
        [FromServices] IMapper<User, UserInfo> messageMapper,
        [FromServices] IMessageBus messageBus,
        CancellationToken cancellationToken = default)
    {
        StringBuilder avatarSb = new();
        var avatarMd5Arr = MD5.HashData(Encoding.UTF8.GetBytes(request.Email));
        for (int i = 0; i < avatarMd5Arr.Length; i++)
        {
            avatarSb.Append(avatarMd5Arr[i].ToString("x2"));
        }
        var user = new User
        {
            UserName = request.Name,
            Email = request.Email,
            Role = UserRole.None,
            Avatar = avatarSb.ToString()
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await messageBus.PublishAsync(new UserRegisterVerificationMessage
            {
                User = messageMapper.Map(user),
                ActivationToken = token
            });
        }

        return result.Succeeded
            ? Results.Created("/user/info/", mapper.Map(user))
            : Results.ValidationProblem(new Dictionary<string, string[]>()
            {
                { "register", result.Errors.Select(t => t.Description).ToArray() }
            });
    }
}