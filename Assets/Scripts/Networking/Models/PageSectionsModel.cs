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
    public sealed class PageSectionsModel
    {
        public Parse parse;

        public class Parse
        {
            public string title;
            public int pageid;
            public List<Sections> sections;
        }

        public class Sections
        {
            [JsonProperty("line")]
            public string name;
            public int index;
        }

        public class PageSection
        {
            public PageSection(string name, int index)
            {
                Name = name;
                Index = index;
            }

            public string Name { get; }
            public int Index { get; }
        }
    }
}
