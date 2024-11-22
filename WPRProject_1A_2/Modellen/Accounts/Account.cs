using System.ComponentModel.DataAnnotations;

namespace WPRProject_1A_2.Modellen.Accounts;

public class Account
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Wachtwoord { get; set; }

    public Account(string email, string wachtwoord)
    {
        Email = email;
        Wachtwoord = wachtwoord;
    }

    public void Login(string email, string wachtwoord)
    {
        
    }

    public void Loguit()
    {
        
    }

    public void Registreer()
    {
        
    }

    public void VraagStatusOp()
    {
        
    }
}