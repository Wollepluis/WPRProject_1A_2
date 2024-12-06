using System.Net;
using System.Net.Mail;
using WPRRewrite.Modellen;

namespace WPRRewrite.SysteemFuncties;

public class EmailSender
{
    public void SendEmail(Bedrijf bedrijf)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mark2492@gmail.com");
        mailMessage.To.Add("23096780@student.hhs.nl");
        mailMessage.Subject = "BevestigingsTest";
        mailMessage.Body = "This is test email";

        SmtpClient smtpClient = new SmtpClient();
        smtpClient.Host = "smtp.gmail.com";
        smtpClient.Port = 587;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential("mark2492@gmail.com", "khfp clab fvmm wgmc");
        smtpClient.EnableSsl = true;

        try
        {
            smtpClient.Send(mailMessage);
            Console.WriteLine("Email verzonden!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            throw;
        }
    }
}