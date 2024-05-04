using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers;

[Route("api/auth/[controller]")]
[ApiController]
public class SignOutController(SignInManager<IdentityUser> signInManager) : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;

    [HttpGet]
    public async Task<IActionResult> Signout()
    {
        Response.Cookies.Delete("AccessToken");
        await _signInManager.SignOutAsync();
        return Ok(); 
     }
}