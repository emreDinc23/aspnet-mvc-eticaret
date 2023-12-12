using NETCore.MailKit.Core;

namespace Uk.Eticaret.Web.Mvc.Services.PasswordReset
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IEmailService _emailService;

        public PasswordResetService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<string> GenerateResetTokenAsync(string userEmail)
        {
            // Şifre sıfırlama token'ını burada oluşturun (örneğin, bir GUID kullanabilirsiniz)
            string resetToken = Guid.NewGuid().ToString();

            // TODO: Reset token'ını veritabanında saklamak için ilgili işlemleri gerçekleştirin

            return resetToken;
        }

        public async Task<bool> SendResetEmailAsync(string userEmail, string resetToken)
        {
            // E-posta gönderme işlemini burada gerçekleştirin
            string subject = "Şifre Sıfırlama";
            string body = $"Şifrenizi sıfırlamak için aşağıdaki bağlantıyı kullanın: /auth/resetpassword?email={userEmail}&resetToken={resetToken}";

            // TODO: E-postayı göndermek için EmailService kullanın
            await _emailService.SendAsync(userEmail, subject, body);

            return true;
        }
    }
}