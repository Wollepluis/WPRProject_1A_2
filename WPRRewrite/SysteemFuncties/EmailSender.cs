﻿using System.Net;
using System.Net.Mail;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.SysteemFuncties;

public class EmailSender
{
    public static void SendEmail(Bedrijf bedrijf, AccountZakelijkBeheerder account)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mark2492@gmail.com");
        mailMessage.To.Add(bedrijf.BevoegdeMedewerkers.OfType<AccountZakelijkBeheerder>().First().Email);
        mailMessage.Subject = "Bedrijf Aangemaakt";
        mailMessage.Body = "Het bedrijf met de naam: " + bedrijf.Bedrijfsnaam + "met het zakelijk account met het email " + account.Email + " is aangemaakt!";

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
    
    public static void SendEmail(AccountZakelijkBeheerder account, AccountZakelijkHuurder accountZakelijkHuurder)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mark2492@gmail.com");
        mailMessage.To.Add(account.Email);
        mailMessage.CC.Add(accountZakelijkHuurder.Email);
        mailMessage.Subject = "Medewerker toegevoegd";
        mailMessage.Body = "Huurder: " + accountZakelijkHuurder.Email + " is aangemaakt en toegevoegd aan het bedrijf";

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
    
    public static void SendEmail(IAccount account)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mark2492@gmail.com");
        mailMessage.To.Add(account.Email);
        mailMessage.Subject = "Account aangemaakt";
        mailMessage.Body = "Uw account: " + account.Email + "is aangemaakt!";

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