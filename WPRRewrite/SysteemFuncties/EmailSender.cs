using System.Net;
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
    
    public static void SendEmail(AccountZakelijkBeheerder beheerderAccount, AccountZakelijkHuurder ToegevoegdAccount)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mark2492@gmail.com");
        mailMessage.To.Add(beheerderAccount.Email);
        mailMessage.CC.Add(ToegevoegdAccount.Email);
        mailMessage.Subject = "Medewerker toegevoegd";
        mailMessage.Body = "Huurder: " + ToegevoegdAccount.Email + " is aangemaakt en toegevoegd aan het bedrijf";

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
    // Maak een nieuwe e-mail aan
    MailMessage mailMessage = new MailMessage();
    mailMessage.From = new MailAddress("mark2492@gmail.com");
    mailMessage.To.Add(account.Email);
    mailMessage.Subject = "Account aangemaakt";

    // HTML voor de e-mail body
    string htmlBody = @"
    <html>
    <head>
        <style>
            body {
                font-family: Arial, sans-serif;
                background-color: #f9f9f9;
                color: #333333;
                line-height: 1.6;
                margin: 0;
                padding: 20px;
            }
            h1 {
                color: #4CAF50;
                text-align: center;
            }
            p {
                margin: 10px 0;
            }
            .button {
                display: inline-block;
                background-color: #4CAF50;
                color: white;
                padding: 10px 20px;
                text-align: center;
                text-decoration: none;
                border-radius: 5px;
                margin-top: 20px;
                font-size: 16px;
            }
            .footer {
                margin-top: 30px;
                font-size: 12px;
                text-align: center;
                color: #aaaaaa;
            }
        </style>
    </head>
    <body>
        <h1>Welkom bij CarAndAll!</h1>
        <p>Beste gebruiker,</p>
        <p>Gefeliciteerd! Uw account is succesvol aangemaakt.</p>
        <p>Uw geregistreerde e-mailadres: <strong>" + account.Email + @"</strong></p>
        <p>We raden aan om uw accountgegevens goed te bewaren en bij vragen contact met ons op te nemen.</p>
        <p><a href='http://www.example.com/inloggen' class='button'>Log nu in</a></p>
        <div class='footer'>
            <p>Met vriendelijke groet,<br>Het Bedrijfsteam (niet Abhishrek)</p>
            <p>&copy; 2024 Bedrijf. Alle rechten voorbehouden.</p>
        </div>
    </body>
    </html>
    ";

    // Stel de HTML-body in voor de e-mail
    mailMessage.Body = htmlBody;
    mailMessage.IsBodyHtml = true;

    // Instellen van de SMTP-client
    SmtpClient smtpClient = new SmtpClient();
    smtpClient.Host = "smtp.gmail.com";
    smtpClient.Port = 587;
    smtpClient.UseDefaultCredentials = false;
    smtpClient.Credentials = new NetworkCredential("mark2492@gmail.com", "khfp clab fvmm wgmc");
    smtpClient.EnableSsl = true;

    try
    {
        // Verzend de e-mail
        smtpClient.Send(mailMessage);
        Console.WriteLine("Email verzonden!");
    }
    catch (Exception e)
    {
        Console.WriteLine("Error: " + e.Message);
        throw;
    }
}

    
    public static void SendEmail(AccountZakelijkHuurder account)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mark2492@gmail.com");
        mailMessage.To.Add(account.Email);
        mailMessage.Subject = "Account aangemaakt";

        // HTML voor de e-mail body
        string htmlBody = @"
        <html>
        <head>
            <style>
                h1 {
                    color: #4CAF50;
                    font-family: Arial, sans-serif;
                }
                p {
                    font-family: Arial, sans-serif;
                    color: #555555;
                }
                .button {
                    background-color: #4CAF50;
                    color: white;
                    padding: 10px 20px;
                    text-align: center;
                    text-decoration: none;
                    border-radius: 5px;
                }
            </style>
        </head>
        <body>
            <h1>Welkom!</h1>
            <p>Uw account <strong>" + account.Email + @"</strong> is succesvol aangemaakt!</p>
            <p>Bedankt voor uw registratie.</p>
            <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
        </body>
        </html>
    ";

        mailMessage.Body = htmlBody;
        mailMessage.IsBodyHtml = true;  // Zet de body als HTML

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
    
    public static void SendVerwijderEmail(string email)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("mark2492@gmail.com");
        mailMessage.To.Add(email);
        mailMessage.Subject = "Jammer dat je gaat";

        // HTML voor de e-mail body
        string htmlBody = @"
