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
    public async Task<ActionResult<IEnumerable<IAccountMedewerker>>> GetAlleMedewerkerFrontofficeAccounts()
    {
        return await context.Accounts.OfType<AccountMedewerkerFrontoffice>().ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountMedewerkerFrontoffice>> GetAccountMedewerkerFrontoffice(int id)
    {
        var account = await context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }

    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountMedewerkerFrontoffice>> PostAccountMedewerkerFrontoffice([FromBody] AccountMedewerkerFrontoffice account)
    {
        if (account == null)
        {
            return BadRequest("AccountMedewerkerFrontoffice mag niet 'NULL' zijn");
        }
        
        account.Wachtwoord = passwordHasher.HashPassword(account, account.Wachtwoord); // hasher toegevoegd
        account.Account = account.AccountId;

        context.Accounts.Add(account);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountMedewerkerFrontoffice), new { id = account.AccountId }, account);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

        if (account == null)
        {
            return Unauthorized();
        }
        
        var result = account.WachtwoordVerifieren(password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        return Ok("Inloggen succesvol");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAccountMedewerkerFrontoffice(int id, [FromBody] AccountMedewerkerFrontoffice updatedAccount)
    {
        if (id != updatedAccount.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccount = await context.Accounts.FindAsync(id);
        if (existingAccount == null)
        {
            return NotFound();
        }

        existingAccount.UpdateAccount(updatedAccount);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }

        context.Accounts.Remove(account);
        await context.SaveChangesAsync();

        return NoContent();
    }
}