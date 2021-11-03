using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Infra.ExceptionHandling.Controllers
{
  public class GenericWebExceptionHandler : IGenericExceptionHandler
  {
    private readonly IServiceProvider serviceProvider;
    private readonly bool showDetails;

    public GenericWebExceptionHandler(IServiceProvider serviceProvider, IWebHostEnvironment env)
    {
      this.serviceProvider = serviceProvider;
      showDetails = env.IsDevelopment();
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
        return HandleOtherExceptions(exception);
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
      return new DefaultExceptionHandler().Handle(exception, showDetails);
    }
  }
}
