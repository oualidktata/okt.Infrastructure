using Infra.Common.Models;

namespace Infra.DomainDrivenDesign.Base
{
    public abstract class AggregateRoot : Entity, IRepoQueryable
    {
        protected AggregateRoot() {}
    }
}
