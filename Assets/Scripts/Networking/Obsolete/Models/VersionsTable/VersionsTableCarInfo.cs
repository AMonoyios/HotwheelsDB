/*
 * Script developed by Andreas Monoyios
 * GitHub: https://github.com/AMonoyios?tab=repositories
 */

using Newtonsoft.Json;
using UnityEngine;

namespace HWAPI
{
    public class VersionsTableCarInfo
    {
        public VersionsTableCarInfo(string colNumber, string year, string series, string color, string tampo, string baseColor, string windowColor, string interiorColor, string wheelType, string toyNumber, string country, string notes, string photo)
        {
            ColNumber = colNumber;
            Year = year;
            Series = series;
            Color = color;
            Tampo = tampo;
            BaseColor = baseColor;
            WindowColor = windowColor;
            InteriorColor = interiorColor;
            WheelType = wheelType;
            ToyNumber = toyNumber;
            Country = country;
            Notes = notes;
            Photo = photo;
        }

        public string ColNumber { get; }
        public string Year { get; }
        public string Series { get; }
        public string Color { get; }
        public string Tampo { get; }
        public string BaseColor { get; }
        public string WindowColor { get; }
        public string InteriorColor { get; }
        public string WheelType { get; }
        public string ToyNumber { get; }
        public string Country { get; }
        public string Notes { get; }
        public string Photo { get; set; }
    }
}
