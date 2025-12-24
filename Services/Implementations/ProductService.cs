using AvvalOnline.Shop.Api.Messaging.Product;
using AvvalOnline.Shop.Api.Model;
using AvvalOnline.Shop.Api.Model.Entites;
using AvvalOnline.Shop.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AvvalOnline.Shop.Api.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _db;
        private readonly ILogger<ProductService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly string _productImagesPath;

        public ProductService(
            ShopDbContext context,
            ILogger<ProductService> logger,
            IWebHostEnvironment environment)
        {
            _db = context;
            _logger = logger;
            _environment = environment;
            _productImagesPath = Path.Combine(_environment.WebRootPath ?? _environment.ContentRootPath, "ProductPictures");

            if (!Directory.Exists(_productImagesPath))
                Directory.CreateDirectory(_productImagesPath);
        }

        private string GetPublicImageUrl(string fileName)
        {
            return $"/ProductPictures/{fileName}";
        }

        public async Task<CreateProductRes> CreateProductAsync(CreateProductReq req)
        {
            string code = await GenerateCode();
            var categoryIsExist = await _db.ProductCategories.FindAsync(req.Entity.CategoryId);
            if (categoryIsExist is null)
                return new CreateProductRes
                {
                    Message = "دسته بندی وارد شده موجود نمیباشد"
                };
            var product = new Product
            {
                CategoryId = req.Entity.CategoryId,
                Code = code,
                Description = req.Entity.Description,
                Name = req.Entity.Name,
                Price = req.Entity.Price,
                IsAvailable = req.Entity.IsAvailable,
                IsActive = req.Entity.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();

            if (req.Entity.Images is not null && req.Entity.Images.Any())
            {
                foreach (var item in req.Entity.Images)
                {
                    var fileName = CreateFile(code, item.ImageUrl);
                    if (fileName != null)
                    {
                        product.Images.Add(new ProductImage
                        {
                            ImageUrl = GetPublicImageUrl(Path.GetFileName(fileName)),
                            AltText = item.AltText,
                            DisplayOrder = item.DisplayOrder,
                            IsMain = item.IsMain
                        });
                    }
                }
                await _db.SaveChangesAsync();
            }

            return new CreateProductRes
            {
                IsSuccess = true,
                Message = "محصول با موفقیت ثبت شد"
            };
        }

        public async Task<DeleteProductRes> DeleteProductAsync(DeleteProductReq req)
        {
            var entity = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == req.Id);
            if (entity is null)
            {
                return new DeleteProductRes
                {
                    IsSuccess = false,
                    Message = "محصول یافت نشد"
                };
            }

            try
            {
                foreach (var img in entity.Images)
                {
                    var picturePath = Path.Combine(_productImagesPath, Path.GetFileName(img.ImageUrl));
                    if (File.Exists(picturePath))
                        File.Delete(picturePath);
                }

                _db.Products.Remove(entity);
                await _db.SaveChangesAsync();

                return new DeleteProductRes
                {
                    IsSuccess = true,
                    Message = "محصول حذف شد"
                };
            }
            catch (Exception ex)
            {
                return new DeleteProductRes
                {
                    IsSuccess = false,
                    Message = "خطا در حذف محصول: " + ex.Message
                };
            }
        }

        public async Task<GetAllProductsRes> GetAllProductsAsync(GetAllProductsReq req)
        {
            var list = await _db.Products
                .Include(x => x.Category)
                .Include(x => x.Images)
                .OrderBy(x => x.Price)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new GetAllProductsRes
            {
                IsSuccess = true,
                Entities = list.Select(x => new ProductDTO
                {
                    Id = x.Id,
                    CategoryName = x.Category.Name,
                    CategoryId = x.Category.Id,
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
                }).ToList(),
                Message = "Success",
                TotalCount = await _db.Products.CountAsync(),
                Page = req.Page,
                PageSize = req.PageSize
            };
        }

        public async Task<GetProductByIdRes> GetProductByIdAsync(GetProductByIdReq req)
        {
            var product = await _db.Products
                .Include(x => x.Images)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == req.Id);

            if (product == null)
            {
                return new GetProductByIdRes
                {
                    IsSuccess = false,
                    Message = "محصول یافت نشد"
                };
            }

            var categoryFullName = await BuildCategoryFullName(product.CategoryId);

            var dto = new ProductDTO
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                CategoryFullName = categoryFullName,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                IsActive = product.IsActive,
                IsAvailable = product.IsAvailable,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                Images = product.Images.Select(img => new ProductImageDto
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    AltText = img.AltText,
                    DisplayOrder = img.DisplayOrder,
                    IsMain = img.IsMain
                }).ToList()
            };

            return new GetProductByIdRes
            {
                IsSuccess = true,
                Entity = dto,
                Message = "Success"
            };
        }

        public async Task<UpdateProductRes> UpdateProductAsync(UpdateProductReq req)
        {
            var entity = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == req.Entity.Id);
            if (entity is null)
            {
                return new UpdateProductRes { IsSuccess = false, Message = "محصول یافت نشد" };
            }

            entity.Description = req.Entity.Description;
            entity.CategoryId = req.Entity.CategoryId;
            entity.Price = req.Entity.Price;
            entity.Name = req.Entity.Name;
            entity.IsAvailable = req.Entity.IsAvailable;
            entity.IsActive = req.Entity.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            var currentPictures = new List<string>();
            foreach (var item in req.Entity.Images)
            {
                var fileName = item.ImageUrl;
                if (IsBase64String(item.ImageUrl))
                    fileName = CreateFile(entity.Code, item.ImageUrl);

                if (fileName != null)
                {
                    currentPictures.Add(Path.GetFileName(fileName));
                    if (!entity.Images.Any(i => i.ImageUrl.EndsWith(Path.GetFileName(fileName))))
                    {
                        entity.Images.Add(new ProductImage
                        {
                            ImageUrl = GetPublicImageUrl(Path.GetFileName(fileName)),
                            AltText = item.AltText,
                            DisplayOrder = item.DisplayOrder,
                            IsMain = item.IsMain
                        });
                    }
                }
            }

            foreach (var oldPic in entity.Images.Where(i => !currentPictures.Contains(Path.GetFileName(i.ImageUrl))).ToList())
            {
                var path = Path.Combine(_productImagesPath, Path.GetFileName(oldPic.ImageUrl));
                if (File.Exists(path))
                    File.Delete(path);

                entity.Images.Remove(oldPic);
            }

            await _db.SaveChangesAsync();
            return new UpdateProductRes
            {
                IsSuccess = true,
                Message = "محصول بروزرسانی شد"
            };

        }

        public async Task<GetProductByCodeRes> GetProductByCodeAsync(GetProductByCodeReq req)
        {
            var product = await _db.Products
                .Include(x => x.Images)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Code == req.Entity.Code);

            if (product is null)
            {
                return new GetProductByCodeRes
                {
                    IsSuccess = false,
                    Message = "محصول یافت نشد"
                };
            }

            var categoryFullName = await BuildCategoryFullName(product.CategoryId);

            return new GetProductByCodeRes
            {
                IsSuccess = true,
                Entity = new ProductDTO
                {
                    Id = product.Id,
                    CategoryName = product.Category.Name,
                    CategoryFullName = categoryFullName,
                    CategoryId = product.Category.Id,
                    Code = product.Code,
                    Description = product.Description,
                    IsActive = product.IsActive,
                    IsAvailable = product.IsAvailable,
                    Name = product.Name,
                    Price = product.Price,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                    Images = product.Images.Select(x => new ProductImageDto
                    {
                        ImageUrl = x.ImageUrl, // حالا فقط URL عمومی ذخیره میشه
                        AltText = x.AltText,
                        DisplayOrder = x.DisplayOrder,
                        Id = x.Id,
                        IsMain = x.IsMain,
                    }).ToList(),
                },
                Message = "Success"
            };
        }

        private string CreateFile(string name, string base64)
        {
            if (!Directory.Exists(_productImagesPath))
                Directory.CreateDirectory(_productImagesPath);

            var existPictures = Directory.GetFiles(_productImagesPath).Where(x => Path.GetFileName(x).Contains(name));
            int lastIdx = 0;
            if (existPictures.Any())
            {
                lastIdx = existPictures.Select(x =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(x);
                    if (int.TryParse(fileName.Split("_")[1], out int res))
                        return res;
                    else
                        return 1;
                }).LastOrDefault();
            }

            var fileName = name + $"_{lastIdx + 1}.png";
            var path = Path.Combine(_productImagesPath, fileName);

            if (!File.Exists(path))
            {
                using (var newFile = File.Create(path))
                {
                    if (base64.Contains(",")) base64 = base64.Split(",")[1];
                    if (IsBase64String(base64))
                        newFile.Write(Convert.FromBase64String(base64));
                    else
                        return null;
                }
            }
            return path;
        }

        private async Task<string> GenerateCode()
        {
            string code;
            do
            {
                code = new Random().Next(10000, 99999).ToString();
            } while (await _db.Products.AnyAsync(x => x.Code == code));
            return code;
        }

        //private static async Task<bool> IsBase64String(string base64)
        //{
        //    if (base64.Contains(",")) base64 = base64.Split(",")[1];
        //    return await Task.FromResult(CheckIfIsBase64String());

        //    bool CheckIfIsBase64String()
        //    {
        //        Span<byte> buffer = stackalloc byte[base64.Length];
        //        return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        //    }
        //}
        private static bool IsBase64String(string base64)
        {
            try
            {
                if (base64.Contains(","))
                    base64 = base64.Split(",")[1];

                Convert.FromBase64String(base64);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task<string> BuildCategoryFullName(int categoryId)
        {
            var names = new List<string>();

            var category = await _db.ProductCategories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            while (category != null)
            {
                names.Add(category.Name);
                if (category.ParentCategoryId == null)
                    break;

                category = await _db.ProductCategories
                    .Include(c => c.ParentCategory)
                    .FirstOrDefaultAsync(c => c.Id == category.ParentCategoryId);
            }

            names.Reverse();
            return string.Join("/", names);
        }

    }
}