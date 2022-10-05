/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */
namespace HWAPI
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class YearCategoriesModel
    {
        // TODO: make the most basic version of navigation response a sub class

        [JsonProperty("query")]
        private Query query;
        [JsonProperty("continue")]
        private Continue navigation;

        public class Continue
        {
            [JsonProperty("cmcontinue")]
            public string next;
        }

        // TODO: For each different variant of navigation create a parent class that derives from the above and has the bellow parameters/functions ekatalaves ti enoo

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
        public Continue Navigate => navigation;
    }
}
