using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AccountParticulierController : ControllerBase
{
    private readonly CarAndAllContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly IAdresService _adresService;

    public AccountParticulierController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher, IAdresService adresService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _adresService = adresService ?? throw new ArgumentNullException(nameof(adresService));
    }
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountParticulier>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountParticulier>().ToListAsync();
    }

    [HttpGet("Krijg specifiek account")]
    public async Task<ActionResult<AccountParticulier>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }
    
    [HttpPost("maakaccount")]
    public async Task<ActionResult<AccountParticulier>> PostAccount([FromBody] AccountParticulier account, string postcode, int huisnummer)
    {
        if (account == null)
        {
            return BadRequest("AccountParticulier mag niet 'NULL' zijn");
        }

        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);
        
        var adres = await _adresService.ZoekAdresAsync(postcode, huisnummer);

        if (adres == null)
        {
            return NotFound("Address not found for the given postcode and huisnummer.");
        }
        
        account.ParticulierAdres = adres.AdresId;
        account.Account = account.AccountId;

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);

        if (account == null)
        {
            return Unauthorized();
        }

        var result = account.WachtwoordVerify(password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        return Ok("Inloggen succesvol");
    }

    [HttpPut("Update Account")]
    public async Task<IActionResult> PutAccount(int id, [FromBody] AccountParticulier updatedAccount)
    {
        if (id != updatedAccount.AccountId)
        {
            return BadRequest("ID mismatch");
        }

        var existingAccount = await _context.Accounts.FindAsync(id);
        if (existingAccount == null)
        {
            return NotFound();
        }

        existingAccount.UpdateAccount(updatedAccount);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);
        if (account == null)
        {
            return NotFound();
        }

        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    
}