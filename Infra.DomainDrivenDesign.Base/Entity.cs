using System;
using System.ComponentModel.DataAnnotations;
using Infra.Common.Models;

namespace Infra.DomainDrivenDesign.Base
{
    public abstract class Entity : BaseEntity, IEntity
    {
        [Key]
        public Guid Id { get; protected set; }

        protected Entity()
        {
            
        }

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

            return Id == ((Entity) obj).Id;
        }

        public static bool operator ==(
            Entity left,
            Entity right)
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
            Entity left,
            Entity right) => !(left == right);

        public override int GetHashCode()
        {
            if (Id.Equals(default(Guid)))
            {
                return base.GetHashCode();
            }

            return GetType()
                .GetHashCode() ^ Id.GetHashCode();
        }
    }
}
