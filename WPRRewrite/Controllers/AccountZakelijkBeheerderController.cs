using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountZakelijkBeheerderController(CarAndAllContext context) : ControllerBase
{
    private AdresController _adresController;
    
    [HttpGet]
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

    [HttpPost]
    public async Task<ActionResult<AccountZakelijkBeheerder>> PostAccountZakelijkBeheerder([FromBody] AccountZakelijkBeheerder accountZakelijkBeheerder)
    {
        if (accountZakelijkBeheerder == null)
        {
            return BadRequest("AccountZakelijkBeheerder mag niet 'NULL' zijn");
        }

        accountZakelijkBeheerder.Account = accountZakelijkBeheerder.AccountId;

        context.ZakelijkBeheerderAccounts.Add(accountZakelijkBeheerder);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountZakelijkBeheerder), new { id = accountZakelijkBeheerder.AccountId }, accountZakelijkBeheerder);
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