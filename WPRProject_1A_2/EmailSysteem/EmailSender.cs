using System;
using System.Net;
using System.Net.Mail;

namespace WPRProject_1A_2.EmailSysteem;

public static class EmailSender
{
    public static void SendEmail()
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