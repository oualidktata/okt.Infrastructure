using System;
using AutoMapper;
using Infra.MediatR.Events;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Infra.Api
{
  [ApiController]
  [Route("api/[controller]")]
  public class PwcBaseController : ControllerBase
  {
    protected IMediator Mediator { get; }

    protected IMapper Mapper { get; }

    public PwcBaseController(IMediator mediator, IMapper mapper)
    {
      Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      Mapper = mapper;
      DomainEvents.Mediator = () => mediator;
    }
  }
}
