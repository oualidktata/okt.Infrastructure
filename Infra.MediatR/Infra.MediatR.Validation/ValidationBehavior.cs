using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infra.MediatR.Validation
{
  public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
  {
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger _logger;

    public ValidationBehavior(
      IEnumerable<IValidator<TRequest>> validators,
      ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
      _validators = validators;
      _logger = logger;
    }

    public async Task<TResponse> Handle(
      TRequest request,
      CancellationToken cancellationToken,
      RequestHandlerDelegate<TResponse> next)
    {
      var context = new ValidationContext<TRequest>(request);
      var failures = _validators
        .Select(x => x.Validate(context))
        .SelectMany(x => x.Errors)
        .Where(x => x != null)
        .ToList();

      if (failures.Any())
      {
        throw new ValidationException(failures);
      }

      _logger.LogInformation($"ValidationBehavior {request.ToString()}");

      return await next();
    }
  }
}
