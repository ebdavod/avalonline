using AvvalOnline.Shop.Api.Messaging.Discount;

namespace AvvalOnline.Shop.Api.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<CreateDiscountRes> CreateDiscountAsync(CreateDiscountReq req);
        Task<UpdateDiscountRes> UpdateDiscountAsync(UpdateDiscountReq req);
        Task<DeleteDiscountRes> DeleteDiscountAsync(DeleteDiscountReq req);
        Task<GetDiscountByCodeRes> GetDiscountByCodeAsync(GetDiscountByCodeReq req);
        Task<GetAllDiscountsRes> GetAllDiscountsAsync(GetAllDiscountsReq req);
    }
}
