using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BedrijfController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private readonly IAdresService _adresService;
    public BedrijfController(CarAndAllContext context, IAdresService adresService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _adresService = adresService ?? throw new ArgumentNullException(nameof(adresService));
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bedrijf>>> GetAlleBedrijven()
    {
        var bedrijf = await _context.Bedrijven.Include(b => b.BevoegdeMedewerkers)
            .ToListAsync();

        return Ok(bedrijf);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BedrijfDto>> GetBedrijf(int id, int kvknummer)
    {
        Bedrijf bedrijf = await _context.Bedrijven.FindAsync(id);

        if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
        if (bedrijf.KvkNummer != kvknummer) return BadRequest("Kvknummer komt niet overeen...");

        var abonnement = await _context.Abonnementen.FindAsync(bedrijf.AbonnementId);
        var adres = await _context.Adressen.FindAsync(bedrijf.BedrijfAdres);
        
        BedrijfDto bedrijfDto = new BedrijfDto(bedrijf.KvkNummer, bedrijf.Bedrijfsnaam, adres.Postcode, adres.Huisnummer, abonnement.MaxMedewerkers, abonnement.MaxVoertuigen);
        return Ok(bedrijf);
    }

    [HttpPost("MaakBedrijf")]
    public async Task<ActionResult<Bedrijf>> PostBedrijf([FromBody] BedrijfEnBeheerderDto bedrijfEnBeheerderDto)
    {
        if (bedrijfEnBeheerderDto == null) return BadRequest("Bedrijf moet ingevuld zijn!");
        var bedrijfDto = bedrijfEnBeheerderDto.Bedrijf;
        var zakelijkBeheerderDto = bedrijfEnBeheerderDto.Beheerder;
        
        var anyEmail = _context.Accounts.Any(a => a.Email == zakelijkBeheerderDto.Email);
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        
        Adres adres = await _adresService.ZoekAdresAsync(bedrijfDto.Postcode, bedrijfDto.Huisnummer);
        if (adres == null) return NotFound("Geen adres gevonden bij deze postcode en huisnummer");
        Abonnement abonnement = new PayAsYouGo(bedrijfDto.MaxMedewerkers, bedrijfDto.MaxVoertuigen);

        _context.Abonnementen.Add(abonnement);
        _context.Adressen.Add(adres);
        await _context.SaveChangesAsync();

        if (adres == null) return NotFound("Het adres is niet gevonden met de bijbehorende postcode en huisnummer...");
        
        Bedrijf bedrijf = new Bedrijf(bedrijfDto.Kvknummer, bedrijfDto.Bedrijfsnaam, adres.AdresId, abonnement.AbonnementId, "@" + bedrijfDto.Bedrijfsnaam + ".com");
        bedrijf.BevoegdeMedewerkers.Add(new AccountZakelijkBeheerder(zakelijkBeheerderDto.Email, zakelijkBeheerderDto.Wachtwoord, bedrijf.BedrijfId,new PasswordHasher<Account>()));
        
        _context.Bedrijven.Add(bedrijf);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBedrijf), new { id = bedrijf.BedrijfId }, bedrijf);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBedrijf(int id, [FromBody] BedrijfDto updatedBedrijfDto)
    {
        var existingBedrijf = await _context.Bedrijven.FindAsync(id);
        
        if (existingBedrijf == null) return NotFound("Er is geen bedrijf gevonden...");

        Bedrijf updatedBedrijf = new Bedrijf(updatedBedrijfDto.Bedrijfsnaam, "@" + updatedBedrijfDto + ".com");
        existingBedrijf.UpdateBedrijf(updatedBedrijf);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBedrijf(int id, int kvknummer)
    {
        var bedrijf = await _context.Bedrijven.FindAsync(id);
        if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
        if (bedrijf.KvkNummer != kvknummer) return BadRequest("Kvknummer komt niet overeen...");
        
        Adres adres = await _context.Adressen.FindAsync(bedrijf.BedrijfAdres);
        var abonnement = await _context.Abonnementen.FindAsync(bedrijf.AbonnementId);
        if (adres == null) return NotFound("Er is geen adres gevonden...");
        if (abonnement == null) return NotFound("Geen abonnement gevonden...");
        
        _context.Bedrijven.Remove(bedrijf);
        _context.Adressen.Remove(adres);
        _context.Abonnementen.Remove(abonnement);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("VoegMedewerkerToe")]
    public async Task<ActionResult<Bedrijf>> PostMedewerker(ZakelijkHuurderDto accountZakelijkDto)
    {
        var anyEmail = _context.Accounts.Any(a => a.Email == accountZakelijkDto.Email);
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        // Zoek het bedrijf op basis van de BedrijfId
        var bedrijf = await _context.Bedrijven.FindAsync(accountZakelijkDto.BedrijfId);
        if (bedrijf == null)
        {
            return NotFound("Er is geen bedrijf gevonden...");
        }

        // Maak een nieuw AccountZakelijkHuurder object
        AccountZakelijk accountZakelijkHuurder = new AccountZakelijkHuurder(accountZakelijkDto.Email, accountZakelijkDto.Wachtwoord, accountZakelijkDto.BedrijfId, new PasswordHasher<Account>());
        
        _context.Accounts.Add(accountZakelijkHuurder);
        // Voeg de medewerker toe aan het bedrijf
        bedrijf.BevoegdeMedewerkers.Add(accountZakelijkHuurder);
        
        // Voeg het account toe aan de database en sla de wijzigingen op
        await _context.SaveChangesAsync();

        return Ok("Account Toegevoegd");
    }
}