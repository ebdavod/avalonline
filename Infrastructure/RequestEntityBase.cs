namespace AvvalOnline.Shop.Api.Infrastructure
{
    public abstract class RequestEntityBase<TEntity> : RequestBase
    {
        public TEntity Entity { get; set; }
    }
}
