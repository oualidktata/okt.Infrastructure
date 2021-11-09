using Microsoft.AspNetCore.Mvc;

namespace Infra.ExceptionHandling
{
  public interface IDomainExceptionHandler
  {
    ProblemDetails Handle(DomainException exception, bool showDetails);
  }

  public abstract class DomainExceptionHandler<T> : IDomainExceptionHandler
    where T : DomainException
  {

    public ProblemDetails Handle(DomainException e, bool showDetails)
    {
      return HandleException((T)e, showDetails);
    }

    public abstract ProblemDetails HandleException(T exception, bool showDetails);
  }
}
