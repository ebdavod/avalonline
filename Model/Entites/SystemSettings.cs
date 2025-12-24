namespace AvvalOnline.Shop.Api.Model.Entites
{
    // تنظیمات پیش‌فرض
    public static class SystemSettings
    {
        public const string StoreName = "Store.Name";
        public const string StoreDescription = "Store.Description";
        public const string StorePhone = "Store.Phone";
        public const string StoreEmail = "Store.Email";
        public const string StoreAddress = "Store.Address";

        public const string Currency = "General.Currency";
        public const string TimeZone = "General.TimeZone";
        public const string Language = "General.Language";

        public const string MinimumOrderAmount = "Order.MinimumAmount";
        public const string TaxPercentage = "Order.TaxPercentage";

        public const string SMTPHost = "Email.SMTPHost";
        public const string SMTPPort = "Email.SMTPPort";
        public const string SMTPUsername = "Email.SMTPUsername";
        public const string SMTPPassword = "Email.SMTPPassword";

        public const string PaymentGateway = "Payment.Gateway";
        public const string PaymentTestMode = "Payment.TestMode";
    }
}