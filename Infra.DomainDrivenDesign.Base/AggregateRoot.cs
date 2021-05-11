namespace Infra.DomainDrivenDesign.Base
{
    public abstract class AggregateRoot<TId> : Entity<TId> where TId : IEntityId
    {
        protected AggregateRoot() {}
    }
}
