/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */
namespace HWAPI
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public sealed class YearCategoriesModel
    {
        [JsonProperty("query")]
        private Query query;

        private class Query
        {
            [JsonProperty("categorymembers")]
            public List<YearMember> years;
        }

        public class YearMember
        {
            [JsonProperty("pageid")]
            public int id;
            [JsonProperty("title")]
            public string title;

            public string label;
            public uint onPage;
        }

        public List<YearMember> YearCategories => query.years;
    }
}
