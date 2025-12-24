using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string AltText { get; set; }
        public int DisplayOrder { get; set; } // ترتیب نمایش

        public bool IsMain { get; set; } // تصویر اصلی
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}