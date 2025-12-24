    namespace AvvalOnline.Shop.Api.Infrastructure
    {
        public class JWTConfigModel
        {
            public string SecretKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
        }
    }
