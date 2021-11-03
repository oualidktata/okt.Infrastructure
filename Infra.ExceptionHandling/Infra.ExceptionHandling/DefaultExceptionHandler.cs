using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infra.ExceptionHandling
{
  public class DefaultExceptionHandler
  {
    public ProblemDetails Handle(Exception exception, bool showDetails)
    {
      return new ProblemDetails
      {
        Title = showDetails ? exception.Message : "An error has occured",
        Status = StatusCodes.Status500InternalServerError,
        Detail = showDetails ? exception.StackTrace : string.Empty,
      };
    }
  }
}
