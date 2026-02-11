using MediatR;
using Microsoft.AspNetCore.Mvc;
using poketra_vyrt_api.Application.User.Command;

namespace poketra_vyrt_api.Presentation.Controllers;

[ApiController]
[Route("user")]
public class UserController(IMediator mediator): ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpCommand cmd)
    {
        var userId = await mediator.Send(cmd);
        return CreatedAtAction(nameof(ValidateAccount), new { userId }, userId);
    }
    
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(SendOtpForPhoneNumberVerificationCommand cmd)
    {
        await mediator.Send(cmd);
        return Ok();
    }

    [HttpPost("validate-account/{userId}")]
    public IActionResult ValidateAccount(Guid userId)
    {
        return Ok();
    }
}