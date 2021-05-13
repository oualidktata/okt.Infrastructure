using Infra.Common.Models;

namespace Infra.DomainDrivenDesign.Base
{
    public interface IAggregateRoot : IEntity, IRepoQueryable
    {
    }
}
