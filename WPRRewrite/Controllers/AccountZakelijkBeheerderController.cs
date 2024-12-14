﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WPRRewrite.Dtos;
using WPRRewrite.Interfaces;
using WPRRewrite.Modellen;
using WPRRewrite.Modellen.Accounts;
using WPRRewrite.SysteemFuncties;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/ZakelijkBeheerder")]
public class AccountZakelijkBeheerderController : ControllerBase
{
    
    private CarAndAllContext _context;
    private IPasswordHasher<Account> _passwordHasher;
    private readonly EmailSender _emailSender;

    public AccountZakelijkBeheerderController(CarAndAllContext context, IPasswordHasher<Account> passwordHasher, EmailSender emailSender)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    }
    
    [HttpGet("Krijg alle accounts")]
    public async Task<ActionResult<IEnumerable<AccountZakelijkBeheerder>>> GetAllAccounts()
    {
        return await _context.Accounts.OfType<AccountZakelijkBeheerder>().ToListAsync();
    }

    [HttpGet("Krijg specifiek account")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> GetAccount(int id)
    {
        var account = await _context.Accounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }
    
    [HttpPost("Maak account aan")]
    public async Task<ActionResult<AccountZakelijkBeheerder>> PostAccount(ZakelijkBeheerderDto accountDto)
    {
        if (accountDto == null) return BadRequest("AccountZakelijkBeheerder mag niet 'NULL' zijn");

        AccountZakelijkBeheerder account = new AccountZakelijkBeheerder(accountDto.Email, accountDto.Wachtwoord, accountDto.BedrijfId, _passwordHasher);
        
        account.Wachtwoord = _passwordHasher.HashPassword(account, account.Wachtwoord);

        try
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var bedrijf = await _context.Bedrijven.FindAsync(account.BedrijfId);
            //emailSender.SendEmail(bedrijf);

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
        var account = await _context.Accounts.OfType<AccountZakelijkBeheerder>().FirstOrDefaultAsync(a => a.Email == accountDto.Email);
        string hashedPassword = _passwordHasher.HashPassword(account, accountDto.Wachtwoord);
             
        if (account == null) return Unauthorized("Account is niet gevonden");
        if (_passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, accountDto.Wachtwoord) == PasswordVerificationResult.Failed) return Unauthorized("Verkeerd wachtwoord");
     
        return Ok(account.AccountId);
    }

    [HttpPut("Voeg medewerker toe aan abonnement")]
    public async Task<IActionResult> VoegMedewerkerToe(string email, Bedrijf bedrijf)
    {
        var account = await _context.Accounts
            .OfType<AccountZakelijk>()
            .FirstOrDefaultAsync(a => a.Email == email);

        if (account == null)
        {
            return BadRequest($"Account met email {email} niet gevonden");
        }

        if (bedrijf.BevoegdeMedewerkers.Contains(account))
        {
            return BadRequest($"Account is al toegevoegd aan abonnement");
        }
        
        bedrijf.VoegMedewerkerToe(account);

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("Update Account")]
    public async Task<IActionResult> PutAccount(int id, [FromBody] AccountZakelijkBeheerder updatedAccount)
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