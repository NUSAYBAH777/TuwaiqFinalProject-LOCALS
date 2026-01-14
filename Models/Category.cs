namespace lLOCALS.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;

        // علاقة: التصنيف الواحد يحتوي على عدة أماكن
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}