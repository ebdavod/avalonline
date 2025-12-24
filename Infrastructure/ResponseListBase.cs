namespace AvvalOnline.Shop.Api.Infrastructure
{
    public class ResponseListBase<TEntity> : ResponseBase
    {
        public List<TEntity> Entities { get; set; }
    }
}
