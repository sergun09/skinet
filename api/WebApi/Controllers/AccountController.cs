using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using WebApi.Dtos;
using WebApi.Extensions;

namespace WebApi.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _manager;

    public AccountController(SignInManager<AppUser> manager)
    {
        _manager = manager;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDTO registerDTO)
    {
        var user = new AppUser
        {
            FirstName = registerDTO.FirstName,
            LastName = registerDTO.LastName,
            Email = registerDTO.Email,
            UserName = registerDTO.Email
        };

        var result = await _manager.UserManager.CreateAsync(user, registerDTO.Password);

        if (!result.Succeeded) {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem();
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await _manager.SignOutAsync();

        return NoContent();
    }

    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();

        AppUser user;
        try 
        {
            user = await _manager.UserManager.GetUserByEmailWithAddress(User);
        }catch(AuthenticationException ex) 
        {
            return Unauthorized(ex.Message);
        }
        

        return Ok(new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            Address = user.Adress?.ToDto(),
            Roles = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpGet("auth-status")]
    public ActionResult GetAuthState()
    {
        return Ok(new {IsAuthenticated = User.Identity?.IsAuthenticated ?? false});
    }

    [Authorize]
    [HttpPost("address")]
    public async Task<ActionResult<Adress>> CreateOrUpdateAddress(AddressDTO addressDTO)
    {
        var user = await _manager.UserManager.GetUserByEmailWithAddress(User);

        if(user.Adress is null)
        {
            user.Adress = addressDTO.ToEntity();
        }
        else 
        {
            user.Adress.UpdateFromDto(addressDTO);
        }

        var result = await _manager.UserManager.UpdateAsync(user);

        if (!result.Succeeded) return BadRequest("Problème avec la mise à jour de l'adresse");

        return Ok(user.Adress.ToDto());
    } 
}
