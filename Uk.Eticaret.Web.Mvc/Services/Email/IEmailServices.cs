namespace Uk.Eticaret.Web.Mvc.Services.Email
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}