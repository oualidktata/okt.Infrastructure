using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infra.ExceptionHandling.Extensions
{
  public class CustomMiddleware
  {
    private readonly RequestDelegate next;

    public CustomMiddleware(RequestDelegate next)
    {
      this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await next(context);
      }
      catch(Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
  }
}
