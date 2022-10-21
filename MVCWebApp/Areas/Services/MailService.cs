using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MVCWebApp.Areas.Settings;
using MVCWebApp.Models;
using System.IO;

using System.Threading.Tasks;


namespace MVCWebApp.Areas.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        private ICyberSecurity _cyberSecurity;

        public MailService(IOptions<MailSettings> mailSettings, ICyberSecurity cyberSecurity)
        {
            
            _mailSettings = mailSettings.Value;
            _cyberSecurity = cyberSecurity;

        }

        public async Task<string> SendEmailAsync(MailRequest mailRequest)
        {

            //InternetAddressList list = new InternetAddressList();
            //list.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            ////list.Add(MailboxAddress.Parse("jmckinney0987@gmail.com"));
            ////list.Add(MailboxAddress.Parse("barbarasinclaire123@btinternet.com"));
            ////email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            //email.To.AddRange(list);


            var email = new MimeMessage();
            //email.Sender = MailboxAddress.Parse(_mailSettings.Mail);   WRONG IN EVERY RESPECT!!!!
            email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            //email.Body = new TextPart(TextFormat.Plain) { Text = "Example Plain Text Message Body" };

            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            var message = "";
            using var smtp = new SmtpClient();
            smtp.MessageSent += async (sender, args) =>
            {
                var myTask = await Task.Run(() => message = args.Response);

            };

            // send email
            //using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, false);
            smtp.Authenticate(_mailSettings.Mail, _cyberSecurity.Decrypt(_mailSettings.Password));
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
            return message;

        }
    }
}
