using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPRProject_1A_2.Modellen.Accounts;
using WPRProject_1A_2.Modellen.Enums;

namespace WPRProject_1A_2.Modellen.Voertuigmodellen;

public class Reservering
{
    [Key]
    public int ReserveringId { get; set; }
    public DateTime Begindatum { get; set; }
    public DateTime Einddatum { get; set; }
    public string AardVanReis { get; set; }
    public string VersteBestemming { get; set; }
    public int VerwachteHoeveelheidKm { get; set; }
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    
    public List<Voertuig> BesteldeVoertuigen { get; set; }
    
    public Account Account { get; set; }
    public long RijbewijsDocumentnummer { get; set; }
    
    [DataType(DataType.Currency)]
    public Double Totaalprijs { get; set; }
    public Huuraanvraag Huuraanvraag { get; set; }
    
    public bool IsBetaald { get; set; }
    
    public Reservering() {}

    public Reservering(List<Voertuig> besteldeVoertuigen, DateTime begindatum, DateTime einddatum, string aardVanReis, string versteBestemming, int verwachteHoeveelheidKm, Account account, int accountId, int rijbewijsDocumentnummer, double totaalprijs, bool isBetaald)
    {
        BesteldeVoertuigen = besteldeVoertuigen;
        Begindatum = begindatum;
        Einddatum = einddatum;
        AardVanReis = aardVanReis;
        VersteBestemming = versteBestemming;
        VerwachteHoeveelheidKm = verwachteHoeveelheidKm;
        AccountId = accountId;
        Account = account;
        RijbewijsDocumentnummer = rijbewijsDocumentnummer;
        
        Totaalprijs = totaalprijs;
        Huuraanvraag = Huuraanvraag.InBehandeling;
        
        IsBetaald = false;
    }

    public int BerekenPrijs()
    {
        int prijs = 0;
        return prijs;
    }

    public void MaakFactuurAan()
    {
        
    }

    public async void CheckRijbewijs(long rijbewijs)
    {
        string apiUrl = "https://api.rdw.nl/rijbewijs";  // Voorbeeld URL van een REST API

        using (HttpClient client = new HttpClient())
        {
            // Stel de parameters in voor de aanvraag
            var parameters = new
            {
                rijbewijsnummer = rijbewijs,
                gebruiker = "jouw_gebruikersnaam",
                wachtwoord = "jouw_wachtwoord"
            };

            // Stuur de GET-aanroep
            HttpResponseMessage response = await client.GetAsync($"{apiUrl}?rijbewijsnummer={rijbewijs}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Rijbewijs is geldig:");
                Console.WriteLine(responseBody); // Verwerk de JSON response
            }
            else
            {
                Console.WriteLine($"Fout bij het controleren van het rijbewijs: {response.StatusCode}");
            }
        }
    }
}