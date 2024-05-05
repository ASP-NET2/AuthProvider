using AuthApi.Context;
using AuthApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerificationController(DataContext context, UserManager<IdentityUser> userManager, HttpClient httpClient) : ControllerBase
{
    private readonly DataContext _context = context;
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly HttpClient _httpClient = httpClient;


    [HttpPost("verify")]
    public async Task<IActionResult> VerifyEmail(VerificationMessage model)
    {
        var apiUrl = "https://manero-verificationprovider.azurewebsites.net/api/Verification";

        var response = await _httpClient.PostAsJsonAsync(apiUrl, model);

        if(response.IsSuccessStatusCode)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=> x.Email == model.Email);
            if(user != null && !user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
        else if(response.StatusCode == HttpStatusCode.Unauthorized) 
        { 
            return Unauthorized();
        }
        else if(response.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound();
        }
        else if(response.StatusCode == HttpStatusCode.BadRequest)
        {
            return StatusCode((int)response.StatusCode);
        }
        else
        {
            return StatusCode((int)response.StatusCode);
        }
    }


   
}

