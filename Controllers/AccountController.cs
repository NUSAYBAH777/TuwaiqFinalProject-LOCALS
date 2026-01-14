using lLOCALS.Data;
using lLOCALS.Models;
using lLOCALS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lLOCALS.Controllers
{
    public class AccountController : Controller
    {
        private readonly LocalsContext _context;

        public AccountController(LocalsContext context)
        {
            _context = context;
        }

        // --- صفحة إنشاء حساب جديد ---

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. التأكد من أن الإيميل غير موجود مسبقاً في الداتا بيس
                var emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "هذا البريد الإلكتروني مستخدم بالفعل.");
                    return View(model);
                }

                // 2. تحويل بيانات الـ ViewModel إلى الموديل الأصلي (User)
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    // تشفير الباسورد فوراً قبل الحفظ
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Role = "User" // دور افتراضي لكل مسجل جديد
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // التوجه لصفحة تسجيل الدخول بعد النجاح
                return RedirectToAction("Login");
            }
            return View(model);
        }

        // --- صفحة تسجيل الدخول ---

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // --- 1. التحقق من دخول الأدمن الثابت (الباب الخلفي) ---
                if (model.Email == "admin@locals.com" && model.Password == "Admin123")
                {
                    HttpContext.Session.SetInt32("UserId", 0); 
                    HttpContext.Session.SetString("UserName", "مدير النظام");
                    HttpContext.Session.SetString("UserRole", "Admin");

                    return RedirectToAction("Dashboard", "Admin"); // يوجهك للوحة تحكم الإدارة
                }

                // --- 2. التحقق من المستخدمين العاديين في قاعدة البيانات ---
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    HttpContext.Session.SetString("UserName", user.FullName);
                    HttpContext.Session.SetString("UserRole", user.Role);

                    // توجيه المستخدم حسب رتبته المخزنة في القاعدة
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (user.Role == "Local Contributor")
                    {
                        return RedirectToAction("Create", "Contributor");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "عذراً، البريد الإلكتروني أو كلمة المرور غير صحيحة");
            }

            return View(model);
        }

        // 1. لعرض الصفحة
        [HttpGet]
        public IActionResult RequestUpgrade()
        {
            // تأكدي أن اليوزر مسجل دخول
            if (HttpContext.Session.GetInt32("UserId") == null) return RedirectToAction("Login");
            return View();
        }

        // 2. لاستقبال البيانات من الفورم
        [HttpPost]
        public async Task<IActionResult> RequestUpgrade(UpgradeRequest model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            model.UserId = userId.Value;
            model.Status = "Pending";
            model.SubmittedAt = DateTime.Now; 

            _context.UpgradeRequests.Add(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "تم إرسال طلبك بنجاح، انتظر موافقة الأدمن!";
            return RedirectToAction("Index", "Home");
        }
        // --- تسجيل الخروج ---

        public IActionResult Logout()
        {
            // مسح كل بيانات الجلسة وإرجاعه لصفحة الدخول
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}