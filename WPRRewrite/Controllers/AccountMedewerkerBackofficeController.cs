using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountMedewerkerBackofficeController(CarAndAllContext context) : ControllerBase
{
    private AdresController _adresController;
    
    [HttpGet]
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

    [HttpPost]
    public async Task<ActionResult<AccountMedewerkerBackoffice>> PostAccountMedewerkerBackoffice([FromBody] AccountMedewerkerBackoffice accountMedewerkerBackoffice)
    {
        if (accountMedewerkerBackoffice == null)
        {
            return BadRequest("AccountMedewerkerBackoffice mag niet 'NULL' zijn");
        }

        accountMedewerkerBackoffice.Account = accountMedewerkerBackoffice.AccountId;

        context.BackofficeAccounts.Add(accountMedewerkerBackoffice);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountMedewerkerBackoffice), new { id = accountMedewerkerBackoffice.AccountId }, accountMedewerkerBackoffice);
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