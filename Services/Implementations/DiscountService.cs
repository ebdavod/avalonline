using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.Discount;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly ShopDbContext _db;

        public DiscountService(ShopDbContext db)
        {
            _db = db;
        }

        public async Task<CreateDiscountRes> CreateDiscountAsync(CreateDiscountReq req)
        {
            var dto = req.Entity;
            var discount = new Discount
            {
                Code = dto.Code,
                Description = dto.Description,
                Percentage = dto.Percentage,
                MaxAmount = dto.MaxAmount,
                UsageLimit = dto.UsageLimit,
                UsedCount = 0,
                IsActive = true,
                ExpireDate = dto.ExpireDate
            };

            _db.Discounts.Add(discount);
            await _db.SaveChangesAsync();

            dto.Id = discount.Id;
            return new CreateDiscountRes { IsSuccess = true, Entity = dto };
        }

        public async Task<UpdateDiscountRes> UpdateDiscountAsync(UpdateDiscountReq req)
        {
            var dto = req.Entity;
            var discount = await _db.Discounts.FirstOrDefaultAsync(d => d.Id == dto.Id);
            if (discount == null)
                return new UpdateDiscountRes { IsSuccess = false, Message = "تخفیف یافت نشد" };

            discount.Description = dto.Description;
            discount.Percentage = dto.Percentage;
            discount.MaxAmount = dto.MaxAmount;
            discount.UsageLimit = dto.UsageLimit;
            discount.IsActive = dto.IsActive;
            discount.ExpireDate = dto.ExpireDate;

            await _db.SaveChangesAsync();
            return new UpdateDiscountRes { IsSuccess = true, Message = "تخفیف بروزرسانی شد" };
        }

        public async Task<DeleteDiscountRes> DeleteDiscountAsync(DeleteDiscountReq req)
        {
            var discount = await _db.Discounts.FirstOrDefaultAsync(d => d.Id == req.Id);
            if (discount == null)
                return new DeleteDiscountRes { IsSuccess = false, Message = "تخفیف یافت نشد" };

            _db.Discounts.Remove(discount);
            await _db.SaveChangesAsync();
            return new DeleteDiscountRes { IsSuccess = true, Message = "تخفیف حذف شد" };
        }

        public async Task<GetDiscountByCodeRes> GetDiscountByCodeAsync(GetDiscountByCodeReq req)
        {
            var discount = await _db.Discounts.FirstOrDefaultAsync(d => d.Code == req.Entity);
            if (discount == null || !discount.IsActive || discount.ExpireDate < DateTime.UtcNow)
                return new GetDiscountByCodeRes { IsSuccess = false, Message = "کد تخفیف نامعتبر است" };

            var dto = new DiscountDTO
            {
                Id = discount.Id,
                Code = discount.Code,
                Description = discount.Description,
                Percentage = discount.Percentage,
                MaxAmount = discount.MaxAmount,
                UsageLimit = discount.UsageLimit,
                UsedCount = discount.UsedCount,
                IsActive = discount.IsActive,
                ExpireDate = discount.ExpireDate
            };

            return new GetDiscountByCodeRes { IsSuccess = true, Entity = dto };
        }

        public async Task<GetAllDiscountsRes> GetAllDiscountsAsync(GetAllDiscountsReq req)
        {
            var discounts = await _db.Discounts
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            var dtos = discounts.Select(d => new DiscountDTO
            {
                Id = d.Id,
                Code = d.Code,
                Description = d.Description,
                Percentage = d.Percentage,
                MaxAmount = d.MaxAmount,
                UsageLimit = d.UsageLimit,
                UsedCount = d.UsedCount,
                IsActive = d.IsActive,
                ExpireDate = d.ExpireDate
            }).ToList();

            return new GetAllDiscountsRes { IsSuccess = true, Entities = dtos, TotalCount = await _db.Discounts.CountAsync() };
        }
    }
}
