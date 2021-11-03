using System;
using Microsoft.AspNetCore.Mvc;

namespace Infra.ExceptionHandling
{
  public interface IGenericExceptionHandler
  {
    ProblemDetails Handle(Exception exception);
  }
}
