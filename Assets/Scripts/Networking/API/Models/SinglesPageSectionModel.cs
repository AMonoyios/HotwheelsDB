/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using SW.Logger;
using UnityEngine;

namespace HWAPI
{
    public sealed class SinglesPageSectionModel
    {
        [JsonProperty("parse")]
        public Parse parse;

        public class Parse
        {
            [JsonProperty("title")]
            public string title;

            [JsonProperty("text")]
            public Text table;

            public class Text
            {
                [JsonProperty("*")]
                public string content;
            }
        }

        public class Car
        {
            public Car(string name, string image)
            {
                Name = name;
                Image = image;
            }
            public string Name { get; }
            public string Image { get; }
        }
    }
}
