namespace AvvalOnline.Shop.Api.Infrastructure
{
    public class RequestByIdBase<TId> : RequestBase
    {
        public TId Id { get; set; }
    }
}
