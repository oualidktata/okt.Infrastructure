using System;
using System.Runtime.Serialization;

namespace Infra.OAuth.Introspection
{
  public class JwtIntrospectionException : Exception
  {
    public JwtIntrospectionException()
    {
    }

    public JwtIntrospectionException(string? message)
      : base(message)
    {
    }

    public JwtIntrospectionException(string? message, Exception? innerException)
      : base(message, innerException)
    {
    }

    protected JwtIntrospectionException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    internal static JwtIntrospectionException UnsupportedAuthorizationSchemes(string[] supportedAuthorizationSchemes)
    {
      var customMessage = string.Join(", ", supportedAuthorizationSchemes);
      return new JwtIntrospectionException($"Authorization Scheme isn't supported. Should be one of {customMessage}");
    }

    internal static JwtIntrospectionException AuthorizationHeaderMissing()
    {
      return new JwtIntrospectionException("Authorization header isn't provided");
    }
  }
}
