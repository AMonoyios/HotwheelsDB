/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace HWAPI
{
    public sealed class SinglesPageModel
    {
        [JsonProperty("parse")]
        public Parse parse;

        public class Parse
        {
            [JsonProperty("title")]
            public string title;

            [JsonProperty("sections")]
            public List<Sections> sections;
        }

        public class Sections
        {
            /// <summary>
            ///     The header for this page section
            /// </summary>
            [JsonProperty("line")]
            public string header;

            /// <summary>
            ///     The index of this section
            /// </summary>
            [JsonProperty("index")]
            public int index;
        }

        public List<Sections> PageSections
        {
            get
            {
                return parse.sections;
            }
            set
            {
                parse.sections = value;
            }
        }
    }
}
