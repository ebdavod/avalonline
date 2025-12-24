namespace AvvalOnline.Shop.Api.Messaging.Discount
{
    public class DiscountDTO
    {
        public int Id { get; set; }
        public string Code { get; set; } // کد تخفیف
        public string Description { get; set; }
        public decimal Percentage { get; set; } // درصد تخفیف
        public decimal? MaxAmount { get; set; } // سقف تخفیف
        public int UsageLimit { get; set; } // تعداد دفعات مجاز
        public int UsedCount { get; set; } // تعداد استفاده شده
        public bool IsActive { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
