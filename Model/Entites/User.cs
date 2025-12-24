using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public string NationalCode { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string IdentifierCode { get; set; }
        public int? IdentifierUserId { get; set; }

        [ForeignKey("IdentifierUserId")]
        public User IdentifierUser { get; set; }
        [Required]
        public UserStatus Status { get; set; } = UserStatus.Active;

        public string VerificationCode { get; set; }
        public DateTime? VerificationCodeExpiry { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        // Navigation Properties
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        // Navigation Properties
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Suspended = 3,
        PendingVerification = 4
    }
}
namespace AvvalOnline.Shop.Api.Model.Entites
{

}