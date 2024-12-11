using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/Particulier")]
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
    
    [HttpGet("KrijgAlleAccounts")]
    public async Task<ActionResult<IEnumerable<AccountParticulier>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountParticulier>().ToListAsync();
    }

    [HttpGet("KrijgSpecifiekAccount")]
    public async Task<ActionResult<AccountParticulier>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }
    
    [HttpPost("MaakAccount")]
    public async Task<ActionResult<AccountParticulier>> PostAccount([FromBody] ParticulierDto accountDto, string postcode, int huisnummer)
    {
        Adres adres = await _adresService.ZoekAdresAsync(postcode, huisnummer);
        if (adres == null) return NotFound("Address not found for the given postcode and huisnummer.");
        _context.Adressen.Add(adres);
        await _context.SaveChangesAsync();
        
        
        if (accountDto == null) return BadRequest("AccountParticulier mag niet 'NULL' zijn");
        
        AccountParticulier account = new AccountParticulier(accountDto.Email, accountDto.Wachtwoord, accountDto.Naam, adres.AdresId, accountDto.Telefoonnummer, _passwordHasher);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);
        
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return account;
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
        _passwordHasher.HashPassword(account, password);
        
        if (account == null || string.IsNullOrEmpty(account.Wachtwoord))
        {
            return Unauthorized("Account of wachtwoord niet gevonden.");
        }

        var result = account.WachtwoordVerify(password);

        if (result == PasswordVerificationResult.Failed) return Unauthorized("Fout wachtwoord");

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