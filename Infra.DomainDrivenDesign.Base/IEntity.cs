namespace Infra.DomainDrivenDesign.Base
{
    public interface IEntity<out TId> where TId : IEntityId
    {
        TId Id { get; }
    }
}
