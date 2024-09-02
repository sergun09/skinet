using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [Route("api/errors")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet("unauthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFound() {
            return NotFound();
        }

        [HttpGet("internalerror")]
        public ActionResult GetInternalError() 
        {
            throw new Exception();
        }

        [HttpPost("validationerror")]
        public ActionResult GetError(CreateProductDTO dto) {
            return Ok();
        }

        [Authorize]
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok($"Test du User connecté {name}, {id}");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("admin-secret")]
        public IActionResult GetAdminSecret()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            var roles = User.FindFirstValue(ClaimTypes.Role);
            return Ok(new
            {
                name,id,isAdmin,roles
            });
        }
    }
}
