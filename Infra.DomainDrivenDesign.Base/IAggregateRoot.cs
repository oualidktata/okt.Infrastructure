namespace Infra.DomainDrivenDesign.Base
{
    public interface IAggregateRoot<out TId> : IEntity<TId>
        where TId : IEntityId
    {
    }
}
