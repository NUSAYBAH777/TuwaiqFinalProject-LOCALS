using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using lLOCALS.Data;
using lLOCALS.Models;
using Microsoft.EntityFrameworkCore;

namespace lLOCALS.Controllers
{
    public class ContributorController : Controller
    {
        private readonly LocalsContext _context;

        public ContributorController(LocalsContext context)
        {
            _context = context;
        }

        // 1. عرض صفحة إضافة المكان (GET)
        [HttpGet]
        public IActionResult Create()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Local Contributor") return RedirectToAction("Login", "Account");

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewBag.DistrictId = new SelectList(_context.Districts, "DistrictId", "Name");

            return View();
        }

        // 2. استقبال البيانات وحفظ المكان مع الصور (POST)
        // ملاحظة: قمنا بدمج الوظيفتين هنا وحذفنا الدالة المكررة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Place place, List<IFormFile> imageFiles)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                // إعداد بيانات المكان
                place.AddedByUserId = userId.Value;
                place.Status = "Pending";
                place.CreatedAt = DateTime.Now;

                // أولاً: حفظ بيانات المكان الأساسية لتوليد الـ ID
                _context.Places.Add(place);
                await _context.SaveChangesAsync();

                // ثانياً: معالجة الصور (إن وجدت)
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in imageFiles)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // إضافة مسار الصورة لجدول الصور المرتبط بهذا المكان
                        var placeImage = new PlaceImage
                        {
                            PlaceId = place.PlaceId, // نستخدم ID المكان الذي حفظناه للتو
                            ImagePath = "/uploads/" + uniqueFileName
                        };
                        _context.PlaceImages.Add(placeImage);
                    }
                    await _context.SaveChangesAsync(); // حفظ جميع الصور في قاعدة البيانات
                }

                return RedirectToAction("Index", "Home");
            }

            // في حال فشل تسجيل الدخول أو وجود خطأ
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", place.CategoryId);
            ViewBag.DistrictId = new SelectList(_context.Districts, "DistrictId", "Name", place.DistrictId);
            return View(place);
        }

        public async Task<IActionResult> MyPlaces()
        {
            // 1. جلب معرف المستخدم الحالي من الجلسة
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Local Contributor")
            {
                // إذا لم يكن لوكال، وجهه لصفحة الدخول أو الرئيسية
                return RedirectToAction("Login", "Account");
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // 2. جلب الأماكن المرتبطة بهذا المستخدم فقط مع تضمين التصنيف والحي
            var myPlaces = await _context.Places
                .Include(p => p.Category)
                .Include(p => p.District)
                .Where(p => p.AddedByUserId == userId) // الفلترة هنا هي الأهم
                .ToListAsync();

            return View(myPlaces);
        }

        // 1. عرض صفحة التعديل مع البيانات الحالية
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // تأكدي أن الاستعلام يستخدم AddedByUserId كما هو في قاعدة بياناتك
            var place = await _context.Places
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.PlaceId == id && p.AddedByUserId == userId);

            if (place == null) return NotFound();

            // التعديل هنا: استخدام CategoryId و DistrictId بدلاً من Id
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", place.CategoryId);
            ViewBag.DistrictId = new SelectList(_context.Districts, "DistrictId", "Name", place.DistrictId);

            return View(place);
        }

        // 2. استقبال التعديلات وحفظها
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Place place, List<IFormFile> imageFiles, int[] deletedImageIds)
        {
            if (id != place.PlaceId) return NotFound();

            try
            {
                // 1. معالجة حذف الصور المحددة
                if (deletedImageIds != null && deletedImageIds.Length > 0)
                {
                    foreach (var imageId in deletedImageIds)
                    {
                        var image = await _context.PlaceImages.FindAsync(imageId);
                        if (image != null)
                        {
                            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                            _context.PlaceImages.Remove(image);
                        }
                    }
                }

                // 2. معالجة إضافة الصور الجديدة
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in imageFiles)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                        _context.PlaceImages.Add(new PlaceImage { PlaceId = id, ImagePath = "/uploads/" + uniqueFileName });
                    }
                }

                // 3. تحديث بيانات المكان (مع تغيير الحالة إلى Pending ليراجعها الأدمن مرة أخرى إذا رغبتِ)
                place.Status = "Pending";
                _context.Update(place);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyPlaces));
            }
            catch (Exception ex)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", place.CategoryId);
                ViewBag.DistrictId = new SelectList(_context.Districts, "DistrictId", "Name", place.DistrictId);
                return View(place);
            }
        }
    }
}