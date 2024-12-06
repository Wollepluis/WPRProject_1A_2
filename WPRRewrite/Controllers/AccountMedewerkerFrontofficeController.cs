using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountMedewerkerFrontofficeController(CarAndAllContext context, PasswordHasher<AccountMedewerkerFrontoffice> passwordHasher) : ControllerBase
{
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountMedewerkerFrontoffice>>> GetAlleMedewerkerFrontofficeAccounts()
    {
        return await context.FrontofficeAccounts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountMedewerkerFrontoffice>> GetAccountMedewerkerFrontoffice(int id)
    {
        var accountMedewerkerFrontoffice = await context.FrontofficeAccounts.FindAsync(id);

        if (accountMedewerkerFrontoffice == null)
        {
            return NotFound();
        }
        return Ok(accountMedewerkerFrontoffice);
    }

    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountMedewerkerFrontoffice>> PostAccountMedewerkerFrontoffice([FromBody] AccountMedewerkerFrontoffice accountMedewerkerFrontoffice)
    {
        if (accountMedewerkerFrontoffice == null)
        {
            return BadRequest("AccountMedewerkerFrontoffice mag niet 'NULL' zijn");
        }
        
        accountMedewerkerFrontoffice.Wachtwoord = passwordHasher.HashPassword(accountMedewerkerFrontoffice, accountMedewerkerFrontoffice.Wachtwoord); // hasher toegevoegd
        accountMedewerkerFrontoffice.Account = accountMedewerkerFrontoffice.AccountId;

        context.FrontofficeAccounts.Add(accountMedewerkerFrontoffice);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountMedewerkerFrontoffice), new { id = accountMedewerkerFrontoffice.AccountId }, accountMedewerkerFrontoffice);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await context.FrontofficeAccounts.FirstOrDefaultAsync(a => a.Email == email);

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
    public async Task<IActionResult> PutAccountMedewerkerFrontoffice(int id, [FromBody] AccountMedewerkerFrontoffice updatedAccountMedewerkerFrontoffice)
    {
        if (id != updatedAccountMedewerkerFrontoffice.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccountMedewerkerFrontoffice = await context.FrontofficeAccounts.FindAsync(id);
        if (existingAccountMedewerkerFrontoffice == null)
        {
            return NotFound();
        }

        existingAccountMedewerkerFrontoffice.UpdateAccountMedewerkerFrontoffice(updatedAccountMedewerkerFrontoffice);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountMedewerkerFrontoffice(int id)
    {
        var accountMedewerkerFrontoffice = await context.FrontofficeAccounts.FindAsync(id);
        if (accountMedewerkerFrontoffice == null)
        {
            return NotFound();
        }

        context.FrontofficeAccounts.Remove(accountMedewerkerFrontoffice);
        await context.SaveChangesAsync();

        return NoContent();
    }
}