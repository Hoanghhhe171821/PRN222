using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

public class FakeEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // Không làm gì cả, chỉ để tránh lỗi
        return Task.CompletedTask;
    }
}