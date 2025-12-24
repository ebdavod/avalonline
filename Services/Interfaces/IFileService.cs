public interface IFileService
{
    Task<string> SaveProductImageAsync(string productCode, string base64Image);
    Task<bool> DeleteProductImageAsync(string imageUrl);
    Task<List<string>> SaveMultipleImagesAsync(string productCode, List<string> base64Images);
    Task<byte[]> GenerateOrderPdfAsync(int orderId);
}