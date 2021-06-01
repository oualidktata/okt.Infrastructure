using System.Threading;
using System.Threading.Tasks;
using Infra.MediatR.Logging;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Infra.MediatR.Caching
{
  public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachable
  {
    private readonly IMemoryCache cache;
    private readonly ILogger logger;

    public CachingBehavior(
      IMemoryCache cache,
      ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
      this.cache = cache;
      this.logger = logger;
    }

    public async Task<TResponse> Handle(
      TRequest request,
      CancellationToken cancellationToken,
      RequestHandlerDelegate<TResponse> next)
    {
      var requestName = request.GetType();
      logger.LogDebug(
        "{Request} response is cacheable",
        requestName);

      TResponse response;

      if (cache.TryGetValue(
        request.CacheKey,
        out response))
      {
        logger.LogInformation(
          "{Request} response was retrieved from cache",
          requestName);

        return response;
      }

      logger.LogInformation(
        "{Request} response is not in cache,request executed",
        requestName);
      response = await next();
      cache.Set(
        request.CacheKey,
        response);
      logger.LogInformation(
        "{Request} response is now cached",
        requestName);

      return response;
    }
  }
}
