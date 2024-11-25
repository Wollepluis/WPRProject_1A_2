using System.ComponentModel.DataAnnotations;
using WPRProject_1A_2.Modellen.Abonnementen;

namespace WPRProject_1A_2.Modellen.Accounts;

public class AccountParticulier : Account 
{
    public string Naam { get; set; }
    public Adres Adres { get; set; }
    [DataType(DataType.PhoneNumber)]
    public int Telefoonnummer { get; set; }

    public AccountParticulier(string email, string wachtwoord, string naam, Adres adres, int telefoonnummer) : base(email, wachtwoord)
    {
        Naam = naam;
        Adres = adres;
        Telefoonnummer = telefoonnummer;
    }

    public void VraagHuurAan()
    {
        
    }
}