using AvvalOnline.Shop.Api.Messaging.Product;
namespace AvvalOnline.Shop.Api.Messaging.Home
{
    public class HomePageDTO
    {
        public List<ProductDTO> Newest { get; set; }
        public List<ProductDTO> BestSellers { get; set; }
        public List<ProductDTO> Popular { get; set; }
        public List<ProductDTO> Featured { get; set; }
    }
}
