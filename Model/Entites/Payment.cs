using AvvalOnline.Shop.Api.Infrastructure;
using AvvalOnline.Shop.Api.Model.Entites;

public class Payment : ModelBase<int>
{
    public int OrderId { get; set; }              // شناسه سفارش
    public Order Order { get; set; }              // ارتباط با سفارش
    public decimal Amount { get; set; }           // مبلغ پرداخت
    public string TransactionId { get; set; }     // شناسه تراکنش (Authority)
    public string ReferenceNumber { get; set; }   // شماره پیگیری/RefId
    public string GatewayName { get; set; }       // نام درگاه (مثلاً ZarinPal)
    public PaymentMethod Method { get; set; }     // روش پرداخت
    public PaymentStatus Status { get; set; }     // وضعیت پرداخت
    public DateTime PaymentDate { get; set; }     // تاریخ پرداخت
    public DateTime CreatedAt { get; set; }       // تاریخ ایجاد رکورد
    public DateTime UpdatedAt { get; set; }       // تاریخ آخرین بروزرسانی
}
