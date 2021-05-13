using System;

namespace Infra.DomainDrivenDesign.Base
{
    public interface IEntityId
    {
        string ToString();
        Guid Key { get; set; }
    }
}
