using AvvalOnline.Shop.Api.Messaging.Category;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        ShopDbContext _db;
        public CategoryService(ShopDbContext context)
        {
            _db = context;
        }
        public async Task<CreateCategoryRes> CreateCategoryAsync(CreateCategoryReq req)
        {
            var category = _db.ProductCategories.AddAsync(new Model.Entites.ProdcutCategory
            {
                Description = req.Entity.Description,
                IsActive = req.Entity.IsActive,
                Name = req.Entity.Name,
                ParentCategoryId = req.Entity.ParentCategoryId,
            });
            await _db.SaveChangesAsync();
            return new CreateCategoryRes
            {
                IsSuccess = true,
                Message = "دسته بندی با موفقیت ایجاد شد"
            };
        }

        public async Task<DeleteCategoryRes> DeleteCategoryAsyc(DeleteCategoryReq req)
        {
            var deletedRecords = await _db.ProductCategories.Where(x => x.Id == req.Id).ExecuteDeleteAsync();
            return new DeleteCategoryRes
            {
                IsSuccess = deletedRecords > 0,
                Message = deletedRecords > 0 ? "دسته بندی با موفقیت حذف شد" : "حذف دسته بندی انجام نشد",
            };
        }

        public async Task<GetAllCategoryRes> GetAllCategoriesAsync(GetAllCategoryReq req)
        {
            Func<ProdcutCategory, CategoryDTO> getCat = null;
            getCat = (ProdcutCategory pc) =>
            {
                return new CategoryDTO
                {
                    Description = pc.Description,
                    Id = pc.Id,
                    IsActive = pc.IsActive,
                    Name = pc.Name,
                    ParentCategoryId = pc.ParentCategoryId,
                    SubCategories = pc.SubCategories.Select(m => getCat(m)).ToList(),
                };
            };

            var lst = await _db.ProductCategories
                .Include(x => x.SubCategories)
                .Select(x => getCat(x)).ToListAsync();

            return new GetAllCategoryRes
            {
                Entities = lst,
                IsSuccess = true,
                Message = "عملیات با موفقیت انجام شد",
                TotalCount = lst.Count,
                Page = 1,
                PageSize = lst.Count
            };
        }

        public async Task<UpdateCategoryRes> UpdateCategoryAsync(UpdateCategoryReq req)
        {
            var category = await _db.ProductCategories.FindAsync(req.Entity.Id);
            if (category is null)
            {
                return new UpdateCategoryRes
                {
                    Message = "دسته بندی مورد نظر یافت نشد"
                };
            }
            category.Name = req.Entity.Name;
            category.ParentCategoryId = req.Entity.ParentCategoryId;
            category.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return new UpdateCategoryRes
            {
                IsSuccess = true,
                Message="ویرایش دسته بندی با موفقیت انجام شد"
            };
        }
    }
}
