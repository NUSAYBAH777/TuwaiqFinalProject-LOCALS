using lLOCALS.Data;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders; // أضفنا هذا المكتبة

var builder = WebApplication.CreateBuilder(args);

// 1. تسجيل خدمة الوصول للسياق (لحل مشكلة الخطأ الحالي)
builder.Services.AddHttpContextAccessor();

// 2. تسجيل خدمة الجلسة (Session) لتعمل الأدوار والدخول
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // مدة الجلسة 30 دقيقة
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 1. إضافة خدمات الـ Controllers والـ Views
builder.Services.AddControllersWithViews();

// 2. إعداد الاتصال بقاعدة البيانات
builder.Services.AddDbContext<LocalsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".otf"] = "application/font-sfnt"; // تعريف ملفات الـ OpenType
provider.Mappings[".ttf"] = "application/font-sfnt";

// تفعيل استخدام الجلسة في التطبيق
app.UseSession();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// --- التعديل الجوهري هنا ---
// 1. تفعيل الملفات الساكنة العادية (css, js)
app.UseStaticFiles();

// 2. إجبار السيرفر على السماح بالوصول لمجلد الصور المرفوعة يدوياً
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "uploads")),
    RequestPath = "/uploads"
});
// -------------------------

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();