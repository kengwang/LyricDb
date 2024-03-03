using ALRC.Converters;
using LyricDb.Web.Interfaces;

namespace LyricDb.Web.Endpoints;

public class ALRCEndpoint : IEndpointBase
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ILyricConverterSelector, LyricConverterSelector>();
    }

    public static void ConfigureApp(WebApplication app)
    {
        // ignore
    }
}

public interface ILyricConverterSelector
{
    public ILyricConverter<string>? GetConverter(string? type);
}

public class LyricConverterSelector : ILyricConverterSelector
{
   
    private readonly Dictionary<string, ILyricConverter<string>> _converters = new()
    {
        {"ttml", new AppleSyllableConverter()},
        {"alrc", new ALRCConverter()},
        {"lrc", new LrcConverter()},
        {"lyricify", new LyricifySyllableConverter()},
        {"yrc", new NeteaseYrcConverter()},
        {"qrc", new QQLyricConverter()}
    };
    

    public ILyricConverter<string>? GetConverter(string? type)
    {
        return _converters.GetValueOrDefault(type ?? "");
    }
}