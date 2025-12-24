namespace AvvalOnline.Shop.Api.Infrastructure
{
    public abstract class ResponseBase
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
