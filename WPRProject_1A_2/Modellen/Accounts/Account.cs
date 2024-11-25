using System.ComponentModel.DataAnnotations;
using WPRProject_1A_2.Modellen.Betalingen;
using WPRProject_1A_2.Modellen.Voertuigmodellen;

namespace WPRProject_1A_2.Modellen.Accounts;

public class Account
{
    [Key]
    public int Id { get; set; }
    [MaxLength(30)]
    public string Email { get; set; }
    [StringLength(30, MinimumLength = 8)]
    public string Wachtwoord { get; set; }
    
    public List<Reservering> ActieveReserveringen { get; set; }
    public List<Factuur> Betaalgeschiedenis { get; set; }

    public Account(string email, string wachtwoord)
    {
        Email = email;
        Wachtwoord = wachtwoord;

        ActieveReserveringen = new List<Reservering>();
        Betaalgeschiedenis = new List<Factuur>();
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