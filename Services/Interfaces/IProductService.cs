using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Messaging.Product;

namespace AvvalOnline.Shop.Api.Services.Interfaces
{

    public interface IProductService
    {
        // عملیات اصلی CRUD
        Task<GetProductByIdRes> GetProductByIdAsync(GetProductByIdReq req);
        Task<GetAllProductsRes> GetAllProductsAsync(GetAllProductsReq req);
        Task<CreateProductRes> CreateProductAsync(CreateProductReq req);
        Task<UpdateProductRes> UpdateProductAsync(UpdateProductReq req);
        Task<DeleteProductRes> DeleteProductAsync(DeleteProductReq req);
        Task<GetProductByCodeRes> GetProductByCodeAsync(GetProductByCodeReq req);

        // عملیات خاص
        //Task<List<ProductDTO>> SearchProductsAsync(string searchTerm, int page = 1, int pageSize = 20);
        //Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        //Task<List<ProductDTO>> GetProductsByVehicleTypeAsync(string vehicleType);
        //Task<bool> CheckProductAvailabilityAsync(int productId, string size, int quantity);
        //Task UpdateProductStockAsync(int productId, string size, int quantityChange);

        // مدیریت تصاویر
        //Task<List<ProductImageDTO>> GetProductImagesAsync(int productId);
        //Task AddProductImageAsync(int productId, string base64Image, bool isMain = false);
        //Task RemoveProductImageAsync(int imageId);
        //Task SetMainImageAsync(int productId, int imageId);

        // گزارش‌گیری
        //Task<ProductStatisticsDTO> GetProductStatisticsAsync();
        //Task<List<ProductLowStockDTO>> GetLowStockProductsAsync(int threshold = 5);
    }
    public class ProductImageDTO
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string AltText { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsMain { get; set; }
    }

    public class ProductInventoryDTO
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public int StockQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int AvailableQuantity => StockQuantity - ReservedQuantity;
        public bool IsLowStock { get; set; }
    }

    public class ProductStatisticsDTO
    {
        public int TotalProducts { get; set; }
        public int AvailableProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int LowStockProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }

    public class ProductLowStockDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Size { get; set; }
        public int CurrentStock { get; set; }
        public int Threshold { get; set; }
    }
}