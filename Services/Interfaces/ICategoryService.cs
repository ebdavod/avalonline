using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.Category;

public interface ICategoryService
{
    //Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<GetAllCategoryRes> GetAllCategoriesAsync(GetAllCategoryReq req);
    Task<CreateCategoryRes> CreateCategoryAsync(CreateCategoryReq req);
    Task<UpdateCategoryRes> UpdateCategoryAsync(UpdateCategoryReq req);
    Task<DeleteCategoryRes> DeleteCategoryAsyc(DeleteCategoryReq req);
    //Task<List<CategoryDto>> GetSubCategoriesAsync(int parentCategoryId);
}
