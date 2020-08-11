using System;
using System.Collections.Generic;

namespace MetalMonkey.Engine.Components
{
    public class FrontMatter
    {
        public static FrontMatter Default { get; } = new FrontMatter();

        public string Title { get; set; } = "Title";

        public string Lede { get; set; } = "Lede";

        public DateTime Published { get; set; }

        public string Author { get; set; } = "Nomen Nescio";

        public Uri AuthorAvatar { get; set; } = new Uri("images/avatar.jpg", UriKind.Relative);

        public Uri FeaturedImage { get; set; } = new Uri("images/pic01.jpg", UriKind.Relative);

        public string FeaturedImageAlt { get; set; } = string.Empty;

        public IEnumerable<string> Tags { get; set; } = new List<string> { "General" };

        public int Likes { get; set; }

        public int CommentCount { get; set; }
    }
}
