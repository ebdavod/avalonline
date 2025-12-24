
namespace AvvalOnline.Shop.Api.Messaging.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public string IdentifierCode { get; set; }
        public int? IdentifierUserId { get; set; }
    }
    public class UserCreateDTO
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public string IdentifierCode { get; set; }
        public int? IdentifierUserId { get; set; }
    }
}
