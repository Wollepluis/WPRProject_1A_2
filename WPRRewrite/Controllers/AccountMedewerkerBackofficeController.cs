using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountMedewerkerBackofficeController(CarAndAllContext context,PasswordHasher<AccountMedewerkerBackoffice> passwordHasher) : ControllerBase
{
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountMedewerkerBackoffice>>> GetAlleMedewerkerBackofficeAccounts()
    {
        return await context.BackofficeAccounts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountMedewerkerBackoffice>> GetAccountMedewerkerBackoffice(int id)
    {
        var accountMedewerkerBackoffice = await context.BackofficeAccounts.FindAsync(id);

        if (accountMedewerkerBackoffice == null)
        {
            return NotFound();
        }
        return Ok(accountMedewerkerBackoffice);
    }

    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountMedewerkerBackoffice>> PostAccountMedewerkerBackoffice([FromBody] AccountMedewerkerBackoffice accountMedewerkerBackoffice)
    {
        if (accountMedewerkerBackoffice == null)
        {
            return BadRequest("AccountMedewerkerBackoffice mag niet 'NULL' zijn");
        }

        accountMedewerkerBackoffice.Wachtwoord = passwordHasher.HashPassword(accountMedewerkerBackoffice, accountMedewerkerBackoffice.Wachtwoord); // hasher toegevoegd
        accountMedewerkerBackoffice.Account = accountMedewerkerBackoffice.AccountId;

        context.BackofficeAccounts.Add(accountMedewerkerBackoffice);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountMedewerkerBackoffice), new { id = accountMedewerkerBackoffice.AccountId }, accountMedewerkerBackoffice);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await context.BackofficeAccounts.FirstOrDefaultAsync(a => a.Email == email);

        if (account == null)
        {
            return Unauthorized();
        }
        
        var result = passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        return Ok("Inloggen succesvol");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAccountMedewerkerBackoffice(int id, [FromBody] AccountMedewerkerBackoffice updatedAccountMedewerkerBackoffice)
    {
        if (id != updatedAccountMedewerkerBackoffice.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccountMedewerkerBackoffice = await context.BackofficeAccounts.FindAsync(id);
        if (existingAccountMedewerkerBackoffice == null)
        {
            return NotFound();
        }

        existingAccountMedewerkerBackoffice.UpdateAccountMedewerkerBackoffice(updatedAccountMedewerkerBackoffice);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountMedewerkerBackoffice(int id)
    {
        var accountMedewerkerBackoffice = await context.BackofficeAccounts.FindAsync(id);
        if (accountMedewerkerBackoffice == null)
        {
            return NotFound();
        }

        context.BackofficeAccounts.Remove(accountMedewerkerBackoffice);
        await context.SaveChangesAsync();

        return NoContent();
    }
}