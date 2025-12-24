using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class Vehicle
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string PlateNumber { get; set; } // پلاک

        public string VIN { get; set; } // شماره شاسی
        public string Brand { get; set; } // برند (تویوتا، هیوندای، etc)
        public string Model { get; set; } // مدل
        public int? Year { get; set; } // سال ساخت
        public string Color { get; set; }
        public string EngineNumber { get; set; }

        // مشخصات فنی برای لاستیک
        public string TireSize { get; set; } // سایز لاستیک
        public string RimSize { get; set; } // سایز رینگ

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}