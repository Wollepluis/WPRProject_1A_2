using System.Net;
using System.Net.Mail;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.SysteemFuncties;

public class EmailSender
{
    private static string _mailAdress = "carandall.business@gmail.com";
    
    private static SmtpClient _smtpClient;
    public EmailSender(SmtpClient smtpClient)
    {
        _smtpClient = smtpClient ?? throw new ArgumentException();
    }

    private static string _bedrijfString = "";
    private static string _emailString = "";
    private static string _htmlBody = @"
        <html>
        <head>
            <style>
                h1 {{
                    color: #4CAF50;
                    font-family: Arial, sans-serif;
                }}
                p {{
                    font-family: Arial, sans-serif;
                    color: #555555;
                }}
                .button {{
                    background-color: #4CAF50;
                    color: white;
                    padding: 10px 20px;
                    text-align: center;
                    text-decoration: none;
                    border-radius: 5px;
                }}
            </style>
        </head>
        <body>
            <h1>Welkom!</h1>
            <p>Uw account " + _bedrijfString + "<strong> " + _emailString + @" </strong> is succesvol aangemaakt!</p>
            <p>Bedankt voor uw registratie.</p>
            <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
        </body>
        </html>
    ";
    
    
    private static void EmailConnector()
    {
        _smtpClient.Host = "smtp.gmail.com";
        _smtpClient.Port = 587;
        _smtpClient.UseDefaultCredentials = false;
        _smtpClient.Credentials = new NetworkCredential(_mailAdress, "Auto.Project18");
        _smtpClient.EnableSsl = true; 
    }

    private static void SendEmail(MailMessage mailMessage)
    {
        try
        {
            _smtpClient.Send(mailMessage);
            Console.WriteLine("Email verzonden!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
            throw;
        }
    }

    private static MailMessage MessageCrafter(IAccount account)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("carandall.business@gmail.com");
        mailMessage.To.Add(account.Email);
        mailMessage.Subject = "Account aangemaakt";

        if (account is AccountZakelijkHuurder)
        {
            var zakelijkAccount = (AccountZakelijkHuurder)account;
            _bedrijfString = zakelijkAccount.Bedrijf.Bedrijfsnaam;
        }

        _emailString = account.Email;
        mailMessage.Body = _htmlBody;
        mailMessage.IsBodyHtml = true; 
        
        return mailMessage;
    }
    
    public static void BevestigingsEmail(IAccount account)
    {
        EmailConnector();
        MailMessage mailMessage = MessageCrafter(account);
        
        SendEmail(mailMessage);
    }
    
    public static void BevestigingsEmail(AccountZakelijkBeheerder beheerderAccount, AccountZakelijkHuurder toegevoegdAccount)
    {
        EmailConnector();
        MailMessage mailMessage = MessageCrafter(toegevoegdAccount);
        mailMessage.CC.Add(beheerderAccount.Email);

        SendEmail(mailMessage);
    }
    
    public static void SendVerwijderEmail(string email)
    {
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress("carandall.business@gmail.com");
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
    <p>Neem gerust contact met ons op via <a href='mailto:support@bedrijf.com'>carandall.business@gmail.com</a> als je vragen hebt.</p>
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

        SendEmail(mailMessage);
    }
}