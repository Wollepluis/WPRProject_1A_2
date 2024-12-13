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
    public async Task<ActionResult<AccountParticulier>> PostAccount([FromBody] ParticulierDto accountDto)
    {
        var anyEmail = _context.Accounts.Any(a => a.Email == accountDto.Email);
        
        if (anyEmail) return BadRequest("Een gebruiker met deze email bestaat al");
        if (accountDto == null) return BadRequest("Accountgegevens mogen niet leeg zijn.");
        
        Adres adres = await _adresService.ZoekAdresAsync(accountDto.Postcode, accountDto.Huisnummer);
        
        if (adres == null) return NotFound("Address not found for the given postcode and huisnummer.");
        
        _context.Adressen.Add(adres);
        await _context.SaveChangesAsync();
                
        AccountParticulier account = new AccountParticulier(accountDto.Email, accountDto.Wachtwoord, accountDto.Naam, adres.AdresId, accountDto.Telefoonnummer, _passwordHasher);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);
        
        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        return Ok(new { AccountId = account.AccountId, Message = "Account succesvol aangemaakt." });
    }
    
    [HttpPost("Login")]
         public async Task<IActionResult> Login([FromBody] LoginDto accountDto)
         {
             var account = await _context.Accounts.OfType<AccountParticulier>().FirstOrDefaultAsync(a => a.Email == accountDto.Email);
             string hashedPassword = _passwordHasher.HashPassword(account, accountDto.Wachtwoord);
             
             if (account == null) return Unauthorized("Account is niet gevonden");
             if (_passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord) == PasswordVerificationResult.Failed) return Unauthorized("Verkeerd wachtwoord");
     
             return Ok();
         }

    [HttpPut("updateaccount")]
    public async Task<IActionResult> PutAccount([FromQuery]int id, [FromBody]ParticulierDto dto)
    {
        var existingAccount = await _context.Accounts.FindAsync(id);
        if (existingAccount == null)
        {
            return NotFound();
        }

        Adres adres = await _adresService.ZoekAdresAsync(dto.Postcode, dto.Huisnummer);
        if (adres == null) return NotFound("Address not found for the given postcode and huisnummer.");
        
        _context.Adressen.Add(adres);
        await _context.SaveChangesAsync();

        AccountParticulier updatedAccount = new AccountParticulier(dto.Email, dto.Wachtwoord, dto.Naam, adres.AdresId,
            dto.Telefoonnummer, _passwordHasher);
        
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