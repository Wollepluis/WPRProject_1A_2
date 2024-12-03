using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WPRProject_1A_2.Modellen.Accounts; // Namespace waarin jouw `account`-klasse zit


namespace WPRProject_1A_2.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private CarAndAllContext _context;
        private readonly PasswordHasher<Account> _passwordHasher;
        public AccountController()
        {
            _context = new CarAndAllContext();
            _passwordHasher = new PasswordHasher<Account>(); // Initialiseer de PasswordHasher
        }

        [HttpGet("Krijg Account")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return Ok(account);
        }

        [HttpPost("Maak Account")]
        public async Task<IActionResult> PostAccount(Account account)
        {
            //var account = new Account(email, password);
            
            //account.Wachtwoord = _passwordHasher.HashPassword(account, password);
            
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (account == null)
            {
                return Unauthorized(); // Account niet gevonden
            }

            // Vergelijk het gehashte wachtwoord
            var result = _passwordHasher.VerifyHashedPassword(account, account.Wachtwoord, password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(); // Wachtwoord incorrect
            }

            return Ok("Inloggen succesvol");
        }

        [HttpDelete("Verwijder Account")]
        public async Task<IActionResult> VerwijderAccount(int id)
        {
            //BevestigingVerwijderen();

            Account? account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}