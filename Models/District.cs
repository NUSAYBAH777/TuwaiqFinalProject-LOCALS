namespace lLOCALS.Models
{
    public class District
    {
        public int DistrictId { get; set; }
        public string Name { get; set; } = string.Empty;

        // علاقة: الحي الواحد يحتوي على عدة أماكن
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}