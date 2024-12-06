using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountZakelijkHuurderController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher, EmailSender emailSender) : ControllerBase
{
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountZakelijkHuurder>>> GetAlleZakelijkHuurderAccounts()
    {
        return await context.ZakelijkHuurderAccounts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountZakelijkHuurder>> GetAccountZakelijkHuurder(int id)
    {
        var accountZakelijkHuurder = await context.ZakelijkHuurderAccounts.FindAsync(id);

        if (accountZakelijkHuurder == null)
        {
            return NotFound();
        }
        return Ok(accountZakelijkHuurder);
    }

    [HttpPost("Maak accounts aan")]
    public async Task<ActionResult<AccountZakelijkHuurder>> PostAccountZakelijkHuurder([FromBody] AccountZakelijkHuurder accountZakelijkHuurder)
    {
        if (accountZakelijkHuurder == null)
        {
            return BadRequest("AccountZakelijkHuurder mag niet 'NULL' zijn");
        }

        accountZakelijkHuurder.Wachtwoord = passwordHasher.HashPassword(accountZakelijkHuurder, accountZakelijkHuurder.Wachtwoord); // hasher toegevoegd
        accountZakelijkHuurder.Account = accountZakelijkHuurder.AccountId;
        
        try
        {
            context.ZakelijkHuurderAccounts.Add(accountZakelijkHuurder);
            await context.SaveChangesAsync();
            
            var bedrijf = await context.Bedrijven.FindAsync(accountZakelijkHuurder.BedrijfsId);
            emailSender.SendEmail(bedrijf);

            return CreatedAtAction(nameof(GetAccountZakelijkHuurder), new { id = accountZakelijkHuurder.AccountId }, accountZakelijkHuurder);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);;
            return StatusCode(500);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await context.ZakelijkHuurderAccounts.FirstOrDefaultAsync(a => a.Email == email);

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
    public async Task<IActionResult> PutAccountZakelijkHuurder(int id, [FromBody] AccountZakelijkHuurder updatedAccountZakelijkHuurder)
    {
        if (id != updatedAccountZakelijkHuurder.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccountZakelijkHuurder = await context.ZakelijkHuurderAccounts.FindAsync(id);
        if (existingAccountZakelijkHuurder == null)
        {
            return NotFound();
        }

        existingAccountZakelijkHuurder.UpdateAccountZakelijkHuurder(updatedAccountZakelijkHuurder);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountZakelijkHuurder(int id)
    {
        var accountZakelijkHuurder = await context.ZakelijkHuurderAccounts.FindAsync(id);
        if (accountZakelijkHuurder == null)
        {
            return NotFound();
        }

        context.ZakelijkHuurderAccounts.Remove(accountZakelijkHuurder);
        await context.SaveChangesAsync();

        return NoContent();
    }
}