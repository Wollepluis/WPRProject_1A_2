namespace WPRProject_1A_2.Modellen.Accounts;

public class Account_Particulier : Account 
{
    public string Naam { get; set; }
    /*public Adres Adres { get; set; }*/
    public int Telefoonnummer { get; set; }

    public Account_Particulier(string email, string wachtwoord, string naam, /*Adres adres,*/ int telefoonnummer) : base(email, wachtwoord)
    {
        Naam = naam;
        /*Adres = adres;*/
        Telefoonnummer = telefoonnummer;
    }

    public void VraagHuurAan()
    {
        
    }
}