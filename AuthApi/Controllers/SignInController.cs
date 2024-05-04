using AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/auth/[controller]")]
    [ApiController]
    public class SignInController(SignInManager<IdentityUser> signInManager) : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;

        [HttpPost]
        public async Task <IActionResult> SignIn(SignIn model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    return Ok();
                }

                return Unauthorized();
            }
            return BadRequest();
        }
    }
}
