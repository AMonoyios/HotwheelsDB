/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */
namespace HWAPI
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class YearCategoriesModel : BaseNavigateModel
    {
        [JsonProperty("query")]
        private readonly Query query;

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
        }

        public List<YearMember> YearCategories
        {
            get
            {
                return query.years;
            }
            set
            {
                query.years = value;
            }
        }
    }
}
