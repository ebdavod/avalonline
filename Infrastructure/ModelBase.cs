namespace AvvalOnline.Shop.Api.Infrastructure
{
    public abstract class ModelBase<TId>
    {
        public TId Id { get; set; }
    }
}
