using AvvalOnline.Shop.Api.Model.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Model
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProdcutCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }
        public DbSet<Discount> Discounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.PhoneNumber).IsUnique();
                entity.HasIndex(u => u.NationalCode).IsUnique();

                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PhoneNumber).HasMaxLength(15);
                entity.Property(u => u.FirstName).HasMaxLength(50);
                entity.Property(u => u.LastName).HasMaxLength(50);
                entity.Property(u => u.NationalCode).HasMaxLength(10);

                entity.HasMany(u => u.UserRoles)
                      .WithOne(ur => ur.User)
                      .HasForeignKey(ur => ur.UserId);

                entity.HasMany(u => u.Orders)
                      .WithOne(o => o.User)
                      .HasForeignKey(o => o.UserId);
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasIndex(r => r.Name).IsUnique();

                entity.Property(r => r.Name).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Description).HasMaxLength(255);

                entity.HasMany(r => r.UserRoles)
                      .WithOne(ur => ur.Role)
                      .HasForeignKey(ur => ur.RoleId);

                entity.HasMany(r => r.RolePermissions)
                      .WithOne(rp => rp.Role)
                      .HasForeignKey(rp => rp.RoleId);
            });

            // Permission Configuration
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.Name).IsUnique();

                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(255);
                entity.Property(p => p.Category).HasMaxLength(50);

                entity.HasMany(p => p.RolePermissions)
                      .WithOne(rp => rp.Permission)
                      .HasForeignKey(rp => rp.PermissionId);
            });

            // UserRole Configuration
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => ur.Id);

                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId);
            });

            // RolePermission Configuration
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(rp => rp.Id);

                entity.HasOne(rp => rp.Role)
                      .WithMany(r => r.RolePermissions)
                      .HasForeignKey(rp => rp.RoleId);

                entity.HasOne(rp => rp.Permission)
                      .WithMany(p => p.RolePermissions)
                      .HasForeignKey(rp => rp.PermissionId);
            });

            // Vehicle Configuration
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.HasIndex(v => v.PlateNumber).IsUnique();
                entity.HasIndex(v => v.VIN).IsUnique();

                entity.Property(v => v.PlateNumber).IsRequired().HasMaxLength(20);
                entity.Property(v => v.VIN).HasMaxLength(50);
                entity.Property(v => v.Brand).HasMaxLength(50);
                entity.Property(v => v.Model).HasMaxLength(50);
                entity.Property(v => v.Color).HasMaxLength(30);
                entity.Property(v => v.EngineNumber).HasMaxLength(50);

                entity.HasOne(v => v.User)
                      .WithMany(c => c.Vehicles)
                      .HasForeignKey(v => v.UserId);

                entity.HasMany(v => v.Orders)
                      .WithOne(o => o.Vehicle)
                      .HasForeignKey(o => o.VehicleId);
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.Code).IsUnique();
                entity.HasIndex(p => p.Name);

                entity.Property(p => p.Code).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasColumnType("nvarchar(max)");

                entity.HasOne(p => p.Category)
                      .WithMany(pc => pc.Products)
                      .HasForeignKey(p => p.CategoryId);

                entity.HasMany(p => p.Images)
                      .WithOne(pi => pi.Product)
                      .HasForeignKey(pi => pi.ProductId);

                entity.HasMany(p => p.OrderItems)
                      .WithOne(oi => oi.Product)
                      .HasForeignKey(oi => oi.ProductId);
            });

            // ProductCategory Configuration
            modelBuilder.Entity<ProdcutCategory>(entity =>
            {
                entity.HasKey(pc => pc.Id);
                entity.HasIndex(pc => pc.Name).IsUnique();

                entity.Property(pc => pc.Name).IsRequired().HasMaxLength(100);
                entity.Property(pc => pc.Description).HasMaxLength(500);

                entity.HasOne(pc => pc.ParentCategory)
                      .WithMany(pc => pc.SubCategories)
                      .HasForeignKey(pc => pc.ParentCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(pc => pc.Products)
                      .WithOne(p => p.Category)
                      .HasForeignKey(p => p.CategoryId);
            });

            // ProductImage Configuration
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(pi => pi.Id);

                entity.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(pi => pi.AltText).HasMaxLength(200);

                entity.HasOne(pi => pi.Product)
                      .WithMany(p => p.Images)
                      .HasForeignKey(pi => pi.ProductId);
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasIndex(o => o.Code).IsUnique();

                entity.Property(o => o.Code).IsRequired().HasMaxLength(20);

                entity.HasOne(o => o.Vehicle)
                      .WithMany(v => v.Orders)
                      .HasForeignKey(o => o.VehicleId);

                entity.HasOne(o => o.User)
                      .WithMany(u => u.Orders)
                      .HasForeignKey(o => o.UserId);

                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId);
            });

            // OrderItem Configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.Property(oi => oi.SelectedSize).HasMaxLength(20);
                entity.Property(oi => oi.InstallationType).HasMaxLength(50);

                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId);

                entity.HasOne(oi => oi.Product)
                      .WithMany(p => p.OrderItems)
                      .HasForeignKey(oi => oi.ProductId);
            });

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // نقش‌های پیش‌فرض
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "SuperAdmin", Description = "مدیر کل سیستم", CreatedAt = DateTime.UtcNow },
                new Role { Id = 2, Name = "Admin", Description = "مدیر سیستم", CreatedAt = DateTime.UtcNow },
                new Role { Id = 3, Name = "Manager", Description = "مدیر فروش", CreatedAt = DateTime.UtcNow },
                new Role { Id = 4, Name = "User", Description = "کاربر", CreatedAt = DateTime.UtcNow },
                new Role { Id = 5, Name = "Support", Description = "پشتیبان", CreatedAt = DateTime.UtcNow }
            );

            // کاربر ادمین پیش‌فرض
            var adminUser = new User
            {
                Id = 1,
                Username = "09134474057",
                Email = "ebdavod@gmail.com",
                PhoneNumber = "09134474057",
                FirstName = "داود",
                LastName = "ابراهیمی",
                NationalCode = "4420928565",
                Address = "یزد",
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "14021402"),
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<User>().HasData(adminUser);

            // اختصاص نقش ادمین
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { Id = 1, UserId = 1, RoleId = 1, AssignedAt = DateTime.UtcNow }
            );

            // دسترسی‌های پیش‌فرض
            modelBuilder.Entity<Permission>().HasData(
                // Product Permissions
                new Permission { Id = 1, Name = "Product.View", Description = "مشاهده محصولات", Category = "Product" },
                new Permission { Id = 2, Name = "Product.Create", Description = "ایجاد محصول", Category = "Product" },
                new Permission { Id = 3, Name = "Product.Edit", Description = "ویرایش محصول", Category = "Product" },
                new Permission { Id = 4, Name = "Product.Delete", Description = "حذف محصول", Category = "Product" },

                // Order Permissions
                new Permission { Id = 5, Name = "Order.View", Description = "مشاهده سفارشات", Category = "Order" },
                new Permission { Id = 6, Name = "Order.Create", Description = "ایجاد سفارش", Category = "Order" },
                new Permission { Id = 7, Name = "Order.Edit", Description = "ویرایش سفارش", Category = "Order" },
                new Permission { Id = 8, Name = "Order.Cancel", Description = "لغو سفارش", Category = "Order" },

                // User Permissions
                new Permission { Id = 9, Name = "User.View", Description = "مشاهده کاربران", Category = "User" },
                new Permission { Id = 10, Name = "User.Create", Description = "ایجاد کاربر", Category = "User" },
                new Permission { Id = 11, Name = "User.Edit", Description = "ویرایش کاربر", Category = "User" },
                new Permission { Id = 12, Name = "User.Delete", Description = "حذف کاربر", Category = "User" }
            );

            // اختصاص دسترسی‌ها به نقش ادمین
            var adminPermissions = new List<RolePermission>();
            for (int i = 1; i <= 12; i++)
            {
                adminPermissions.Add(new RolePermission
                {
                    Id = i,
                    RoleId = 1,
                    PermissionId = i,
                    GrantedAt = DateTime.UtcNow
                });
            }
            modelBuilder.Entity<RolePermission>().HasData(adminPermissions);

        }
    } 
}