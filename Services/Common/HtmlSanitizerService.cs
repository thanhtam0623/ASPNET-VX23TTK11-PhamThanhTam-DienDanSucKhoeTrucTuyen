using Ganss.Xss;

namespace ApiApplication.Services.Common
{
    public class HtmlSanitizerService : IHtmlSanitizerService
    {
        private readonly HtmlSanitizer _sanitizer;
        private readonly HtmlSanitizer _strictSanitizer;

        public HtmlSanitizerService()
        {
            // For content - allow some safe HTML tags
            _sanitizer = new HtmlSanitizer();
            _sanitizer.AllowedTags.Clear();
            _sanitizer.AllowedTags.UnionWith(new[] { "p", "br", "strong", "em", "ul", "ol", "li", "blockquote", "code", "pre" });
            _sanitizer.AllowedAttributes.Clear();
            _sanitizer.AllowedCssProperties.Clear();

            // For titles and tags - remove all HTML
            _strictSanitizer = new HtmlSanitizer();
            _strictSanitizer.AllowedTags.Clear();
            _strictSanitizer.AllowedAttributes.Clear();
            _strictSanitizer.AllowedCssProperties.Clear();
        }

        public string Sanitize(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            return _sanitizer.Sanitize(html);
        }

        public string SanitizeTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            return _strictSanitizer.Sanitize(title).Trim();
        }

        public List<string> SanitizeTags(List<string> tags)
        {
            if (tags == null || !tags.Any())
                return new List<string>();

            return tags
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Select(tag => _strictSanitizer.Sanitize(tag).Trim())
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .ToList();
        }
    }
}
