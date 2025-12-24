using System.ComponentModel.DataAnnotations;


namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // Product.Create, Order.View, etc.

        [StringLength(255)]
        public string Description { get; set; }

        public string Category { get; set; } // Product, Order, User, etc.
        public bool IsActive { get; set; } = true;

        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
