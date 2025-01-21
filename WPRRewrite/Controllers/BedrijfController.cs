using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("[Controller]")]
public class BedrijfController(Context context) : ControllerBase
{
    private readonly Context _context = context ?? throw new ArgumentNullException(nameof(context));

    [HttpGet("GetBedrijven")]
    public async Task<ActionResult<IEnumerable<Bedrijf>>> GetBedrijven([FromQuery] int? bedrijfId)
    {
        try
        {
            IQueryable<Bedrijf> query = _context.Bedrijven;

            if (bedrijfId.HasValue)
            {
                var bedrijf = await query
                    .Include(b => b.BevoegdeMedewerkers)
                    .FirstOrDefaultAsync(b => b.BedrijfId == bedrijfId);
                if (bedrijf == null)
                    return NotFound(new { Message = $"Bedrijf met ID {bedrijfId} niet gevonden" });

                return Ok(bedrijf);
            }

            var bedrijven = await query.ToListAsync();
            if (bedrijven.Count == 0)
                return NotFound(new { Message = "Geen bedrijven gevonden in de database" });

            return Ok(bedrijven);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { ex.Message });
        }
    }

    [HttpPost("MaakBedrijf")]
    public async Task<ActionResult<Bedrijf>> PostBedrijf([FromBody] BedrijfEnBeheerderDto bedrijfEnBeheerderDto)
    {
        var bedrijfGegevens = bedrijfEnBeheerderDto.Bedrijf;
        var accountGegevens = bedrijfEnBeheerderDto.Account;
        var abonnementGegevens = bedrijfEnBeheerderDto.Abonnement;

        var checkBedrijf = _context.Bedrijven.Any(b => b.KvkNummer == bedrijfGegevens.KvkNummer);
        if (checkBedrijf) 
            return BadRequest("Een bedrijf met dit Kvk-nummer bestaat al...");
        
        var checkEmail = _context.Accounts.Any(a => a.Email == accountGegevens.Email);
        if (checkEmail) 
            return BadRequest(new { Message = "Een gebruiker met deze Email bestaat al" });

        var adres = await _context.Adressen
            .Where(a => a.Postcode == bedrijfGegevens.Postcode && a.Huisnummer == bedrijfGegevens.Huisnummer)
            .FirstOrDefaultAsync();

        if (adres == null)
        {
            adres = await AdresService.ZoekAdresAsync(bedrijfGegevens.Postcode, bedrijfGegevens.Huisnummer);
        }
        else
        {
            return BadRequest(new { Message = "Ongeldig Adres" });
        }

        var nieuwAbonnement = Abonnement.MaakAbonnement(abonnementGegevens);
        _context.Abonnementen.Add(nieuwAbonnement);
        await _context.SaveChangesAsync();

        var bedrijf = new Bedrijf(bedrijfGegevens.KvkNummer, bedrijfGegevens.Bedrijfsnaam, bedrijfGegevens.Domeinnaam, adres.AdresId, nieuwAbonnement.AbonnementId);
        var nieuwAccount = Account.MaakAccount(accountGegevens.AccountType, accountGegevens.Email,
            accountGegevens.Wachtwoord, bedrijf.BedrijfId, null, null);
        
        _context.Accounts.Add(nieuwAccount);
        _context.Bedrijven.Add(bedrijf);
        await _context.SaveChangesAsync();

        try
        {
            EmailSender.VerstuurBevestigingEmail(nieuwAccount.Email);
        }
        catch (Exception e)
        {
            Console.WriteLine("Jammer dan, geen email");
        }

        return Ok(nieuwAccount);
    }

// [HttpGet("KrijgBedrijf")]
// public async Task<ActionResult<BedrijfDto>> GetBedrijf(int id)
// {
//     Bedrijf bedrijf = await _context.Bedrijven.Include(a => a.BevoegdeMedewerkers).ThenInclude(b => b.Reserveringen).FirstOrDefaultAsync(b => b.BedrijfId == id);
//
//     if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
//
//     var abonnement = await _context.Abonnementen.FindAsync(bedrijf.AbonnementId);
//     var adres = await _context.Adressen.FindAsync(bedrijf.BedrijfAdres);
//
//     //BedrijfDto bedrijfDto = new BedrijfDto(bedrijf.KvkNummer, bedrijf.Bedrijfsnaam, bedrijf.Domeinnaam, adres.Postcode, adres.Huisnummer);
//     return Ok(bedrijf);
// }

    [HttpPut("Update")]
    public async Task<IActionResult> PutBedrijf(int id, [FromBody] BedrijfDto updatedBedrijfDto)
    {
        var existingBedrijf = await _context.Bedrijven.FindAsync(id);

        if (existingBedrijf == null) 
            return NotFound("Er is geen bedrijf gevonden...");

        var updatedBedrijf = new Bedrijf(updatedBedrijfDto.Bedrijfsnaam, "@" + updatedBedrijfDto + ".com");
        existingBedrijf.UpdateBedrijf(updatedBedrijf);

        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("VerwijderBedrijf")]
    public async Task<IActionResult> DeleteBedrijf(int id/*, int KvkNummer*/)
    {
        try
        {
            var zakelijkBeheerder = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(a => a.AccountId == id);
            var bedrijf = await _context.Bedrijven.FindAsync(zakelijkBeheerder.BedrijfId);
            if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
            // Zoek het abonnement op basis van id
            var abonnement = await _context.Abonnementen.FindAsync(id);
            if (abonnement == null)
            {
                return NotFound("Abonnement niet gevonden.");
            }
            bedrijf.AbonnementId = 0;
            await _context.SaveChangesAsync();

            // Controleer of het abonnement nog in gebruik is
            var bedrijfMetAbonnement = await _context.Bedrijven
                .FirstOrDefaultAsync(b => b.AbonnementId == abonnement.AbonnementId);

            if (bedrijfMetAbonnement == null)
            {
                // Als het abonnement niet in gebruik is, verwijder het dan
                _context.Abonnementen.Remove(abonnement);
                await _context.SaveChangesAsync();
            }



            _context.Bedrijven.Remove(bedrijf);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception e)
        {
            return Unauthorized("U heeft de rechten niet om het acccount te verwijderen...");
        }

}




