using System.Security.Claims;
using System.Text.Json;
using ALRC.Abstraction;
using LyricDb.Web.Interfaces;
using LyricDb.Web.Models.Constants;
using LyricDb.Web.Models.Dao;
using LyricDb.Web.Models.Dto.Requests;
using LyricDb.Web.Models.Dto.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LyricDb.Web.Endpoints;

public class LyricEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
    }

    public static void ConfigureApp(WebApplication app)
    {
        var group = app.MapGroup("/lyric");
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
            .Produces(StatusCodes.Status201Created)
            .WithName(nameof(PostLyric));
        group.MapPost("/{type}", PostLyricType)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status201Created)
            .WithName(nameof(PostLyricType));
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
            Song = song
        };
        await repository.CreateAsync(lyric, cancellationToken);
        song.Lyrics.Add(lyric.Id);
        await songRepository.UpdateAsync(song, cancellationToken);
        return Results.Created($"/lyric/{lyric.Id}", mapper.Map(lyric));
    }
    
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
            Song = song
        };
        await repository.CreateAsync(lyric, cancellationToken);
        song.Lyrics.Add(lyric.Id);
        await songRepository.UpdateAsync(song, cancellationToken);
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