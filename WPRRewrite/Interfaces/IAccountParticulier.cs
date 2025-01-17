using System.ComponentModel.DataAnnotations.Schema;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen;

namespace WPRRewrite.Interfaces;

public interface IAccountParticulier
{
    string Naam { get; set; }
    int Telefoonnummer { get; set; }
    
    int AdresId { get; set; }
    [ForeignKey("AdresId")] Adres? Adres { get; set; }

    void UpdateAccount(AccountDto account);
}