/*[HttpGet("KrijgAlleBedrijfstatistieken")]
public async Task<ActionResult<AccountZakelijkBeheerder>> GetKosten(int bedrijfsId)
{
    var bedrijf = await _context.Bedrijven.Include(a => a.BevoegdeMedewerkers).Include(a => a.Adres).Include(bedrijf => bedrijf.Abonnement).FirstOrDefaultAsync(b => b.BedrijfId == bedrijfsId);
    if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
    var accounts = await _context.Accounts.OfType<AccountZakelijk>().Where(a => a.BedrijfId == bedrijfsId).Include(a => a.Reserveringen).ToListAsync();
    double kosten = 0;
    foreach (var account in accounts)
    {
        var reserveringen = await _context.Reserveringen.Where(a => a.AccountId == account.AccountId).ToListAsync();
        foreach (var reservering in reserveringen)
        {
            kosten += reservering.TotaalPrijs;
        }
    }

    var abonnementType = _context.Entry(bedrijf.Abonnement).Property("AbonnementType").CurrentValue?.ToString();
    if (abonnementType == null) abonnementType = "Geen";
    var hoeveelheidGehuurdeAutos = await _context.Reserveringen.Include(a => a.Account).Select(a => a.Account).OfType<AccountZakelijk>().Where(a => a.BedrijfId == bedrijfsId).CountAsync();
    int aantalMedewerkers = bedrijf.BevoegdeMedewerkers.Count;
    BedrijfstatistiekenDto statistieken = new BedrijfstatistiekenDto(kosten, hoeveelheidGehuurdeAutos, aantalMedewerkers, abonnementType, bedrijf.Bedrijfsnaam, bedrijf.Adres);
    return Ok(new { Statistieken = statistieken, Abonnement = bedrijf.Abonnement });
}*/

    [HttpPost("VoegMedewerkerToe")]
    public async Task<ActionResult<Bedrijf>> PostMedewerker(ZakelijkHuurderDto accountZakelijkDto)
    {
        var anyEmail = _context.Accounts.Any(a => a.Email == accountZakelijkDto.Email);
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        
        var bedrijf = await _context.Bedrijven.Include(a => a.BevoegdeMedewerkers).FirstOrDefaultAsync(a => a.BedrijfId == accountZakelijkDto.BedrijfId);
        if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
        
        var nieuwAccount = new AccountZakelijkHuurder(accountZakelijkDto.Email, accountZakelijkDto.Wachtwoord, accountZakelijkDto.BedrijfId);
        
        AccountZakelijkBeheerder account = bedrijf.BevoegdeMedewerkers.OfType<AccountZakelijkBeheerder>().FirstOrDefault();
        if (account != null) EmailSender.VerstuurBevestigingEmail(account.Email);
        
        await _context.SaveChangesAsync();

        return Ok("Account Toegevoegd");
    }

[HttpDelete("BeheerderVerwijdertHuurder")]
public async Task<ActionResult> DeleteHuurder(int id, [FromQuery] string email)
{
    // Controleer of het beheerdersaccount bestaat
    var account = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(a => a.AccountId == id);
    if (account == null)
        return NotFound("Er is geen beheerdersaccount gevonden...");

    // Controleer of het bedrijf bestaat
    var bedrijf = await _context.Bedrijven.Include(b => b.BevoegdeMedewerkers).FirstOrDefaultAsync(a => account.BedrijfId == a.BedrijfId);
    if (bedrijf == null)
        return NotFound("Er is geen bedrijf gevonden...");

    // Controleer of de huurder bestaat
    if (bedrijf.BevoegdeMedewerkers == null)
        return NotFound("Dit bedrijf heeft geen bevoegde medewerkers.");

    var huurderAccount = bedrijf.BevoegdeMedewerkers.FirstOrDefault(a => a.Email == email);
    if (huurderAccount == null)
        return NotFound("Er is geen huurder met dit emailadres gevonden...");

    // Verwijder de huurder
    _context.Accounts.Remove(huurderAccount);
    await _context.SaveChangesAsync();

    // Probeer een email te sturen
    try
    {
        EmailSender.VerstuurVerwijderEmail(email);
    }
    catch (Exception ex)
    {
        // Log de fout en ga verder
        Console.WriteLine($"Fout bij het versturen van de email: {ex.Message}");
    }

    return NoContent(); // Geen extra data nodig
}
}