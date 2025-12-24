using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class ProductReview
    {
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } // 1-5 stars

        [StringLength(500)]
        public string Comment { get; set; }

        public string Pros { get; set; } // نقاط قوت
        public string Cons { get; set; } // نقاط ضعف

        public bool IsVerifiedPurchase { get; set; } // خرید تایید شده
        public bool IsApproved { get; set; } = false; // تایید مدیریت

        public int HelpfulCount { get; set; } = 0;
        public int NotHelpfulCount { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public List<ReviewMedia> ReviewMedia { get; set; } = new List<ReviewMedia>();
    }
    public enum MediaType
    {
        Image = 1,
        Video = 2
    }
}
