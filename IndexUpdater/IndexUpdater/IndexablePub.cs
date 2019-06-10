using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;

namespace IndexUpdater
{
    [SerializePropertyNamesAsCamelCase]
    public class IndexablePub
    {
        public IndexablePub(Pub pub)
        {
            Key = pub.Name.GetHashCode().ToString();
            Coordinates = GeographyConvertor.ConvertFromString(pub.Coordinates);
            Name = pub.Name;
            Link = pub.Link;
            Photo = pub.Photo;
        }

        [System.ComponentModel.DataAnnotations.Key]
        public string Key { get; set; }

        public GeographyPoint Coordinates { get; set; }
        [IsFilterable, IsSortable, IsSearchable]
        public string Name { get; set; }
        public string Link { get; set; }
        public string Photo { get; set; }
    }
}