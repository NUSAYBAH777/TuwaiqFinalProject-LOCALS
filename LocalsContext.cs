using System.Collections.Generic;
using System.Reflection.Emit;
using lLOCALS.Models;
using Microsoft.EntityFrameworkCore;

namespace lLOCALS.Data
{
    public class LocalsContext : DbContext
    {
        public LocalsContext(DbContextOptions<LocalsContext> options) : base(options) { }

        // جداول قاعدة البيانات الأساسية
        public DbSet<User> Users { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<PlaceImage> PlaceImages { get; set; }
        public DbSet<UpgradeRequest> UpgradeRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "مطاعم وكافيهات" },
                new Category { CategoryId = 2, Name = "أماكن ترفيهية" },
                new Category { CategoryId = 3, Name = "حدائق ومنتزهات" },
                new Category { CategoryId = 4, Name = "مراكز تسوق" },
                new Category { CategoryId = 5, Name = "متاحف ومعالم تاريخية" },
                new Category { CategoryId = 6, Name = "أنشطة رياضية" },
                new Category { CategoryId = 7, Name = "فعاليات ومواسم" },
                new Category { CategoryId = 8, Name = "فنادق وإقامات" }
            );

            // هذه تساعد المستخدم في الفلترة بجانب الخريطة
            modelBuilder.Entity<District>().HasData(
                new District { DistrictId = 1, Name = "حي الملقا" },
                new District { DistrictId = 2, Name = "حي حطين" },
                new District { DistrictId = 3, Name = "حي النخيل" },
                new District { DistrictId = 4, Name = "حي الدرعية" },
                new District { DistrictId = 5, Name = "حي العليا" },
                new District { DistrictId = 6, Name = "حي الياسمين" },
                new District { DistrictId = 7, Name = "حي النرجس" },
                new District { DistrictId = 8, Name = "حي السليمانية" }
            );
        }
    }
}