using Microsoft.AspNetCore.Mvc;
using WPRRewrite.Modellen;

namespace WPRRewrite.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AbonnementController : ControllerBase
{
    private readonly CarAndAllContext _context;

    public AbonnementController(CarAndAllContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

}