using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WPRProject_1A_2.Modellen.Accounts; // Namespace waarin jouw `account`-klasse zit


namespace WPRProject_1A_2.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private CarAndAllContext _context;

        public AccountController()
        {
            _context = new CarAndAllContext();
        }

        [HttpGet("Krijg Account")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return Ok(account);
        }

        [HttpPost("Maak Account")]
        public async Task<IActionResult> PostAccount(string email, string password)
        {
            Account account = new Account(email, password);
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }
    }
}