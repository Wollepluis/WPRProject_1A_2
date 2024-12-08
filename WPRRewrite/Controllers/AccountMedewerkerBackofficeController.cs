using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountMedewerkerBackofficeController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AccountMedewerkerBackofficeController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
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

    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountMedewerkerBackoffice>> PostAccount([FromBody] AccountMedewerkerBackoffice account)
    {
        if (account == null)
        {
            return BadRequest("AccountMedewerkerBackoffice mag niet 'NULL' zijn");
        }

        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);
        account.Account = account.AccountId;

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

        if (account == null)
        {
            return Unauthorized();
        }

        var result = account.WachtwoordVerify(password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        return Ok("Inloggen succesvol");
    }

    [HttpPut("Update Account")]
    public async Task<IActionResult> PutAccount(int id, [FromBody] AccountMedewerkerBackoffice updatedAccount)
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