using System;
using Microsoft.AspNetCore.Mvc;

namespace Infra.ExceptionHandling
{
  public class GlobalExceptionHandler : IGlobalExceptionHandler
  {
    private readonly IServiceProvider serviceProvider;
    private readonly bool showDetails;

    public GlobalExceptionHandler(IServiceProvider serviceProvider, bool showDetails)
    {
      this.serviceProvider = serviceProvider;
      this.showDetails = showDetails;
    }

    public ProblemDetails Handle(Exception exception)
    {
      if (exception is DomainException ex)
      {
        return HandleDomainExceptions(ex);
      }
      else
      {
        return HandleOtherExceptions(exception);
      }
    }

    public ProblemDetails HandleDomainExceptions(DomainException exception)
    {
      var specificHandler = GetHandlerForExceptionType(exception);
      if (specificHandler == null)
      {
        return new DefaultDomainExceptionHandler().Handle(exception, showDetails);
      }

      return specificHandler.Handle(exception, showDetails);
    }

    private IDomainExceptionHandler? GetHandlerForExceptionType(DomainException exception)
    {
      var exceptionType = exception.GetType();
      var handlerType = typeof(DomainExceptionHandler<>).MakeGenericType(exceptionType);
      var handler = serviceProvider.GetService(handlerType);

      return handler as IDomainExceptionHandler;
    }

    public ProblemDetails HandleOtherExceptions(Exception exception)
    {
      return new ServerErrorExceptionHandler().Handle(exception, showDetails);
    }
  }
}
