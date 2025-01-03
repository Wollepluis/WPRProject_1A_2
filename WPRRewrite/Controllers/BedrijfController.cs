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
[Route("api/Bedrijf")]
public class BedrijfController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IAdresService _adresService;
    public BedrijfController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher, IAdresService adresService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _adresService = adresService ?? throw new ArgumentNullException(nameof(adresService));
    }
    
    [HttpGet("KrijgBedrijfDomein")]
    public async Task<ActionResult<IEnumerable<Bedrijf>>> GetBedrijfDomein(int accountId)
    {
        var account = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(i => i.AccountId == accountId);
        if (account == null) return NotFound("Bedrijf niet gevonden");
        var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
        return Ok(bedrijf.Domeinnaam);
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
        
        BedrijfDto bedrijfDto = new BedrijfDto(bedrijf.KvkNummer, bedrijf.Bedrijfsnaam, bedrijf.Domeinnaam, adres.Postcode, adres.Huisnummer, abonnement.MaxMedewerkers, abonnement.MaxVoertuigen);
        return Ok(bedrijf);
    }

    [HttpPost("MaakBedrijf")]
    public async Task<ActionResult<Bedrijf>> PostBedrijf([FromBody] BedrijfEnBeheerderDto bedrijfEnBeheerderDto)
    {
        if (bedrijfEnBeheerderDto == null) return BadRequest("Bedrijf moet ingevuld zijn!");
        var bedrijfDto = bedrijfEnBeheerderDto.Bedrijf;
        var zakelijkBeheerderDto = bedrijfEnBeheerderDto.Beheerder;
        

        var anyKvk = _context.Bedrijven.Any(a => a.KvkNummer == bedrijfDto.Kvknummer);
        if (anyKvk) return BadRequest("Een bedrijf met dit Kvk-nummer bestaat al...");
        var anyEmail = _context.Accounts.Any(a => a.Email == zakelijkBeheerderDto.Email);
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al...");
        
        var adres = await _context.Adressen.Where(a => a.Huisnummer == bedrijfDto.Huisnummer && a.Postcode == bedrijfDto.Postcode).FirstOrDefaultAsync();
        if (adres == null)
        {
            adres = await _adresService.ZoekAdresAsync(bedrijfDto.Postcode, bedrijfDto.Huisnummer);
            
            if (adres == null) return NotFound("Het adres is niet gevonden met de bijbehorende postcode en huisnummer...");
        
            _context.Adressen.Add(adres);
            await _context.SaveChangesAsync();
        }
        await _context.SaveChangesAsync();
        
        Abonnement abonnement = new PayAsYouGo(bedrijfDto.MaxMedewerkers, bedrijfDto.MaxVoertuigen);

        _context.Abonnementen.Add(abonnement);
        await _context.SaveChangesAsync();
        
        Bedrijf bedrijf = new Bedrijf(bedrijfDto.Kvknummer, bedrijfDto.Bedrijfsnaam, adres.AdresId, abonnement.AbonnementId, bedrijfDto.Domeinnaam);
        AccountZakelijkBeheerder account = new AccountZakelijkBeheerder(zakelijkBeheerderDto.Email, zakelijkBeheerderDto.Wachtwoord, bedrijf.BedrijfId, new PasswordHasher<Account>(), _context);
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);
        bedrijf.BevoegdeMedewerkers.Add(account);
        _context.Accounts.Add(account);
        _context.Bedrijven.Add(bedrijf);
        await _context.SaveChangesAsync();

        try
        {
            EmailSender.VerstuurBevestigingsEmail(account.Email, account.Bedrijf.Bedrijfsnaam);
        }
        catch (Exception e)
        {
            Console.WriteLine("Jammer dan, geen email");
        }

        return Ok(account.AccountId);
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

    [HttpDelete("VerwijderBedrijf")]
    public async Task<IActionResult> DeleteBedrijf(int id/*, int kvknummer*/)
    {
        try
        {
            var zakelijkBeheerder = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(a => a.AccountId == id);
            var bedrijf = await _context.Bedrijven.FindAsync(zakelijkBeheerder.BedrijfId);
            if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
            _context.Bedrijven.Remove(bedrijf);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception e)
        {
            return Unauthorized("U heeft de rechten niet om het acccount te verwijderen...");
        }

        
        
        /*if (bedrijf.KvkNummer != kvknummer) return BadRequest("Kvknummer komt niet overeen...");
        
        Adres adres = await _context.Adressen.FindAsync(bedrijf.BedrijfAdres);
        var abonnement = await _context.Abonnementen.FindAsync(bedrijf.AbonnementId);
        if (adres == null) return NotFound("Er is geen adres gevonden...");
        if (abonnement == null) return NotFound("Geen abonnement gevonden...");*/
        
        
        /*_context.Adressen.Remove(adres);
        _context.Abonnementen.Remove(abonnement);*/
        
    }

    
    
    
    [HttpGet("KrijgAlleBedrijfstatistieken")]
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
        if (abonnementType == null) abonnementType = "Poep";
        var hoeveelheidGehuurdeAutos = await _context.Reserveringen.Include(a => a.Account).Select(a => a.Account).OfType<AccountZakelijk>().Where(a => a.BedrijfId == bedrijfsId).CountAsync();
        int aantalMedewerkers = bedrijf.BevoegdeMedewerkers.Count;
        BedrijfstatistiekenDto statistieken = new BedrijfstatistiekenDto(kosten, hoeveelheidGehuurdeAutos, aantalMedewerkers, abonnementType, bedrijf.Bedrijfsnaam, bedrijf.Adres);
        return Ok(statistieken);
    }

    [HttpPost("VoegMedewerkerToe")]
    public async Task<ActionResult<Bedrijf>> PostMedewerker(ZakelijkHuurderDto accountZakelijkDto)
    {
        var anyEmail = _context.Accounts.Any(a => a.Email == accountZakelijkDto.Email);
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        // Zoek het bedrijf op basis van de BedrijfId
        var bedrijf = await _context.Bedrijven.Include(a => a.BevoegdeMedewerkers).FirstOrDefaultAsync(a => a.BedrijfId == accountZakelijkDto.BedrijfId);
        if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");

        // Maak een nieuw AccountZakelijkHuurder object
        AccountZakelijkHuurder accountZakelijkHuurder = new AccountZakelijkHuurder(accountZakelijkDto.Email, accountZakelijkDto.Wachtwoord, accountZakelijkDto.BedrijfId, new PasswordHasher<Account>(), _context);
        //EmailSender.VerstuurBevestigingsEmail(accountZakelijkHuurder.Email, accountZakelijkHuurder.Bedrijf.Bedrijfsnaam);
        accountZakelijkHuurder.Wachtwoord = _passwordHasher.HashPassword(accountZakelijkHuurder, accountZakelijkDto.Wachtwoord);
        // Voeg de medewerker toe aan het bedrijf
        bedrijf.BevoegdeMedewerkers.Add(accountZakelijkHuurder);
        AccountZakelijkBeheerder account = bedrijf.BevoegdeMedewerkers.OfType<AccountZakelijkBeheerder>().FirstOrDefault();
        if (account != null) EmailSender.VerstuurBevestigingsEmail(account.Email, account.Bedrijf.Bedrijfsnaam);
        
        
        // Voeg het account toe aan de database en sla de wijzigingen op
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