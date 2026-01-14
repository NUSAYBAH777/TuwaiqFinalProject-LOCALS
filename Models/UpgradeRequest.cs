namespace lLOCALS.Models
{
    public class UpgradeRequest
    {
        public int UpgradeRequestId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Examples { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; 
        public DateTime SubmittedAt { get; set; } = DateTime.Now;

        // ربط الطلب بالمستخدم
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}