/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class BaseNavigateModel
{
    [JsonProperty("continue")]
    private readonly Continue navigation;

    public class Continue
    {
        [JsonProperty("cmcontinue")]
        public string next;
    }

    public Continue Navigate => navigation;
}
