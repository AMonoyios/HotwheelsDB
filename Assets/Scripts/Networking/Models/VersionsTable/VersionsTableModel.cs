/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using Newtonsoft.Json;

namespace HWAPI
{
    public sealed class VersionsTableModel
    {
        public VersionTable parse;

        public string Title()
        {
            return parse.title;
        }

        public int PageID()
        {
            return parse.pageid;
        }

        public string Content()
        {
            return parse.wikitext.content;
        }
    }

    public class VersionTable
    {
        public string title;
        public int pageid;
        public VersionsTableContent wikitext;
    }

    public class VersionsTableContent
    {
        [JsonProperty("*")]
        public string content;
    }
}
