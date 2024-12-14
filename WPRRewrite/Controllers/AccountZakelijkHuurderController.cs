using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/ZakelijkHuurder")]
public class AccountZakelijkHuurderController : ControllerBase
{
    
    private readonly CarAndAllContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;
    private readonly EmailSender _emailSender;

    public AccountZakelijkHuurderController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher, EmailSender emailSender)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountZakelijkHuurder>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountZakelijkHuurder>().ToListAsync();
    }

    [HttpGet("Krijg specifiek account")]
    public async Task<ActionResult<AccountZakelijkHuurder>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }
    
    [HttpGet("Krijg alle autos")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> GetAutos()
    {
        var autos = await _context.Voertuigen
            .Where(v => EF.Property<string>(v, "VoertuigType") == "Auto")
            .ToListAsync();

        return Ok(autos);
    }
    
    [HttpGet("Filter voertuigen")]
    public async Task<ActionResult<IEnumerable<IVoertuig>>> FilterVoertuigen(string voertuigType)
    {
        if (string.IsNullOrWhiteSpace(voertuigType))
        {
            return BadRequest("VoertuigType is verplicht meegegeven te worden");
        }
        
        var voertuigen = await _context.Voertuigen
            .Where(v => EF.Property<string>(v, "VoertuigType") == voertuigType)
            .ToListAsync();

        return Ok(voertuigen);
    }
    
    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountZakelijkHuurder>> PostAccount([FromBody] ZakelijkHuurderDto accountDto)
    {
        if (accountDto == null)
        {
            return BadRequest("AccountZakelijkHuurder mag niet 'NULL' zijn");
        }
        
        AccountZakelijkHuurder account = new AccountZakelijkHuurder(accountDto.Email, accountDto.Wachtwoord, accountDto.BedrijfId ,_passwordHasher);

        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);

        try
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            /*var bedrijf = await context.Bedrijven.FindAsync(accountZakelijkHuurder.BedrijfsId);
            emailSender.SendEmail(bedrijf);*/
            EmailSender.SendEmail(account);
            return CreatedAtAction(nameof(GetAccount), new { id = account.AccountId }, account);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);;
            return StatusCode(500);
        }
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto accountDto)
    {
        var account = await _context.Accounts.OfType<AccountZakelijkHuurder>().FirstOrDefaultAsync(a => a.Email == accountDto.Email);
        string hashedPassword = _passwordHasher.HashPassword(account, accountDto.Wachtwoord);
             
        if (account == null) return Unauthorized("Account is niet gevonden");
        if (_passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord) == PasswordVerificationResult.Failed) return Unauthorized("Verkeerd wachtwoord");
     
        return Ok(account.AccountId);
    }

    [HttpPut("Update Account")]
    public async Task<IActionResult> PutAccount(int id, [FromBody] AccountZakelijkHuurder updatedAccount)
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