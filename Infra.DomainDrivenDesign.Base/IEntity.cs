using System;

namespace Infra.DomainDrivenDesign.Base
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
