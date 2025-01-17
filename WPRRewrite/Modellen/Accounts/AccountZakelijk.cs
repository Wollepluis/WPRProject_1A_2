using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;

namespace WPRRewrite.Modellen.Accounts;

public abstract class AccountZakelijk : Account, IAccountZakelijk
{
    public int BedrijfId { get; set; }
    [ForeignKey(nameof(BedrijfId))] public Bedrijf Bedrijf { get; set; }
    
    protected AccountZakelijk() { }
    protected AccountZakelijk(string email, string wachtwoord, int bedrijfId)
        : base(email, wachtwoord)
    {
        BedrijfId = bedrijfId;
    }
}