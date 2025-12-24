using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.Product;

namespace AvvalOnline.Shop.Api.Services.Interfaces
{
    public interface IHomeService
    {
        Task<GetHomePageDataRes> GetHomePageDataAsync(GetHomePageDataReq req);
    }
    
}
