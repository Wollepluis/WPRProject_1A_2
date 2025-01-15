using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Abonnementen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AbonnementController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public AbonnementController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Abonnement>>> GetAllAbonnementen()
    {
        var abonnementen = await _context.Abonnementen.ToListAsync();
        return Ok(abonnementen);
    }
    
    
//nieweu
    [HttpGet("getSpecifiekAbonnement")]
    public async Task<ActionResult<Abonnement>> GetAbonnementById([FromQuery]int id)
    {
        var account = await _context.Accounts.OfType<AccountZakelijk>().FirstOrDefaultAsync(a => a.AccountId == id);
        if (account == null) return Unauthorized(new { message = "Account is niet gevonden"});
        
        var bedrijf = await _context.Bedrijven.Include(bedrijf => bedrijf.ToekomstigAbonnementje).FirstOrDefaultAsync(b => b.BedrijfId == account.BedrijfId);
        if (bedrijf == null) return NotFound("Bedrijf niet gevonden");
        
        var toekomstigAbonnement = await _context.Abonnementen.FindAsync(bedrijf.ToekomstigAbonnement);
        if (toekomstigAbonnement != null && toekomstigAbonnement.Begindatum >= DateTime.Now.Date)
        {
            Console.WriteLine("Begindatum is vandaag of later, abonnement wordt aangepast.");
            var oudAbonnement = await _context.Abonnementen.FindAsync(bedrijf.AbonnementId);
            bedrijf.AbonnementId = (int)bedrijf.ToekomstigAbonnement;
            await _context.SaveChangesAsync();

            if (bedrijf.AbonnementId != oudAbonnement.AbonnementId)
            {
                Console.WriteLine("Oud abonnement wordt verwijderd.");
                if (oudAbonnement != null)
                {
                    _context.Abonnementen.Remove(oudAbonnement);
                }
                await _context.SaveChangesAsync();
            }
        }
        var abonnement = await _context.Abonnementen.FindAsync(bedrijf.AbonnementId);
        if (abonnement == null) return NotFound("Abonnement niet gevonden.");
        
        return Ok(abonnement);
    }

    [HttpPost("Create")]
    public async Task<ActionResult> CreateAbonnement([FromQuery]int accountId, [FromBody] Abonnement abonnement)
    {
        try
        {
            // Zorg ervoor dat het abonnement niet null is
            if (abonnement == null)
                return BadRequest("Abonnement gegevens zijn niet ingevuld.");

            // Zoek het bedrijf dat bij dit account hoort
            var account = await _context.Accounts.OfType<AccountZakelijk>()
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        
            var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
            if (bedrijf == null)
                return NotFound("Bedrijf niet gevonden.");

            // Controleer of er al een abonnement bestaat met dezelfde eigenschappen
            var bestaandAbonnement = await _context.Abonnementen
                .FirstOrDefaultAsync(a => a.AbonnementType == abonnement.AbonnementType && 
                                          a.MaxMedewerkers == abonnement.MaxMedewerkers && 
                                          a.MaxVoertuigen == abonnement.MaxVoertuigen);
        
            if (bestaandAbonnement != null)
            {
                // Als er al een bestaand abonnement is, gebruik dat abonnement
                return Ok(bestaandAbonnement);
            }

            // Voeg het nieuwe abonnement toe aan de database
            _context.Abonnementen.Add(abonnement);
            await _context.SaveChangesAsync();

            // Update het bedrijf met het juiste abonnementId
            bedrijf.AbonnementId = abonnement.AbonnementId;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAbonnementById), new { id = abonnement.AbonnementId }, abonnement);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Er is een interne serverfout opgetreden.");
        }
    }



//neiwu
    [HttpPut("UpdateAbonnement")]
public async Task<IActionResult> UpdateAbonnement(int abonnementId, int accountId, [FromBody] Abonnement toekomstigAbonnement)
{
    var account = await _context.Accounts.OfType<AccountZakelijk>().FirstOrDefaultAsync(a => a.AccountId == accountId);
    if (account == null) return NotFound("Account niet gevonden.");

    var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
    if (bedrijf == null) return NotFound("Bedrijf niet gevonden.");

    // Controleer of het abonnementId overeenkomt met het bedrijf
    if (abonnementId != bedrijf.AbonnementId)
        return BadRequest("Het opgegeven abonnement ID komt niet overeen met het bedrijf.");

    // Zoek het bestaande abonnement
    var bestaandAbonnement = await _context.Abonnementen.FindAsync(abonnementId);
    if (bestaandAbonnement == null) return NotFound("Abonnement niet gevonden.");

    // Zoek naar een bestaand abonnement dat hetzelfde is als het toekomstigAbonnement
    var bestaandToekomstigAbonnement = await _context.Abonnementen
        .FirstOrDefaultAsync(a => a.MaxMedewerkers == toekomstigAbonnement.MaxMedewerkers &&
                                  a.MaxVoertuigen == toekomstigAbonnement.MaxVoertuigen &&
                                  a.AbonnementType == toekomstigAbonnement.AbonnementType);

    // Als het toekomstigAbonnement al bestaat, koppel het dan aan het bedrijf
    if (bestaandToekomstigAbonnement != null)
    {
        bedrijf.ToekomstigAbonnement = bestaandToekomstigAbonnement.AbonnementId;
    }
    else
    {
        // Als het toekomstigAbonnement nog niet bestaat, voeg het dan toe aan de database
        _context.Abonnementen.Add(toekomstigAbonnement);
        await _context.SaveChangesAsync();

        // Koppel het nieuwe toekomstigAbonnement aan het bedrijf
        bedrijf.ToekomstigAbonnement = toekomstigAbonnement.AbonnementId;
    }

    // Sla de wijzigingen van het bedrijf op
    await _context.SaveChangesAsync();

    // Controleer of het oude abonnement niet meer in gebruik is door andere bedrijven
    var isAbonnementInGebruik = await _context.Bedrijven
        .AnyAsync(b => b.AbonnementId == bestaandAbonnement.AbonnementId);

    if (!isAbonnementInGebruik)
    {
        // Als het oude abonnement niet meer in gebruik is, verwijder het dan
        _context.Abonnementen.Remove(bestaandAbonnement);
        await _context.SaveChangesAsync();
    }

    // Verstuur de bevestiging via e-mail
    EmailSender.BevestigingAbonnementWijzigen(account.Email, bestaandAbonnement, toekomstigAbonnement);

    return Ok(toekomstigAbonnement);
}


    [HttpDelete("DeleteAbonnement/{id}")]
    public async Task<IActionResult> DeleteAbonnement(int id)
    {
        try
        {
            // Zoek het abonnement op basis van id
            var abonnement = await _context.Abonnementen.FindAsync(id);
            if (abonnement == null)
            {
                return NotFound("Abonnement niet gevonden.");
            }

            // Controleer of het abonnement nog in gebruik is
            var bedrijfMetAbonnement = await _context.Bedrijven
                .FirstOrDefaultAsync(b => b.AbonnementId == abonnement.AbonnementId);
        
            if (bedrijfMetAbonnement != null)
            {
                // Het abonnement wordt nog gebruikt, dus het mag niet worden verwijderd
                return BadRequest("Abonnement kan niet worden verwijderd, het wordt nog gebruikt door een bedrijf.");
            }

            // Als het abonnement niet in gebruik is, verwijder het dan
            _context.Abonnementen.Remove(abonnement);
            await _context.SaveChangesAsync();

            return NoContent(); // Bevestiging van verwijdering
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Er is een interne serverfout opgetreden.");
        }
    }

}
