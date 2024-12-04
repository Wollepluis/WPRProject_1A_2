namespace WPRRewrite.Modellen.Accounts;

public class AccountParticulier : Account
{
    public int ParticulierAccountId { get; set; }
    public string Naam { get; set; }
    public int ParticulierAdres { get; set; }
    public int Telefoonnummer { get; set; }
    public int Account { get; set; }

    public void UpdateAccountParticulier(AccountParticulier updatedAccountParticulier)
    {
        Email = updatedAccountParticulier.Email;
        Wachtwoord = updatedAccountParticulier.Wachtwoord;
        Naam = updatedAccountParticulier.Naam;
        ParticulierAdres = updatedAccountParticulier.ParticulierAdres;
        Telefoonnummer = updatedAccountParticulier.Telefoonnummer;
    }
}