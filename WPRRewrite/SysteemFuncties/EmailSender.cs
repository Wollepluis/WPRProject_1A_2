using System.Net;
using System.Net.Mail;
using WPRRewrite.Modellen.Abonnementen;

namespace WPRRewrite.SysteemFuncties;

public class EmailSender
{
    private static string MailAddress = "carandall.business@gmail.com";
    private static string MailPassword = "niuu gghq qfop vyiz";
    
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
                Console.WriteLine("Email succesvol verzonden!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij het verzenden van email: " + ex.Message);
                throw;
            }
        }

        private static MailMessage MaakMailBericht(string ontvangerEmail, string onderwerp, string htmlInhoud, string? ccEmail = null)
        {
            var mailBericht = new MailMessage
            {
                From = new MailAddress(MailAddress),
                Subject = onderwerp,
                Body = htmlInhoud,
                IsBodyHtml = true
            };

            mailBericht.To.Add(ontvangerEmail);

            if (!string.IsNullOrEmpty(ccEmail))
            {
                mailBericht.CC.Add(ccEmail);
            }

            return mailBericht;
        }

        public static void VerstuurBevestigingsEmail(string ontvangerEmail, string? bedrijfsNaam = null)
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
                <h1>Welkom!</h1>
                <p>Uw account bij <strong>{bedrijfsNaam}</strong> ({ontvangerEmail}) is succesvol aangemaakt!</p>
                <p>Bedankt voor uw registratie.</p>
                <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            </body>
            </html>";

            var mailBericht = MaakMailBericht(ontvangerEmail, "Account Aangemaakt", htmlInhoud);
            VerstuurEmail(mailBericht);
        }

        public static void VerstuurWijzigReserveringEmail(string ontvangerEmail, string? bedrijfsNaam = null)
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
                <h1>Uw datum van reservering is gewijzigd!</h1>
                <p>Toch niet de gewenste datum?</p>
                <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            </body>
            </html>";

            var mailBericht = MaakMailBericht(ontvangerEmail, "Account Aangemaakt", htmlInhoud);
            VerstuurEmail(mailBericht);
        }
        
        public static void VerstuurVerwijderReserveringEmail(string ontvangerEmail, string? bedrijfsNaam = null)
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
                <h1>Uw reservering is geannuleerd!</h1>
                <p>Nieuwe datum reservering inplannen?</p>
                <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            </body>
            </html>";

            var mailBericht = MaakMailBericht(ontvangerEmail, "Account Aangemaakt", htmlInhoud);
            VerstuurEmail(mailBericht);
        }
        
        public static void VerstuurHerinneringsEmail(string ontvangerEmail, int id, DateTime date)
        {
            string htmlInhoud = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333333; line-height: 1.6; margin: 0; padding: 20px; }}
                    h1 {{ color: #d9534f; text-align: center; }}
                    p {{ margin: 10px 0; }}
                    .button {{ display: inline-block; background-color: #0275d8; color: white; padding: 10px 20px; text-align: center; text-decoration: none; border-radius: 5px; margin-top: 20px; font-size: 16px; }}
                    .footer {{ margin-top: 30px; font-size: 12px; text-align: center; color: #aaaaaa; }}
                </style>
            </head>
            <body>
                <h1>Uw Reservering staat klaar!</h1>
                <p>Uw auto met reserverings ID: {id} staat klaar om morgen ({date}) opgehaald te worden.</p>
                <p>Neem gerust contact met ons op via <a href='mailto:{{EmailAdres}}'>{{EmailAdres}}</a> als u vragen heeft.</p>
                <p><a href='http://www.example.com/feedback' class='button'>Deel uw feedback</a></p>
                <div class='footer'>
                    <p>Met vriendelijke groet,<br>Het Team</p>
                    <p>&copy; 2024 Bedrijf. Alle rechten voorbehouden.</p>
                </div>
            </body>
            </html>";
            var mailBericht = MaakMailBericht(ontvangerEmail, "Uw reservering staat klaar", htmlInhoud);
            VerstuurEmail(mailBericht);
        }
        
        public static void VerstuurVerwijderEmail(string ontvangerEmail)
        {
            string htmlInhoud = @"
            <html>
            <head>
                <style>
                    body { font-family: Arial, sans-serif; background-color: #f4f4f9; color: #333333; line-height: 1.6; margin: 0; padding: 20px; }
                    h1 { color: #d9534f; text-align: center; }
                    p { margin: 10px 0; }
                    .button { display: inline-block; background-color: #0275d8; color: white; padding: 10px 20px; text-align: center; text-decoration: none; border-radius: 5px; margin-top: 20px; font-size: 16px; }
                    .footer { margin-top: 30px; font-size: 12px; text-align: center; color: #aaaaaa; }
                </style>
            </head>
            <body>
                <h1>Jammer dat u vertrekt!</h1>
                <p>We hebben ervan genoten om u als gebruiker te hebben. Als we iets beter hadden kunnen doen, horen we dat graag.</p>
                <p>Mocht u van gedachten veranderen, bent u altijd welkom om terug te keren.</p>
                <p>Neem gerust contact met ons op via <a href='mailto:{EmailAdres}'>{EmailAdres}</a> als u vragen heeft.</p>
                <p><a href='http://www.example.com/feedback' class='button'>Deel uw feedback</a></p>
                <div class='footer'>
                    <p>Met vriendelijke groet,<br>Het Team</p>
                    <p>&copy; 2024 Bedrijf. Alle rechten voorbehouden.</p>
                </div>
            </body>
            </html>";

            var mailBericht = MaakMailBericht(ontvangerEmail, "Jammer dat u vertrekt", htmlInhoud);
            VerstuurEmail(mailBericht);
        }
        public static void AanvraagGoedgekeurd(string ontvangerEmail, DateTime begindatum, DateTime einddatum, string merk, string model, string voertuigtype, string comment)
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
        public static void AanvraagAfgekeurd(string ontvangerEmail, DateTime begindatum, DateTime einddatum, string merk, string model, string voertuigtype, string comment)
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
        
        public static void BevestigingAbonnementWijzigen(string ontvangerEmail, Abonnement oudeAbonnement, Abonnement nieuweAbonnement)
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
                <p>U heeft uw abonnement geweizigd van:</p>
                <p>{oudeAbonnement.AbonnementType} met {oudeAbonnement.MaxMedewerkers} medewerkers en {oudeAbonnement.MaxVoertuigen} voertuigen.</p>
                <p>Naar:</p>
                <p>{nieuweAbonnement.AbonnementType} met {nieuweAbonnement.MaxMedewerkers} medewerkers en {nieuweAbonnement.MaxVoertuigen} voertuigen. </p>
                <p><a href='http://www.example.com' class='button'>Klik hier om in te loggen</a></p>
            </body>
            </html>";

            var mailBericht = MaakMailBericht(ontvangerEmail, "Aanvraagafgekeurd", htmlInhoud);
            VerstuurEmail(mailBericht);
        }
        
}