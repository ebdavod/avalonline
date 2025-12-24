using AvvalOnline.Shop.Api.Infrastructure;
using System;
using System.Collections.Generic;

namespace AvvalOnline.Shop.Api.Model.Entites
{
    public class Discount : ModelBase<int>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Percentage { get; set; }
        public decimal? MaxAmount { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpireDate { get; set; }

        // 🔑 Navigation Property
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
