/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections.Generic;

namespace HWAPI
{
    public sealed class ImageModel
    {
        public Query query;

        public Pages[] ImagesData
        {
            get
            {
                List<Pages> imageData = new();

                List<int> keys = new();
                int index = 0;
                foreach (KeyValuePair<int, Pages> page in query.pages)
                {
                    keys.Add(page.Key);

                    var imageElement = query.pages[keys[index]];

                    Pages newPage = new()
                    {
                        ns = imageElement.ns,
                        pageid = imageElement.pageid,
                        title = imageElement.title,
                        thumbnail = imageElement.thumbnail
                    };

                    imageData.Add(newPage);

                    index ++;
                }

                return imageData.ToArray();
            }
        }

        public class Query
        {
            public Dictionary<int, Pages> pages;
        }

        public class Pages
        {
            public int pageid;
            public int ns;
            public string title;
            public Thumbnail thumbnail;
        }

        public class Thumbnail
        {
            public string source;
            public int width;
            public int height;
        }
    }
}
