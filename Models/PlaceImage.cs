namespace lLOCALS.Models
{
    public class PlaceImage
    {
        public int PlaceImageId { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public int PlaceId { get; set; }
        public Place? Place { get; set; }
    }
}