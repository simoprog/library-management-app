using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace library_management_api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;
    
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    
    protected ActionResult HandleResult<T>(LibraryManagement.Application.Common.Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);
            
        if (result.Errors.Any())
            return BadRequest(new { errors = result.Errors });
            
        return BadRequest(new { error = result.Error });
    }
    
    protected ActionResult HandleResult(LibraryManagement.Application.Common.Result result)
    {
        if (result.IsSuccess)
            return Ok();
            
        if (result.Errors.Any())
            return BadRequest(new { errors = result.Errors });
            
        return BadRequest(new { error = result.Error });
    }
}
