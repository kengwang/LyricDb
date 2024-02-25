namespace LyricDb.Worker.Services;

public class MailTemplateSelector
{
    public async Task<string?> GetTemplateAsync(string templateName)
    {
        // get from file
        var path = Path.Combine(Directory.GetCurrentDirectory(), "MailTemplates", $"{templateName}.html");
        if (File.Exists(path))
        {
            return await File.ReadAllTextAsync(path);
        }
        return null;
    }
}