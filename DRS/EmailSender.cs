//using Microsoft.Extensions.Configuration;
//using System.IO;
//using System.Net;
//using System.Net.Mail;
//using System.Threading.Tasks;
//namespace DRS
//{


//    public class EmailSender :IEmailSender
//    {
//        private readonly IConfiguration _configuration;

//        public EmailSender(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
        
//        //public async Task SendEmailAsync(string email, string subject, string message, Stream attachmentStream, string attachmentName, string attachmentContentType)
//        //{
//        //    string senderEmail = _configuration["EmailSettings:Email"];
//        //    string displayName = _configuration["EmailSettings:DisplayName"];
//        //    string password = _configuration["EmailSettings:Password"];
//        //    string smtpServer = _configuration["EmailSettings:SmtpServer"];
//        //    int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

//        //    using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
//        //    {
//        //        smtpClient.UseDefaultCredentials = false;
//        //        smtpClient.Credentials = new NetworkCredential(senderEmail, password);
//        //        smtpClient.EnableSsl = true;

//        //        using (var mailMessage = new MailMessage())
//        //        {
//        //            mailMessage.From = new MailAddress(senderEmail, displayName);
//        //            mailMessage.To.Add(email);
//        //            mailMessage.Subject = subject;
//        //            mailMessage.Body = message;

//        //            // Attach the Excel file to the email
//        //            mailMessage.Attachments.Add(new Attachment(attachmentStream, attachmentName, attachmentContentType));

//        //            await smtpClient.SendMailAsync(mailMessage);
//        //        }
//        //    }
//        //}
//        public async Task SendEmailAsync(string email, string subject, string message)
//        {
//            string senderEmail = _configuration["EmailSettings:Email"];
//            string displayName = _configuration["EmailSettings:DisplayName"];
//            string password = _configuration["EmailSettings:Password"];
//            string smtpServer = _configuration["EmailSettings:SmtpServer"];
//            int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);

//            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
//            {
//                smtpClient.UseDefaultCredentials = false;
//                smtpClient.Credentials = new NetworkCredential(senderEmail, password);
//                smtpClient.EnableSsl = true;

//                using (var mailMessage = new MailMessage())
//                {
//                    mailMessage.From = new MailAddress(senderEmail, displayName);
//                    mailMessage.To.Add(email);
//                    mailMessage.Subject = subject;
//                    mailMessage.Body = message;

//                    // Attach the Excel file to the email
//                    //mailMessage.Attachments.Add(new Attachment(attachmentStream, attachmentName, attachmentContentType));

//                    await smtpClient.SendMailAsync(mailMessage);
//                }
//            }
//        }
//        //Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
//        //{
//        //    throw new NotImplementedException();
//        //}
//    }

//}
