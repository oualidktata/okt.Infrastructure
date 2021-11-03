using System;
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
        Status = 500,
        Detail = showDetails ? exception.StackTrace : string.Empty,
      };
    }
  }
}
