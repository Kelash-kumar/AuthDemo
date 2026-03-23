using AuthDemo.Services.Interfaces;
using Slugify;
namespace AuthDemo.Services.Implementations
{
    public class SlugService : ISlugService
    {
        private readonly SlugHelper _slugify;
        public SlugService()
        {
            // Configure Slugify.Core behaviour once here
            var config = new SlugHelperConfiguration
            {
                ForceLowerCase = true,
                //CollapseWhiteSpace = true,
                TrimWhitespace = true,
                CollapseDashes = true,
                //DeniedCharactersRegex = @"[^a-z0-9\-]"
            };

            _slugify = new SlugHelper(config);
        }


        public string Generate(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            return _slugify.GenerateSlug(input);
        }


        public string GenerateUnique(string input, IEnumerable<string> existingSlugs)
        {
            var baseSlug = Generate(input);
            var slug = baseSlug;
            var counter = 2;
            var slugSet = new HashSet<string>(existingSlugs, StringComparer.OrdinalIgnoreCase);

            while (slugSet.Contains(slug))
                slug = $"{baseSlug}-{counter++}";

            return slug;
        }
    }
}
