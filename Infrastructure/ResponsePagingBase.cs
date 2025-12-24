using AvvalOnline.Shop.Api.Model;

namespace AvvalOnline.Shop.Api.Infrastructure
{
    public class ResponsePagingBase<TEntity> : ResponseListBase<TEntity>
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;
    }
}
