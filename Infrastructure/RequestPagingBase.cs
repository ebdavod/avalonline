namespace AvvalOnline.Shop.Api.Infrastructure
{
    public class RequestPagingBase:RequestBase
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
