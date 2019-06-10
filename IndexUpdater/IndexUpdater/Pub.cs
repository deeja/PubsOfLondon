using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace IndexUpdater
{
    public class Pub
    {
        public string Coordinates { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Photo { get; set; }
    }
}