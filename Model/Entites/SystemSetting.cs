using System.ComponentModel.DataAnnotations;


namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class SystemSetting
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; }

        public string Value { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string DataType { get; set; } // string, int, bool, etc.
        public string Category { get; set; } // General, Payment, Shipping, etc.

        public bool IsEncrypted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public int UpdatedBy { get; set; }
    }
}