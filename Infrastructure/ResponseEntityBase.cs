namespace AvvalOnline.Shop.Api.Infrastructure
{
    public class ResponseEntityBase<TEntity> : ResponseBase
    {
        public TEntity Entity { get; set; }
    }
}
