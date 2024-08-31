using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace FoodFestAPI.Utils
{
    public class SendEmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    //Host = "sandbox.smtp.mailtrap.io",                    
                    Port = 587,
                    Credentials = new NetworkCredential("puntodewofadhillah@gmail.com", "ptbr ekhv xjdx pxwo"),
                    //Credentials = new NetworkCredential("654c3947cadb22", "07ccaead4a47bd"),
                    EnableSsl = true,
                    //UseDefaultCredentials = true
                };

                await client.SendMailAsync(
                    new MailMessage("recipe.admin@recipe.com", email, subject, body) { IsBodyHtml = true }
                );
            }
            catch(Exception ex)
            {
                throw;
            }           
        }
    }
}
