using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/Backoffice")] 
public class AccountMedewerkerBackofficeController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private IPasswordHasher<Account> _passwordHasher = new PasswordHasher<Account>();

    public AccountMedewerkerBackofficeController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountMedewerkerBackoffice>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountMedewerkerBackoffice>().ToListAsync();
    }

    [HttpGet("Krijg specifiek account")]
    public async Task<ActionResult<AccountMedewerkerBackoffice>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }

    [HttpPost("MaakAccount")]
    public async Task<ActionResult<AccountMedewerkerBackoffice>> PostAccount([FromBody] BackofficeDto accountDto)
    {
        var anyEmail = _context.Accounts.Any(a => a.Email == accountDto.Email);
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        if (accountDto == null) return BadRequest("AccountMedewerkerBackoffice mag niet 'NULL' zijn");

        AccountMedewerkerBackoffice account = new AccountMedewerkerBackoffice(accountDto.Email, accountDto.Wachtwoord, _passwordHasher, _context);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return Ok(new { AccountId = account.AccountId, Message = "Account succesvol aangemaakt." });
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto accountDto)
    {
        var account = await _context.Accounts.OfType<AccountMedewerkerBackoffice>().FirstOrDefaultAsync(a => a.Email == accountDto.Email);
        if (account == null) return Unauthorized(new { message = "Account is niet gevonden"});

        var result = _passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord);

        if (result == PasswordVerificationResult.Failed) return Unauthorized(new { message = "Verkeerd wachtwoord"});
        
        return Ok(new {account.AccountId});
    }

    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> PutAccount(int id, [FromBody] AccountMedewerkerBackoffice updatedAccount)
    {
        if (id != updatedAccount.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccount = await _context.Accounts.FindAsync(id);
        
        
        AccountMedewerkerBackoffice account = new AccountMedewerkerBackoffice(updatedAccount.Email, existingAccount.Wachtwoord, _passwordHasher, _context);
        if (account == null)
        {
            return NotFound();
        }
        
        _context.Accounts.Remove(existingAccount);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("DeleteAccount")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("verhuuraanvraagkeuren")]
    public async Task<ActionResult> VerhuuraanvraagKeuren([FromBody] HuuraanvraagDto huuraanvraagDto)
    {
        if (huuraanvraagDto == null)
        {
            return BadRequest("Huuraanvraaggegevens ontbreken.");
        }

        try
        {
            // Validatie van de data
            if (string.IsNullOrEmpty(huuraanvraagDto.ReserveringId.ToString()))
            {
                return BadRequest("ReserveringsId is verplicht.");
            }

            // Ophalen van de bestaande aanvraag in de database
            var aanvraag = await _context.Reserveringen
                .FirstOrDefaultAsync(a => a.ReserveringId == huuraanvraagDto.ReserveringId);
           
            if (aanvraag == null)
            {
                return NotFound("Huuraanvraag niet gevonden.");
            }
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == aanvraag.AccountId);
            var voertuig = await _context.Voertuigen
                .FirstOrDefaultAsync(a => a.VoertuigId == aanvraag.VoertuigId);
            aanvraag.IsGoedgekeurd = huuraanvraagDto.Keuze;
            aanvraag.Comment = huuraanvraagDto.Comment?? aanvraag.Comment;
            
            if (!aanvraag.IsGoedgekeurd)
            {
                EmailSender.AanvraagAfgekeurd(account.Email, aanvraag.Begindatum, aanvraag.Einddatum, voertuig.Merk, voertuig.Model,voertuig.VoertuigType, aanvraag.Comment);
                _context.Reserveringen.Remove(aanvraag);
            }
            else
            {
                EmailSender.AanvraagGoedgekeurd(account.Email, aanvraag.Begindatum, aanvraag.Einddatum, voertuig.Merk, voertuig.Model, voertuig.VoertuigType, aanvraag.Comment);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Huuraanvraag is bijgewerkt.",
                ReserveringsId = aanvraag.ReserveringId,
                Status = aanvraag.IsGoedgekeurd ? "Goedgekeurd" : "Afgekeurd"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Interne serverfout: {ex.Message}");
        }
    }
    
    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        try
        {
            // Test de e-mailfunctionaliteit
            //EmailSender.TestMail();  // Zorg ervoor dat TestMail geen parameters vereist of dat je ze meegeeft

            // Als alles goed gaat, geef een 200 OK response terug
            return Ok();
        }
        catch (Exception ex)
        {
            // Foutafhandelingsmechanisme als er iets misgaat bij het versturen van de e-mail
            return StatusCode(500, "Er is een fout opgetreden: " + ex.Message);
        }
    }
    
    

}