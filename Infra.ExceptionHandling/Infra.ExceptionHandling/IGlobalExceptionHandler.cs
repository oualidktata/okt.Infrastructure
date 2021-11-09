using System;
using Microsoft.AspNetCore.Mvc;

namespace Infra.ExceptionHandling
{
  public interface IGlobalExceptionHandler
  {
    ProblemDetails Handle(Exception exception);
  }
}
