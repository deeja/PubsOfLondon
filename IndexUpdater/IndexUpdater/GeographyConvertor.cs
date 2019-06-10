using System;
using System.Linq;
using Microsoft.Spatial;

namespace IndexUpdater
{
    public static class GeographyConvertor
    {
        public static GeographyPoint ConvertFromString(string text)
        {
            // 51.512656°N 0.13660272°W

            var strings = text.Split(' ');

            double latitude = GetAsValue(strings[0]);
            double longitude = GetAsValue(strings[1]);
            return GeographyPoint.Create(latitude, longitude);
        }

        /// <summary>
        /// When includes these letters, use the inverse lat lon
        /// </summary>
        static readonly string[] Negatives = { "S", "W" };

        private static double GetAsValue(string str)
        {
            var split = str.Split('°');
            var value = Convert.ToDouble(split[0]);
            if (Negatives.Any(s => s.Equals(split[1], StringComparison.InvariantCultureIgnoreCase)))
            {
                return 0 - value;
            }

            return value;
        }
    }
}