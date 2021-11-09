using System;

namespace Infra.ExceptionHandling
{
  public abstract class DomainException : Exception
  {
    protected DomainException(string? message) : base(message)
    {
    }
  }
}
