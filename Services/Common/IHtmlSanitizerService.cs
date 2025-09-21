namespace ApiApplication.Services.Common
{
    public interface IHtmlSanitizerService
    {
        string Sanitize(string html);
        string SanitizeTitle(string title);
        List<string> SanitizeTags(List<string> tags);
    }
}
