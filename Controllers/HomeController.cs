using System.Diagnostics;
using lLOCAlS.Models;
using lLOCALS.Data; // تأكدي من كتابة اسم الـ Namespace الخاص ببياناتك بشكل صحيح
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lLOCALS.Controllers
{
    public class HomeController : Controller
    {
        private readonly LocalsContext _context;

        // إضافة الـ Context للوصول لقاعدة البيانات
        public HomeController(LocalsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // جلب الأماكن المقبولة فقط مع صورها وتصنيفاتها وأحيائها
            var approvedPlaces = await _context.Places
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.District)
                .Where(p => p.Status == "Approved")
                .ToListAsync();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.AllDistricts = _context.Districts.ToList();

            return View(approvedPlaces);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}