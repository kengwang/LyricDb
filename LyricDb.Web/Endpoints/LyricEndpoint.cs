using System.Security.Claims;
using System.Text.Json;
using ALRC.Abstraction;
using LyricDb.Contracts.Models;
using LyricDb.Web.Interfaces;
using LyricDb.Web.Models.Constants;
using LyricDb.Web.Models.Dao;
using LyricDb.Web.Models.Dto.Requests;
using LyricDb.Web.Models.Dto.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LyricDb.Web.Endpoints;

public class LyricEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
    }

    public static void ConfigureApp(WebApplication app)
    {
        var group = app.MapGroup("/lyric");
        group.MapGet("", GetPagedLyrics)
            .Produces<PagedResponseBase<LyricInfoResponse>>()
            .WithName(nameof(GetPagedLyrics));
        group.MapGet("/{id:guid}", GetLyric)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<LyricInfoResponse>()
            .WithName(nameof(GetLyric));
        group.MapGet("/{id:guid}/{type}", GetLyricByType)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<LyricInfoResponse>()
            .WithName(nameof(GetLyricByType));
        group.MapGet("/{id:guid}/{type}/contents", GetLyricContentByType)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<string>()
            .WithName(nameof(GetLyricContentByType));
        group.MapGet("/{id:guid}/contents", GetLyricContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<ALRCFile>()
            .WithName(nameof(GetLyricContent));
        group.MapPost("", PostLyric)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK)
            .WithName(nameof(PostLyric));
        group.MapPost("/{type}", PostLyricType)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status201Created)
            .WithName(nameof(PostLyricType));
        group.MapPut("", PutLyric)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK)
            .WithName(nameof(PutLyric));
        group.MapPut("/{type}", PutLyricType)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status201Created)
            .WithName(nameof(PutLyricType));
    }
    
    private static async Task<IResult> GetPagedLyrics(
        [FromServices] IRepository<Lyric> repository,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] int status = -1,
        CancellationToken cancellationToken = default)
    {
        var query = await repository.GetQueryableAsync(cancellationToken);
        if (status != -1)
        {
            query = query.Where(x => x.Status == (LyricStatus)status );
        }
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

    [Authorize(Roles = AuthRole.Reviewer)]
    private static async Task<IResult> PutLyric(
        [FromBody] LyricPutRequest request,
        [FromServices] IRepository<Lyric> repository,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        [FromServices] UserManager<User> userManager,
        [FromServices] IRepository<Song> songRepository,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null || user.Role < UserRole.Admin)
        {
            return Results.Unauthorized();
        }

        var lyric = await repository.ReadAsync(request.Id, cancellationToken);
        if (lyric is null)
        {
            return Results.NotFound();
        }
        
        lyric.Status = (LyricStatus)request.Status;
        if (lyric.Status is LyricStatus.Approved or LyricStatus.Rejected)
        {
            lyric.Reviewer = user;
        }

        if (!string.IsNullOrEmpty(request.Content))
        {
            lyric.Content = request.Content;
        }
        
        await repository.UpdateAsync(lyric, cancellationToken);
        user.ContributionPoint++;
        await userManager.UpdateAsync(user);
        return Results.Ok(mapper.Map(lyric));
    }
    
    [Authorize(Roles = AuthRole.Reviewer)]
    private static async Task<IResult> PutLyricType(
        [FromRoute] string? type,
        [FromBody] LyricPutRequest request,
        [FromServices] IRepository<Lyric> repository,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        [FromServices] UserManager<User> userManager,
        [FromServices] IRepository<Song> songRepository,
        [FromServices] ILyricConverterSelector lyricConverterSelector,
        ClaimsPrincipal principal,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null || user.Role < UserRole.Admin)
        {
            return Results.Unauthorized();
        }

        var lyric = await repository.ReadAsync(request.Id, cancellationToken);
        if (lyric is null)
        {
            return Results.NotFound();
        }

        lyric.Author = request.Author;
        lyric.Proofreader = request.Proofreader;
        lyric.Timeline = request.Timeline;
        lyric.Translator = request.Translator;
        lyric.Transliterator = request.Transliterator;
        lyric.Status = (LyricStatus)request.Status;
        if (lyric.Status is LyricStatus.Approved or LyricStatus.Rejected)
        {
            lyric.Reviewer = user;
        }

        if (!string.IsNullOrEmpty(request.Content))
        {
            var converter = lyricConverterSelector.GetConverter(type);
            var alrcConverter = lyricConverterSelector.GetConverter("alrc");
            if (converter is null || alrcConverter is null)
            {
                return Results.BadRequest("Bad type");
            }

            var content = alrcConverter.ConvertBack(converter.Convert(request.Content));
            lyric.Content = content;
        }
        
        await repository.UpdateAsync(lyric, cancellationToken);
        user.ContributionPoint++;
        await userManager.UpdateAsync(user);
        return Results.Ok(mapper.Map(lyric));
    }
    
    [Authorize(Roles = AuthRole.User)]
    private static async Task<IResult> PostLyric(
        [FromBody] LyricCreateRequest request,
        IRepository<Lyric> repository,
        IRepository<Song> songRepository,
        IMapper<Lyric, LyricInfoResponse> mapper,
        ClaimsPrincipal principal,
        UserManager<User> userManager,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var song = await songRepository.ReadAsync(request.SongId, cancellationToken);
        if (song is null)
        {
            return Results.NotFound();
        }

        var lyric = new Lyric
        {
            Content = request.Content,
            Id = Guid.NewGuid(),
            CreateTime = DateTime.Now,
            Submitter = user,
            Duration = request.Duration,
            Song = song,
            Author = request.Author,
            Proofreader = request.Proofreader,
            Timeline = request.Timeline,
            Translator = request.Translator,
            Transliterator = request.Transliterator,
        };
        await repository.CreateAsync(lyric, cancellationToken);
        song.Lyrics.Add(lyric.Id);
        await songRepository.UpdateAsync(song, cancellationToken);
        user.ContributionPoint++;
        await userManager.UpdateAsync(user);
        return Results.Created($"/lyric/{lyric.Id}", mapper.Map(lyric));
    }
    
    [Authorize(Roles = AuthRole.User)]
    private static async Task<IResult> PostLyricType(
        [FromRoute] string? type,
        [FromBody] LyricCreateRequest request,
        IRepository<Lyric> repository,
        IRepository<Song> songRepository,
        ILyricConverterSelector lyricConverterSelector,
        IMapper<Lyric, LyricInfoResponse> mapper,
        ClaimsPrincipal principal,
        UserManager<User> userManager,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        var song = await songRepository.ReadAsync(request.SongId, cancellationToken);
        if (song is null)
        {
            return Results.NotFound();
        }

        var converter = lyricConverterSelector.GetConverter(type);
        var alrcConverter = lyricConverterSelector.GetConverter("alrc");
        if (converter is null || alrcConverter is null)
        {
            return Results.BadRequest("Bad type");
        }

        var content = alrcConverter.ConvertBack(converter.Convert(request.Content));
        var lyric = new Lyric
        {
            Content = content,
            Id = Guid.NewGuid(),
            CreateTime = DateTime.Now,
            Submitter = user,
            Duration = request.Duration,
            Song = song,
            Author = request.Author,
            Proofreader = request.Proofreader,
            Timeline = request.Timeline,
            Translator = request.Translator,
            Transliterator = request.Transliterator,
        };
        await repository.CreateAsync(lyric, cancellationToken);
        song.Lyrics.Add(lyric.Id);
        await songRepository.UpdateAsync(song, cancellationToken);
        user.ContributionPoint++;
        await userManager.UpdateAsync(user);
        return Results.Created($"/lyric/{lyric.Id}", mapper.Map(lyric));
    }

    private static async Task<IResult> GetLyric(
        Guid id,
        [FromServices] IRepository<Lyric> lyrics,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var lyric = await lyrics.ReadAsync(id, cancellationToken);
        return lyric == null ? Results.NotFound() : Results.Ok(mapper.Map(lyric));
    }
    
    private static async Task<IResult> GetLyricContent(
        Guid id,
        [FromServices] IRepository<Lyric> lyrics,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var lyric = await lyrics.ReadAsync(id, cancellationToken);
        return lyric == null ? Results.NotFound() : Results.Ok(JsonSerializer.Deserialize<ALRCFile>(lyric.Content));
    }
    
    private static async Task<IResult> GetLyricContentByType(
        Guid id,
        string type,
        [FromServices] ILyricConverterSelector lyricConverterSelector,
        [FromServices] IRepository<Lyric> lyrics,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var lyric = await lyrics.ReadAsync(id, cancellationToken);
        if (lyric is null)
        {
            return Results.NotFound();
        }

        var service = lyricConverterSelector.GetConverter(type);
        var alrcService = lyricConverterSelector.GetConverter("alrc");
        if (service is null || alrcService is null)
        {
            return Results.BadRequest();
        }
        return Results.Ok(service.ConvertBack(alrcService.Convert(lyric.Content)));
    }

    private static async Task<IResult> GetLyricByType(
        Guid id,
        string type,
        [FromServices] LyricConverterSelector lyricConverterSelector,
        [FromServices] IRepository<Lyric> lyrics,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var service = lyricConverterSelector.GetConverter(type);
        var alrcService = lyricConverterSelector.GetConverter("alrc");
        if (service is null || alrcService is null)
        {
            return Results.BadRequest();
        }

        var lyric = await lyrics.ReadAsync(id, cancellationToken);
        if (lyric is null)
        {
            return Results.NotFound();
        }

        var result = mapper.Map(lyric);
        return Results.Ok(result);
    }
}