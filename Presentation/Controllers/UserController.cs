using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using poketra_vyrt_api.Application.User.Command;
using poketra_vyrt_api.Infrastructure.Security.Policy;

namespace poketra_vyrt_api.Presentation.Controllers;

[ApiController]
[Route("user")]
public class UserController(IMediator mediator): ControllerBase
{
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpCommand cmd)
    {
        var token = await mediator.Send(cmd);
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand cmd)
    {
        var token = await mediator.Send(cmd);
        return Ok(token);
    }
    
    [AllowNotVerified]
    [HttpPost("resend-otp/")]
    public async Task<IActionResult> ResendOtp(SendOtpForPhoneNumberVerificationCommand cmd)
    {
        await mediator.Send(cmd);
        return Ok();
    }

    [AllowNotVerified]
    [HttpPost("validate-account/")]
    public async Task<IActionResult> ValidateAccount(PhoneNumberVerificationAttemptCommand cmd)
    {
        var result = await mediator.Send(cmd);
        return Ok(result);
    }
    
    [HttpGet("can-reach-this")]
    public IActionResult CanReachThis() => Ok("Only activated users can reach this endpoint");
}