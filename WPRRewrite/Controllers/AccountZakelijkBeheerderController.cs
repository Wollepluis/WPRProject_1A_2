using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountZakelijkBeheerderController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher, EmailSender emailSender) : ControllerBase
{
    
    [HttpGet("Krijg alle acoounts")]
    public async Task<ActionResult<IEnumerable<AccountZakelijkBeheerder>>> GetAlleZakelijkBeheerderAccounts()
    {
        return await context.ZakelijkBeheerderAccounts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> GetAccountZakelijkBeheerder(int id)
    {
        var accountZakelijkBeheerder = await context.ZakelijkBeheerderAccounts.FindAsync(id);

        if (accountZakelijkBeheerder == null)
        {
            return NotFound();
        }
        return Ok(accountZakelijkBeheerder);
    }

    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> PostAccountZakelijkBeheerder([FromBody] AccountZakelijkBeheerder accountZakelijkBeheerder)
    {
        if (accountZakelijkBeheerder == null)
        {
            return BadRequest("AccountZakelijkBeheerder mag niet 'NULL' zijn");
        }

        accountZakelijkBeheerder.Wachtwoord = passwordHasher.HashPassword(accountZakelijkBeheerder, accountZakelijkBeheerder.Wachtwoord); // hasher toegevoegd
        accountZakelijkBeheerder.Account = accountZakelijkBeheerder.AccountId;

        try
        {
            context.ZakelijkBeheerderAccounts.Add(accountZakelijkBeheerder);
            await context.SaveChangesAsync();

            var bedrijf = await context.Bedrijven.FindAsync(accountZakelijkBeheerder.BedrijfsId);
            emailSender.SendEmail(bedrijf);

            return CreatedAtAction(nameof(GetAccountZakelijkBeheerder), new { id = accountZakelijkBeheerder.AccountId }, accountZakelijkBeheerder);
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
        var account = await context.ZakelijkBeheerderAccounts.FirstOrDefaultAsync(a => a.Email == email);

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
    public async Task<IActionResult> PutAccountZakelijkBeheerder(int id, [FromBody] AccountZakelijkBeheerder updatedAccountZakelijkBeheerder)
    {
        if (id != updatedAccountZakelijkBeheerder.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccountZakelijkBeheerder = await context.ZakelijkBeheerderAccounts.FindAsync(id);
        if (existingAccountZakelijkBeheerder == null)
        {
            return NotFound();
        }

        existingAccountZakelijkBeheerder.UpdateAccountZakelijkBeheerder(updatedAccountZakelijkBeheerder);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountZakelijkBeheerder(int id)
    {
        var accountZakelijkBeheerder = await context.ZakelijkBeheerderAccounts.FindAsync(id);
        if (accountZakelijkBeheerder == null)
        {
            return NotFound();
        }

        context.ZakelijkBeheerderAccounts.Remove(accountZakelijkBeheerder);
        await context.SaveChangesAsync();

        return NoContent();
    }
}