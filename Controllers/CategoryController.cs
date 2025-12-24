using AvvalOnline.Shop.Api.Attributes;
using AvvalOnline.Shop.Api.Messaging.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvvalOnline.Shop.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<CreateCategoryRes> CreateCategory(CreateCategoryReq req)
        {
            return await _categoryService.CreateCategoryAsync(req);
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<DeleteCategoryRes> DeleteCategory(DeleteCategoryReq req)
        {
            return await _categoryService.DeleteCategoryAsyc(req);
        }
        [HttpGet]
        public async Task<GetAllCategoryRes> GetAllCategories(int index, int size)
        {
            return await _categoryService.GetAllCategoriesAsync(new GetAllCategoryReq
            {
                Page = index,
                PageSize = size,
            });
        }

        [HttpPost]
        [CustomAuthorize(["Admin"])]
        public async Task<UpdateCategoryRes> UpdateCategory(UpdateCategoryReq req)
        {
            return await _categoryService.UpdateCategoryAsync(req);
        }
    }
}
