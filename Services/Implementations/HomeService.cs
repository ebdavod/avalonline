using AvvalOnline.Shop.Api.Messaging.Product;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class HomeService : IHomeService
    {
        private readonly ShopDbContext _db;

        public HomeService(ShopDbContext db)
        {
            _db = db;
        }

        public async Task<GetHomePageDataRes> GetHomePageDataAsync(GetHomePageDataReq req)
        {
            int count = 10;

            // ✅ جدیدترین محصولات
            var newest = await _db.Products
                .Include(x => x.Images)
                .Include(x => x.Category)
                .OrderByDescending(x => x.CreatedAt)
                .Take(count)
                .ToListAsync();

            // ✅ پرفروش‌ترین‌ها (Best Sellers)
            var bestSellers = await _db.OrderItems
                .Include(x => x.Product)
                .ThenInclude(p => p.Images)
                .Include(x => x.Product.Category)
                .GroupBy(x => x.ProductId)
                .Select(g => new
                {
                    Product = g.First().Product,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(count)
                .ToListAsync();

            // ✅ محبوب‌ترین‌ها (اگر ViewCount داری)
            var popular = await _db.Products
                .Include(x => x.Images)
                .Include(x => x.Category)
                //.OrderByDescending(x => x.ViewCount) // اگر نداری حذف کن
                .Take(count)
                .ToListAsync();

            // ✅ محصولات ویژه (Active + Available)
            var featured = await _db.Products
                .Include(x => x.Images)
                .Include(x => x.Category)
                .Where(x => x.IsActive && x.IsAvailable)
                .OrderByDescending(x => x.CreatedAt)
                .Take(count)
                .ToListAsync();

            return new GetHomePageDataRes
            {
                IsSuccess = true,
                Message = "Success",
                Entity = new Messaging.Home.HomePageDTO
                {
                    Newest = newest.Select(ToDto).ToList(),
                    BestSellers = bestSellers.Select(x => ToDto(x.Product)).ToList(),
                    Popular = popular.Select(ToDto).ToList(),
                    Featured = featured.Select(ToDto).ToList()
                }
            };
        }

        // ✅ تبدیل Product به DTO
        private ProductDTO ToDto(Product x)
        {
            return new ProductDTO
            {
                Id = x.Id,
                CategoryName = x.Category?.Name,
                CategoryId = x.CategoryId,
                Code = x.Code,
                Description = x.Description,
                IsActive = x.IsActive,
                IsAvailable = x.IsAvailable,
                Name = x.Name,
                Price = x.Price,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Images = x.Images.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText,
                    DisplayOrder = img.DisplayOrder,
                    IsMain = img.IsMain
                }).ToList()
            };
        }
    }
}
