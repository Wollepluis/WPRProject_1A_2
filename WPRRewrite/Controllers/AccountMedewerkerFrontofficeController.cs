using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountMedewerkerFrontofficeController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountMedewerkerFrontofficeController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher)
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

        AccountMedewerkerFrontoffice account = new AccountMedewerkerFrontoffice(accountDto.Email, accountDto.Wachtwoord, _passwordHasher);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return Ok(new { AccountId = account.AccountId, Message = "Account succesvol aangemaakt." });
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto accountDto)
    {
        // Zoek het account op basis van het e-mailadres
        var account = await _context.Accounts.OfType<AccountMedewerkerFrontoffice>()
            .FirstOrDefaultAsync(a => a.Email == accountDto.Email);
    
        // Als het account niet bestaat, geef dan een Unauthorized terug
        if (account == null) return Unauthorized("Account is niet gevonden");

        // Vergelijk het ingevoerde wachtwoord met het gehashte wachtwoord uit de database
        if (_passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord) ==
            PasswordVerificationResult.Failed)
            return Unauthorized($"Verkeerd wachtwoord {account.Wachtwoord} en {accountDto.Wachtwoord}");

        // Als het wachtwoord correct is, geef het AccountId terug
        return Ok(account.AccountId);
    }

    [HttpPut("Update Account")]
    public async Task<IActionResult> PutAccount(int id, [FromBody] AccountMedewerkerFrontoffice updatedAccount)
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
}