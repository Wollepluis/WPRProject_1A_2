using System.Net;
using System.Net.Mail;

namespace WPRRewrite.SysteemFuncties;

public class EmailSender
{
    private const string MailAddress = "carandall.business@gmail.com";
    private const string MailPassword = "niuu gghq qfop vyiz";
    
    private static readonly string HtmlBase = $@"
    <html>
        <head>
            <style>
                h1 {{ color: #4CAF50; font-family: Arial, sans-serif; }}
                p {{ font-family: Arial, sans-serif; color: #555555; }}
                .button {{ background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; border-radius: 5px; }}
            </style>
        </head>
        <body>
            { _htmlBody }
        </body>
    </html>
    ";
    
    private static string _htmlBody = "";

    private static readonly SmtpClient SmtpClient = new()
    {
        Host = "smtp.gmail.com",
        Port = 587,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(MailAddress, MailPassword),
        EnableSsl = true
    };

    private static void VerstuurEmail(MailMessage mailBericht)
    {
        try
        {
            SmtpClient.Send(mailBericht);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    private static MailMessage MaakMailBericht(string ontvangerEmail, string onderwerp, string? ccEmail = null)
    {
        var mailBericht = new MailMessage
        {
            From = new MailAddress(MailAddress),
            Subject = onderwerp,
            Body = HtmlBase,
            IsBodyHtml = true
        };

        mailBericht.To.Add(ontvangerEmail);

        if (!string.IsNullOrEmpty(ccEmail))
        {
            mailBericht.CC.Add(ccEmail);
        }
        return mailBericht;
    }
    
    public static void VerstuurBevestigingEmail(string ontvangerEmail)
    {
        const string onderwerp = "Account Aangemaakt";
        _htmlBody = $@"
            <h1>Welkom!</h1>
            <p>Uw account ({ontvangerEmail}) is succesvol aangemaakt!</p>
            <p>Bedankt voor uw registratie.</p>
            <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            ";

        var mailBericht = MaakMailBericht(ontvangerEmail, onderwerp);
        VerstuurEmail(mailBericht);
    }
    
    public static void VerstuurWijzigReserveringEmail(string ontvangerEmail)
    {
        const string onderwerp = "Uw reservering is gewijzigd!";
        _htmlBody= @"
            <h1>Uw datum van reservering is gewijzigd!</h1>
            <p>Toch niet de gewenste datum?</p>
            <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            ";

        var mailBericht = MaakMailBericht(ontvangerEmail, onderwerp);
        VerstuurEmail(mailBericht);
    }
    
    public static void VerstuurVerwijderReserveringEmail(string ontvangerEmail)
    {
        const string onderwerp = "Uw reservering is verwijderd!";
        _htmlBody= @"
            <h1>Uw reservering is geannuleerd!</h1>
            <p>Nieuwe datum reservering inplannen?</p>
            <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>  
            ";

        var mailBericht = MaakMailBericht(ontvangerEmail, onderwerp);
        VerstuurEmail(mailBericht);
    }

    public static void VerstuurHerinneringEmail(string ontvangerEmail, int id, DateOnly date)
    {
        const string onderwerp = "Uw reservering staat klaar!";
        _htmlBody= $@"
            <h1>Uw Reservering staat klaar!</h1>
            <p>Uw auto met reservering-ID: {id} staat klaar om morgen ({date}) opgehaald te worden.</p>
            <p>Neem gerust contact met ons op via <a href='mailto:{MailAddress}'>{MailAddress}</a> als u vragen heeft.</p>
            <p><a href='http://www.example.com/feedback' class='button'>Deel uw feedback</a></p>
            <div class='footer'>
                <p>Met vriendelijke groet,<br>Het Team</p>
                <p>&copy; 2024 Bedrijf. Alle rechten voorbehouden.</p>
            </div>
            ";

        var mailBericht = MaakMailBericht(ontvangerEmail, onderwerp);
        VerstuurEmail(mailBericht);
    }
    
    public static void VerstuurVerwijderEmail(string ontvangerEmail)
    {
        const string onderwerp = "Uw account is verwijderd!";
        _htmlBody= @"
            <h1>Jammer dat u vertrekt!</h1>
            <p>We hebben ervan genoten om u als gebruiker te hebben. Als we iets beter hadden kunnen doen, horen we dat graag.</p>
            <p>Mocht u van gedachten veranderen, bent u altijd welkom om terug te keren.</p>
            <p>Neem gerust contact met ons op via <a href='mailto:{EmailAdres}'>{EmailAdres}</a> als u vragen heeft.</p>
            <p><a href='http://www.example.com/feedback' class='button'>Deel uw feedback</a></p>
            <div class='footer'>
                <p>Met vriendelijke groet,<br>Het Team</p>
                <p>&copy; 2024 Bedrijf. Alle rechten voorbehouden.</p>
            </div>
            ";

        var mailBericht = MaakMailBericht(ontvangerEmail, onderwerp);
        VerstuurEmail(mailBericht);
    }
    
    public static void AanvraagGoedgekeurd(string ontvangerEmail, DateOnly begindatum, DateOnly einddatum, string merk, string model, string voertuigtype, string comment)
        {
            string htmlInhoud = $@"
            <html>
            <head>
                <style>
                    h1 {{ color: #4CAF50; font-family: Arial, sans-serif; }}
                    p {{ font-family: Arial, sans-serif; color: #555555; }}
                    .button {{ background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; border-radius: 5px; }}
                </style>
            </head>
            <body>
                <h1>CarAndAll</h1>
                <p>Uw aanvraag voor de {voertuigtype}: {merk} {model} van {begindatum} tot {einddatum} is goedgekeurd.</p>
                <p>De reden daarvoor is: {comment}</p>
                <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            </body>
            </html>";

            var mailBericht = MaakMailBericht(ontvangerEmail, "Aanvraaggoedgekeurd", htmlInhoud);
            VerstuurEmail(mailBericht);
        }
        public static void AanvraagAfgekeurd(string ontvangerEmail, DateOnly begindatum, DateOnly einddatum, string merk, string model, string voertuigtype, string comment)
        {
            string htmlInhoud = $@"
            <html>
            <head>
                <style>
                    h1 {{ color: #4CAF50; font-family: Arial, sans-serif; }}
                    p {{ font-family: Arial, sans-serif; color: #555555; }}
                    .button {{ background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; border-radius: 5px; }}
                </style>
            </head>
            <body>
                <h1>CarAndAll</h1>
                <p>Uw aanvraag voor de {voertuigtype}: {merk} {model} van {begindatum} tot {einddatum} is afgekeurd.</p>
                <p>De reden daarvoor is: {comment}</p>
                <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            </body>
            </html>";

            var mailBericht = MaakMailBericht(ontvangerEmail, "Aanvraagafgekeurd", htmlInhoud);
            VerstuurEmail(mailBericht);
        }
}