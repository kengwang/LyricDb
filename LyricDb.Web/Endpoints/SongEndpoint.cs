using System.Runtime.CompilerServices;
using System.Security.Claims;
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

public class SongEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // nothing
    }

    public static void ConfigureApp(WebApplication app)
    {
        var group = app.MapGroup("/song");
        group.MapGet("", GetPagedSongs)
            .Produces<PagedResponseBase<SongInfoResponse>>()
            .WithName(nameof(GetPagedSongs));
        group.MapGet("/{id:guid}", GetSong)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<SongInfoResponse>()
            .WithName(nameof(GetSong));
        group.MapPost("", PostSong)
            .Accepts<SongPostRequest>("application/json")
            .Produces<SongInfoResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithName(nameof(PostSong));
        group.MapPut("/{id:guid}", PutSong)
            .Accepts<SongPutRequest>("application/json")
            .Produces<SongInfoResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithName(nameof(PutSong));
        group.MapGet("/{id:guid}/lyric", GetSongLyrics)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<List<LyricInfoResponse>>()
            .WithName(nameof(GetSongLyrics));
        group.MapPut("/{id:guid}/lyric", SetSongLyric)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status200OK)
            .WithName(nameof(SetSongLyric));
        group.MapGet("/bind/{bind}", GetSongByBind)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<SongInfoResponse>()
            .WithName(nameof(GetSongByBind));
        group.MapDelete("/{id:guid}", DeleteSongById)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces<SongInfoResponse>()
            .WithName(nameof(DeleteSongById));
    }
    
    [Authorize(Roles = AuthRole.Reviewer)]
    private static async Task<IResult> SetSongLyric(
        [FromRoute] Guid id,
        [FromQuery] Guid lyricId,
        [FromServices] IRepository<Song> repository,
        [FromServices] IRepository<Lyric> lyricRepository,
        [FromServices] IMapper<Song, SongInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var song = await repository.ReadAsync(id, cancellationToken);
        if (song is null)
            return Results.NotFound();
        var lyric = await lyricRepository.ReadAsync(lyricId, cancellationToken);
        if (lyric is null)
            return Results.NotFound();
        song.CurrentLyric = lyricId;
        await repository.UpdateAsync(song, cancellationToken);
        return Results.Ok(mapper.Map(song));
    }
    

    private static async Task<IResult> GetPagedSongs(
        [FromServices] IRepository<Song> repository,
        [FromServices] IMapper<Song, SongInfoResponse> mapper,
        [FromQuery] int page = 0,
        [FromQuery] int pageSize = 10,
        [FromQuery] string search = "",
        CancellationToken cancellationToken = default)
    {
        var query = await repository.GetQueryableAsync(cancellationToken);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(t => t.Name.Contains(search) || t.Album.Contains(search) || t.Artists.Contains(search));
        var total = await query.CountAsync(cancellationToken);
        var songs = await query
            .OrderByDescending(t => t.CreateTime)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return Results.Ok(new PagedResponseBase<SongInfoResponse>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = total,
            TotalPages = (int) Math.Ceiling(total / (double) pageSize),
            Items = songs.Select(mapper.Map).ToList()
        });
    }

    [Authorize(Roles = AuthRole.Reviewer)]
    private static async Task<IResult> DeleteSongById(
        Guid id, ClaimsPrincipal principal, IRepository<Song> repository, CancellationToken cancellationToken = default)
    {
        if (await repository.ReadAsync(id, cancellationToken) is null)
        {
            return Results.NotFound();
        }
        await repository.DeleteAsync(id, cancellationToken);
        return Results.Ok();
    }

    private static async Task<IResult> GetSongByBind(
        string bind,
        IRepository<Song> repository,
        IMapper<Song, SongInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var query = await repository.GetQueryableAsync(cancellationToken);
        var song = await query.FirstOrDefaultAsync(t => t.Binds.Any(s => EF.Functions.Like(bind, s)),
            cancellationToken: cancellationToken);
        return song is null ? Results.NotFound() : Results.Ok(mapper.Map(song));
    }

    [Authorize(Roles = AuthRole.Reviewer)]
    private static async Task<IResult> PutSong(
        [FromRoute] Guid id,
        SongPutRequest request,
        [FromServices] IRepository<Song> repository,
        [FromServices] IMapper<Song, SongInfoResponse> mapper,
        ClaimsPrincipal principal,
        UserManager<User> userManager,
        CancellationToken cancellationToken = default
        )
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
            return Results.Unauthorized();

        var song = await repository.ReadAsync(id, cancellationToken);

        if (song is null)
            return Results.NotFound();
        
        song.Name = request.Name;
        song.Artists = request.Artists;
        song.Album = request.Album;
        song.Binds = request.Binds;
        song.Submitter = user;
        if (user.Role >= UserRole.Reviewer)
        {
            song.CurrentLyric = request.CurrentLyric;
        }
        
        await repository.UpdateAsync(song, cancellationToken);
        return Results.Ok(mapper.Map(song));
    }
    
    [Authorize(Roles = AuthRole.User)]
    private static async Task<IResult> PostSong(
        SongPostRequest request,
        [FromServices] IRepository<Song> repository,
        [FromServices] IMapper<Song, SongInfoResponse> mapper,
        ClaimsPrincipal principal,
        UserManager<User> userManager,
        CancellationToken cancellationToken = default)

    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
            return Results.Unauthorized();
        var song = new Song
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Artists = request.Artists,
            Album = request.Album,
            Binds = request.Binds,
            CreateTime = DateTime.Now,
            Submitter = user
        };
        await repository.CreateAsync(song, cancellationToken);
        return Results.Created($"/song/{song.Id}", mapper.Map(song));
    }

    private static async Task<IResult> GetSong(
        Guid id,
        [FromServices] IRepository<Song> repository,
        [FromServices] IMapper<Song, SongInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var song = await repository.ReadAsync(id, cancellationToken);
        return song is null ? Results.NotFound() : Results.Ok(mapper.Map(song));
    }
    
    
    private static async Task<IResult> GetSongLyrics(
        Guid id,
        [FromServices] IRepository<Song> repository,
        [FromServices] IRepository<Lyric> lyricRepository,
        [FromServices] IMapper<Lyric, LyricInfoResponse> mapper,
        CancellationToken cancellationToken = default)
    {
        var song = await repository.ReadAsync(id, cancellationToken);
        if (song is null)
            return Results.NotFound();
        var query = await lyricRepository.GetQueryableAsync(cancellationToken);
        var result = query.Where(t => song.Lyrics.Contains(t.Id)).Select(t=>mapper.Map(t)).ToList();
        return Results.Ok(result);
    }
}