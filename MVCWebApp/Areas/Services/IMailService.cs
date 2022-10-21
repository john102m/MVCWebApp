using MVCWebApp.Models;
using System.Threading.Tasks;

namespace MVCWebApp.Areas.Services
{
    public interface IMailService
    {
        Task<string> SendEmailAsync(MailRequest mailRequest);
    }
}
