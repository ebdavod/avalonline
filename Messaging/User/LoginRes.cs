
namespace AvvalOnline.Shop.Api.Messaging.User
{
    public class LoginRes
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public UserLoginResultVm Entity { get; set; }
    }
    public class UserLoginResultVm
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }
}