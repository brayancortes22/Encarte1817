using MimeKit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken = default);
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task<bool> SendEmailToManyAsync(List<string> to, string subject, string body, bool isHtml = true);
        Task<bool> SendEmailWithCopyAsync(string to, string subject, string body,
            List<string> cc = null, List<string> bcc = null, bool isHtml = true);
    }
}