namespace Infra.MediatR.Caching
{
  public interface ICachable
  {
    string CacheKey { get; }
  }
}
