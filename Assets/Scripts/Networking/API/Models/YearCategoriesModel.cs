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
            /// <summary>
            ///     Page ID
            /// </summary>
            [JsonProperty("pageid")]
            public int id;

            /// <summary>
            ///     The raw title of the current page
            /// </summary>
            [JsonProperty("title")]
            public string title;

            /// <summary>
            ///     Label that represents the UI side of this page
            /// </summary>
            public string label;

            /// <summary>
            ///     The title of the page that this page will point to
            /// </summary>
            public string targetPageTitle;
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
