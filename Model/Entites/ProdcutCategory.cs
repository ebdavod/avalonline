using System.ComponentModel.DataAnnotations;
namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class ProdcutCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // مثال: "لاستیک", "کفش", "لوازم جانبی"

        public string Description { get; set; }

        // برای دسته‌بندی سلسله مراتبی
        public int? ParentCategoryId { get; set; }
        public ProdcutCategory ParentCategory { get; set; }

        public List<ProdcutCategory> SubCategories { get; set; } = new List<ProdcutCategory>();
        public List<Product> Products { get; set; } = new List<Product>();

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }
}