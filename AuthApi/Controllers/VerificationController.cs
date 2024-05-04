using AuthApi.Context;
using AuthApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationController(DataContext context, UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly DataContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        /* [HttpPost]
         public async Task<IActionResult> Verify(VerificationRequest model)
         {
             if (ModelState.IsValid)
             {
                 var verificationCode = await _context.AccountVerification.FirstOrDefault(x=> x.Email == model.Email);
                 if (verificationCode != null)
                 {
                     if(verificationCode.VerificationCode == model.VerificationCode) 
                     {
                         var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.Email == model.Email){

                             if(user != null)
                             {
                                 user.EmailConfirmed = true;
                                 var result = await _userManager.UpdateAsync(user);
                                 if(result.Succeeded)
                                 {
                                     return Ok();
                                 }
                             }
                         }

                     }

                 }

             }
             return BadRequest();

         }
     }*/
    }
}