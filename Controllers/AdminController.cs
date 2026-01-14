using lLOCALS.Data;
using lLOCALS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lLOCALS.Controllers
{
    public class AdminController : Controller
    {
        private readonly LocalsContext _context;

        public AdminController(LocalsContext context)
        {
            _context = context;
        }

        // --- لوحة التحكم الشاملة ---
        public async Task<IActionResult> Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Login", "Account");

            // 1. جلب طلبات ترقية المستخدمين
            var upgradeRequests = await _context.UpgradeRequests
                                                .Include(r => r.User)
                                                .Where(r => r.Status == "Pending")
                                                .ToListAsync();

            // 2. جلب طلبات إضافة الأماكن (التي ستظهر في الجدول الرئيسي)
            var pendingPlaces = await _context.Places
                                             .Include(p => p.Category)
                                             .Include(p => p.District)
                                             .Include(p => p.Images) // تأكدي إن الاسم في الموديل PlaceImages وليس Images
                                             .Where(p => p.Status == "Pending")
                                             .ToListAsync();

            // --- إضافة الإحصائيات للـ ViewBag لتعمل الكروت العلوية ---

            // إجمالي المعالم المعتمدة (التي تظهر للمستخدمين)
            ViewBag.PlacesCount = await _context.Places.CountAsync(p => p.Status == "Approved");

            // عدد المساهمين المحليين المسجلين فعلياً
            ViewBag.ContributorsCount = await _context.Users.CountAsync(u => u.Role == "Local Contributor");

            // عدد طلبات الترقية (نفس القائمة اللي جلبناها فوق)
            ViewBag.UpgradeRequests = upgradeRequests;

            return View(pendingPlaces);
        }

        // --- إجراءات طلبات الترقية (Users) ---

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var request = await _context.UpgradeRequests.FindAsync(requestId);
            if (request != null)
            {
                // ترقية دور المستخدم في جدول Users
                var user = await _context.Users.FindAsync(request.UserId);
                if (user != null)
                {
                    user.Role = "Local Contributor";
                }

                request.Status = "Approved"; // تحديث حالة الطلب
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var request = await _context.UpgradeRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = "Rejected"; // أو يمكنك استخدام Remove لحذفه نهائياً
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        // --- إجراءات الأماكن (Places) ---

        [HttpPost]
        public async Task<IActionResult> ApprovePlace(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place != null)
            {
                place.Status = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpPost]
        public async Task<IActionResult> RejectPlace(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place != null)
            {
                _context.Places.Remove(place);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Dashboard));
        }
    }
}