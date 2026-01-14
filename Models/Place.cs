using System.ComponentModel.DataAnnotations;

namespace lLOCALS.Models
{
    public class Place
    {
        public int PlaceId { get; set; }

        [Required(ErrorMessage = "اسم المكان مطلوب")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // إحداثيات الخريطة
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? LocationDescription { get; set; }

        public string Status { get; set; } = "Pending"; // حالة مراجعة المكان
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // الربط مع التصنيف والحي
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int DistrictId { get; set; }
        public District? District { get; set; }

        // الشخص الذي أضاف المكان
        public int AddedByUserId { get; set; }
        public User? AddedByUser { get; set; }

        // صور المكان
        public ICollection<PlaceImage> Images { get; set; } = new List<PlaceImage>();
    }
}