using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infra.ExceptionHandling
{
  public class DefaultDomainExceptionHandler : IDomainExceptionHandler
  {
    public ProblemDetails Handle(DomainException exception, bool showDetails)
    {
      return new ProblemDetails
      {
        Title = exception.Message,
        Detail = showDetails ? exception.StackTrace : string.Empty,
        Status = StatusCodes.Status400BadRequest,
      };
    }
  }
}
