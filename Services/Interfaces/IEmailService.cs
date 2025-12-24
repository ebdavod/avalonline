public interface IEmailService
{
    Task SendOrderConfirmationAsync(int orderId, string customerEmail);
    Task SendPasswordResetEmailAsync(string email, string resetToken);
    Task SendWelcomeEmailAsync(string email, string userName);
    Task SendLowStockAlertAsync(string productName, string size);
}