using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WPRProject_1A_2.Modellen.Abonnementen; // Namespace waarin jouw `Adres`-klasse zit


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
    }
}