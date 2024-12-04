using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountParticulierController(CarAndAllContext context) : ControllerBase
{
    private IAdresService _adresService;
    
    [HttpGet]
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

    [HttpPost]
    public async Task<ActionResult<AccountParticulier>> PostAccountParticulier([FromBody] AccountParticulier accountParticulier, string postcode, int huisnummer)
    {
        if (accountParticulier == null)
        {
            return BadRequest("AccountParticulier mag niet 'NULL' zijn");
        }

        var adres = await _adresService.ZoekAdresAsync(postcode, huisnummer);

        if (adres == null)
        {
            return NotFound("Address not found for the given postcode and huisnummer.");
        }

        accountParticulier.ParticulierAdres = adres.AdresId;

        context.ParticulierAccounts.Add(accountParticulier);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountParticulier), new { id = accountParticulier.ParticulierAccountId }, accountParticulier);
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