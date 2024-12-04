using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountZakelijkHuurderController(CarAndAllContext context) : ControllerBase
{
    private AdresController _adresController;
    
    [HttpGet]
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

    [HttpPost]
    public async Task<ActionResult<AccountZakelijkHuurder>> PostAccountZakelijkHuurder([FromBody] AccountZakelijkHuurder accountZakelijkHuurder)
    {
        if (accountZakelijkHuurder == null)
        {
            return BadRequest("AccountZakelijkHuurder mag niet 'NULL' zijn");
        }

        accountZakelijkHuurder.Account = accountZakelijkHuurder.AccountId;

        context.ZakelijkHuurderAccounts.Add(accountZakelijkHuurder);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountZakelijkHuurder), new { id = accountZakelijkHuurder.AccountId }, accountZakelijkHuurder);
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