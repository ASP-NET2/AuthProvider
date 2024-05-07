using AuthApi.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace AuthApi.Controllers;

[Route("api/auth/[controller]")]
[ApiController]
public class SignUpController(UserManager<IdentityUser> userManager, ServiceBusSender sender) : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly ServiceBusSender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUp model)
    {
        if (ModelState.IsValid)
        {
            if(!await _userManager.Users.AnyAsync(x=> x.Email == model.Email))
            {
                var identityUser = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(identityUser, model.Password);

                if (result.Succeeded)
                {
                    return await SendVerificationCodeAsync(model.Email);   
                }

            }
           
            return Conflict();
        }
        return BadRequest();
    }


    [HttpPost("verificationCode")]
    public async Task<IActionResult> SendVerificationCodeAsync(string email)
    {
        try
        {
            var verificationRequest = new VerificationRequest
            {
                Email = email
            };

            var jsonMessage = JsonConvert.SerializeObject(verificationRequest);

            var serviceBusMessage = new ServiceBusMessage(jsonMessage)
            {
                ContentType = "application/json"
            };

            await _sender.SendMessageAsync(serviceBusMessage);

            return Ok();

        }
        catch (ServiceBusException ex)
        {
            Console.WriteLine($"Service Bus error: {ex.Message}");
            return StatusCode(500, "Error sending verification code");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending verification code: {ex.Message}");
            return StatusCode(500, "Error sending verification code");
        }
    }
}
