using System;

namespace Infra.DomainDrivenDesign.Base
{
    public interface IDomainEvent
    {
        DateTime CreationDate { get; set; }
    }
}
