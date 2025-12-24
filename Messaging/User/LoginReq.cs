namespace AvvalOnline.Shop.Api.Messaging.User
{
    public class LoginReq
    {
        public UserLoginReqVm Entity { get; set; }
    }

    public class UserLoginReqVm
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

