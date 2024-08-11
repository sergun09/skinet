using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
