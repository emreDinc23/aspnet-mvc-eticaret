namespace Uk.Eticaret.Web.Mvc.Services.PasswordReset
{
    public interface IPasswordResetService
    {
        Task<string> GenerateResetTokenAsync(string userEmail);

        Task<bool> SendResetEmailAsync(string userEmail, string resetToken);
    }
}