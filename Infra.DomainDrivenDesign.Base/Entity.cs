using System.Collections.Generic;

namespace Infra.DomainDrivenDesign.Base
{
    public abstract class Entity<TId> : IEntity<TId>
        where TId : IEntityId
    {
        public TId Id { get; protected set; }

        public override bool Equals(
            object obj)
        {
            if (ReferenceEquals(
                null,
                obj)) return false;
            if (ReferenceEquals(
                this,
                obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return EqualityComparer<TId>.Default.Equals(
                Id,
                ((Entity<TId>) obj).Id);
        }

        public static bool operator ==(
            Entity<TId> left,
            Entity<TId> right)
        {
            if (ReferenceEquals(
                left,
                null))
            {
                return ReferenceEquals(
                    right,
                    null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(
            Entity<TId> left,
            Entity<TId> right) => !(left == right);

        public override int GetHashCode()
        {
            if (Id.Equals(default(TId)))
            {
                return base.GetHashCode();
            }

            return GetType()
                .GetHashCode() ^ Id.GetHashCode();
        }
    }
}
