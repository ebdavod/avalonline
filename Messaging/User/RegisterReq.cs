
namespace AvvalOnline.Shop.Api.Messaging.User
{
    public class RegisterReq
    {
        public UserRegisterReqVm Entity { get; set; }
    }
    public class UserRegisterReqVm
    {
        //public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public string IdentifierCode { get; set; }
    }
}