<html>
<head>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f9;
            color: #333333;
            line-height: 1.6;
            margin: 0;
            padding: 20px;
        }
        h1 {
            color: #d9534f;
            text-align: center;
        }
        p {
            margin: 10px 0;
        }
        .button {
            display: inline-block;
            background-color: #0275d8;
            color: white;
            padding: 10px 20px;
            text-align: center;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
            font-size: 16px;
        }
        .footer {
            margin-top: 30px;
            font-size: 12px;
            text-align: center;
            color: #aaaaaa;
        }
    </style>
</head>
<body>
    <h1>Jammer dat je gaat!</h1>
    <p>Beste gebruiker,</p>
    <p>We hebben met veel plezier je account beheerd. Het spijt ons dat je ervoor hebt gekozen om afscheid te nemen. Als we iets beter hadden kunnen doen, horen we dat graag!</p>
    <p>Als je van gedachten verandert, ben je altijd welkom om terug te keren.</p>
    <p>Neem gerust contact met ons op via <a href='mailto:support@bedrijf.com'>support@bedrijf.com</a> als je vragen hebt.</p>
    <p><a href='http://www.example.com/feedback' class='button'>Deel je feedback</a></p>
    <div class='footer'>
        <p>Met vriendelijke groet,<br>Het Bedrijfsteam (Niet abhishrek)</p>
        <p>&copy; 2024 Bedrijf. Alle rechten voorbehouden.</p>
    </div>
</body>
</html>
";


        mailMessage.Body = htmlBody;
        mailMessage.IsBodyHtml = true;  // Zet de body als HTML

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

    public static void TestMail()
    {
    // Maak een nieuwe e-mail aan
    MailMessage mailMessage = new MailMessage();
    mailMessage.From = new MailAddress("mark2492@gmail.com");
    mailMessage.To.Add("23124024@student.hhs.nl");
    mailMessage.Subject = "Mail";
    mailMessage.Priority = MailPriority.High;

    // HTML voor de e-mail body
    string htmlBody = @"
    <html>
    <head>
        <style>
            body {
                font-family: Arial, sans-serif;
                background-color: #f9f9f9;
                color: #333333;
                line-height: 1.6;
                margin: 0;
                padding: 20px;
            }
            h1 {
                color: #4CAF50;
                text-align: center;
            }
            p {
                margin: 10px 0;
            }
            .button {
                display: inline-block;
                background-color: #4CAF50;
                color: white;
                padding: 10px 20px;
                text-align: center;
                text-decoration: none;
                border-radius: 5px;
                margin-top: 20px;
                font-size: 16px;
            }
            .footer {
                margin-top: 30px;
                font-size: 12px;
                text-align: center;
                color: #aaaaaa;
            }
        </style>
    </head>
    <body>
        <h1>Ik weet niks van een mail</h1>
        
    </body>
    </html>
    ";

    // Stel de HTML-body in voor de e-mail
    mailMessage.Body = htmlBody;
    mailMessage.IsBodyHtml = true;

    // Instellen van de SMTP-client
    SmtpClient smtpClient = new SmtpClient();
    smtpClient.Host = "smtp.gmail.com";
    smtpClient.Port = 587;
    smtpClient.UseDefaultCredentials = false;
    smtpClient.Credentials = new NetworkCredential("mark2492@gmail.com", "khfp clab fvmm wgmc");
    smtpClient.EnableSsl = true;

    try
    {
        // Verzend de e-mail
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