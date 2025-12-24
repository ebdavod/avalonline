using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AvvalOnline.Shop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        /// <summary>
        /// دریافت داده‌های صفحه اصلی (جدیدترین‌ها، پرفروش‌ترین‌ها، محبوب‌ترین‌ها، ویژه‌ها)
        /// </summary>
        [HttpGet]
        public async Task<GetHomePageDataRes> GetHomePageData()
        {
            return await _homeService.GetHomePageDataAsync(null);
        }
    }
}
