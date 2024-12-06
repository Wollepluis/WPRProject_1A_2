using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountParticulierController(CarAndAllContext context, PasswordHasher<AccountParticulier> passwordHasher, IAdresService adresService) : ControllerBase //Parameters toegevoed
{
     
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountParticulier>>> GetAlleParticuliersAccounts()
    {
        return await context.ParticulierAccounts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountParticulier>> GetAccountParticulier(int id)
    {
        var accountParticulier = await context.ParticulierAccounts.FindAsync(id);

        if (accountParticulier == null)
        {
            return NotFound();
        }
        return Ok(accountParticulier);
    }

    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountParticulier>> PostAccountParticulier([FromBody] AccountParticulier accountParticulier, string postcode, int huisnummer)
    {
        if (accountParticulier == null)
        {
            return BadRequest("AccountParticulier mag niet 'NULL' zijn");
        }
        
        accountParticulier.Wachtwoord = passwordHasher.HashPassword(accountParticulier, accountParticulier.Wachtwoord); // hasher toegevoegd

        var adres = await adresService.ZoekAdresAsync(postcode, huisnummer);

        if (adres == null)
        {
            return NotFound("Address not found for the given postcode and huisnummer.");
        }

        accountParticulier.ParticulierAdres = adres.AdresId;
        accountParticulier.Account = accountParticulier.AccountId;

        context.ParticulierAccounts.Add(accountParticulier);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountParticulier), new { id = accountParticulier.ParticulierAccountId }, accountParticulier);
    }
    //Login toegevoegd
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await context.ParticulierAccounts.FirstOrDefaultAsync(a => a.Email == email);

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
    public async Task<IActionResult> PutAccountParticulier(int id, [FromBody] AccountParticulier updatedAccountParticulier)
    {
        if (id != updatedAccountParticulier.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccountParticulier = await context.ParticulierAccounts.FindAsync(id);
        if (existingAccountParticulier == null)
        {
            return NotFound();
        }

        existingAccountParticulier.UpdateAccountParticulier(updatedAccountParticulier);

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountParticulier(int id)
    {
        var accountParticulier = await context.ParticulierAccounts.FindAsync(id);
        if (accountParticulier == null)
        {
            return NotFound();
        }

        context.ParticulierAccounts.Remove(accountParticulier);
        await context.SaveChangesAsync();

        return NoContent();
    }
}