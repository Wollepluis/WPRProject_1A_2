using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountMedewerkerFrontofficeController(CarAndAllContext context) : ControllerBase
{
    private AdresController _adresController;
    
    [HttpGet]
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

    [HttpPost]
    public async Task<ActionResult<AccountMedewerkerFrontoffice>> PostAccountMedewerkerFrontoffice([FromBody] AccountMedewerkerFrontoffice accountMedewerkerFrontoffice)
    {
        if (accountMedewerkerFrontoffice == null)
        {
            return BadRequest("AccountMedewerkerFrontoffice mag niet 'NULL' zijn");
        }

        accountMedewerkerFrontoffice.Account = accountMedewerkerFrontoffice.AccountId;

        context.FrontofficeAccounts.Add(accountMedewerkerFrontoffice);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountMedewerkerFrontoffice), new { id = accountMedewerkerFrontoffice.AccountId }, accountMedewerkerFrontoffice);
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