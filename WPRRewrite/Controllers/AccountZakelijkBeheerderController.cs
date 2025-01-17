using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/ZakelijkBeheerder")]
public class AccountZakelijkBeheerderController : ControllerBase
{
    
    private Context _context;
    private IPasswordHasher<Account> _passwordHasher;

    public AccountZakelijkBeheerderController(Context context, IPasswordHasher<Account> passwordHasher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountZakelijkBeheerder>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountZakelijkBeheerder>().ToListAsync();
    }

    [HttpGet("KrijgBedrijfId")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> GetBedrijfsId(int accountId)
    {
        var account = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(a => a.AccountId == accountId);
        if (account == null) return NotFound("Account niet gevonden.");
        return Ok(account.BedrijfId);
    }
    
    [HttpGet("KrijgSpecifiekAccount")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null) return NotFound();
        return Ok(account);
    }
    [HttpGet("KrijgAccountEmail")]
    public async Task<ActionResult<string>> GetAccountnaam(int accountId)
    {
        var account =  await _context.Accounts.OfType<AccountZakelijk>()
            .FirstOrDefaultAsync(a => a.AccountId == accountId);
        return Ok(account.Email);

    }
    [HttpGet("KrijgSpecifiekAccountEmail")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> GetAccountEmail(string email)
    {
        var account = await _context.Accounts.OfType<AccountZakelijkBeheerder>().Where(a => a.Email == email).FirstOrDefaultAsync();

        if (account == null) return NotFound();
        return Ok(account);
    }
    
    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> PostAccount(ZakelijkBeheerderDto accountDto)
    {
        if (accountDto == null) return BadRequest("AccountZakelijkBeheerder mag niet 'NULL' zijn");

        AccountZakelijkBeheerder account = new AccountZakelijkBeheerder(accountDto.Email, accountDto.Wachtwoord, accountDto.BedrijfId, _passwordHasher, _context);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);

        try
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
            EmailSender.VerstuurBevestigingsEmail(account.Email, bedrijf.Bedrijfsnaam);

            return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);;
            return StatusCode(500);
        }
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto accountDto)
    {
        var account = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(a => a.Email == accountDto.Email);
        if (account == null) return Unauthorized(new { message = "Account is niet gevonden"});

        var result = _passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord);

        if (result == PasswordVerificationResult.Failed) return Unauthorized(new { message = "Verkeerd wachtwoord"});
        
        var reservering = await _context.Reserveringen.FirstOrDefaultAsync(r => r.AccountId == account.AccountId);
        if (reservering != null)
        {
            // Get only the date part (no time)
            var reserveringDate = reservering.Begindatum.Date;
            var currentDate = DateTime.Now.Date;

            // Check if the reservation is tomorrow
            if (reserveringDate == currentDate.AddDays(1) && reservering.Herinnering == false)
            {
                EmailSender.VerstuurHerinneringsEmail(account.Email, reservering.VoertuigId, reservering.Begindatum);
                reservering.UpdateHerinnering();
                await _context.SaveChangesAsync();
            }
        }
        
        return Ok(new {account.AccountId});
    }

    [HttpPut("Voeg medewerker toe aan abonnement")]
    public async Task<IActionResult> VoegMedewerkerToe(string email, Bedrijf bedrijf)
    {
        var account = await _context.Accounts
            .OfType<AccountZakelijk>()
            .FirstOrDefaultAsync(a => a.Email == email);

        if (account == null)
        {
            return BadRequest($"Account met email {email} niet gevonden");
        }

        if (bedrijf.BevoegdeMedewerkers.Contains(account))
        {
            return BadRequest($"Account is al toegevoegd aan abonnement");
        }
        
        bedrijf.VoegMedewerkerToe(account);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("Update Account")]
    public async Task<IActionResult> PutAccount(int id, [FromBody] AccountZakelijkBeheerder updatedAccount)
    {
        if (id != updatedAccount.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccount = await _context.Accounts.FindAsync(id);
        if (existingAccount == null)
        {
            return NotFound();
        }

        existingAccount.UpdateAccount(updatedAccount);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("VerwijderBedrijf")]
    public async Task<IActionResult> DeleteBedrijf(int id)
    {
        try
        {
            var account = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(a => a.AccountId == id);
            var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
            if (bedrijf == null) return NotFound("Er is geen bedrijf gevonden...");
            _context.Bedrijven.Remove(bedrijf);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception e)
        {
            return Unauthorized("U heeft de rechten niet om het acccount te verwijderen...");
        }
    }
}