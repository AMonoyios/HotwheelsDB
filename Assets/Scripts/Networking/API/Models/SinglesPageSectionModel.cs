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
        private readonly Parse parse;

        private class Parse
        {
            [JsonProperty("title")]
            public string title;

            [JsonProperty("links")]
            public List<Links> cars;

            [JsonProperty("images")]
            public List<string> images;
        }

        private class Links
        {
            [JsonProperty("*")]
            public string name;
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

        public int TotalCars
        {
            get
            {
                return parse.cars.Count;
            }
        }

        public Car GetCar(int index)
        {
            if (index > parse.cars.Count || index > parse.images.Count)
            {
                CoreLogger.LogError($"Index for page section is out of bounds. Index: {index}, Max possible index: cars => {parse.cars.Count} images => {parse.images.Count}");
                return null;
            }

            string name = parse.cars[index].name;
            string image = parse.images[index];

            return new(name, image);
        }
    }
}
