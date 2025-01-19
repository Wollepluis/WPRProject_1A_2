/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.Modellen.Voertuigen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/Frontoffice")]
public class AccountMedewerkerFrontofficeController : ControllerBase
{
    private readonly Context _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountMedewerkerFrontofficeController(Context context, IPasswordHasher<Account> passwordHasher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountMedewerkerFrontoffice>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountMedewerkerFrontoffice>().ToListAsync();
    }

    [HttpGet("Krijg specifiek account")]
    public async Task<ActionResult<AccountMedewerkerFrontoffice>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }

    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountMedewerkerFrontoffice>> PostAccount([FromBody] FrontofficeDto accountDto)
    {
        var anyEmail = _context.Accounts.Any(a => a.Email == accountDto.Email);
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        if (accountDto == null) return BadRequest("AccountMedewerkerFrontoffice mag niet 'NULL' zijn");

        AccountMedewerkerFrontoffice account = new AccountMedewerkerFrontoffice(accountDto.Email, accountDto.Wachtwoord, _passwordHasher, _context);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return Ok(new { AccountId = account.AccountId, Message = "Account succesvol aangemaakt." });
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]LoginDto accountDto)
    {
        var account = await _context.Accounts.OfType<AccountMedewerkerFrontoffice>().FirstOrDefaultAsync(a => a.Email == accountDto.Email);
        if (account == null) return Unauthorized(new { message = "Account is niet gevonden"});

        var result = _passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord);

        if (result == PasswordVerificationResult.Failed) return Unauthorized(new { message = "Verkeerd wachtwoord"});
        
        return Ok(new {account.AccountId});
    }

    [HttpPut("Update Account")]
    public async Task<IActionResult> PutAccount(int id, [FromBody]AccountMedewerkerFrontoffice updatedAccount)
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

    [HttpPut("updatevoertuigstatus")]
    public async Task<IActionResult> PutVoertuigStatus([FromQuery]int id, [FromQuery]DateTime begindatum, [FromQuery] DateTime einddatum)
    {
        var voertuig = await _context.Voertuigen.FindAsync(id);
        if (voertuig == null) return NotFound();

        switch (voertuig.VoertuigStatus)
        {
            case "Gereserveerd":
                voertuig.VoertuigStatus = "Uitgegeven";
                break;
            case "Beschikbaar":
                voertuig.VoertuigStatus = "Uitgegeven";
                break;
            case "Uitgegeven":
                voertuig.VoertuigStatus = "Beschikbaar";
                break;
            default:
                return BadRequest("Ongeldige VoertuigStatus");
        }
        
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
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
}*/