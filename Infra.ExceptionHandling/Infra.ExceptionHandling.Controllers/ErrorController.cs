using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Infra.ExceptionHandling.Controllers
{
  [ApiController]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class ErrorController : Controller
  {
    private readonly IGenericExceptionHandler handler;

    public ErrorController(IGenericExceptionHandler handler)
    {
      this.handler = handler;
    }

    [Route("error")]
    public ActionResult<ProblemDetails> Error() => handler.Handle(GrabError());

    // IExceptionHandlerPathFeature
    // This feature gets added to the HttpContext by Microsoft's error handling 
    // middleware before calling the error route so it should always be present
    // hence the ! null-forgiving operator
    private Exception GrabError() => HttpContext.Features.Get<IExceptionHandlerPathFeature>()!.Error;
  }
}
