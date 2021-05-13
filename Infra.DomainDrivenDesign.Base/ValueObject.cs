using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Infra.DomainDrivenDesign.Base
{
    public abstract class ValueObject
    {
        private const int HighPrime = 684563;
        protected abstract IEnumerable<object> GetEqualityValues();
        
        [Key]
        public Guid Id { get; set; }

        public override int GetHashCode()
        {
            return GetEqualityValues()
                .Select(
                    (
                        x,
                        i) => (x != null
                        ? x.GetHashCode()
                        : 0) + (HighPrime * i))
                .Aggregate(
                    (
                        x,
                        y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return MemberwiseClone() as ValueObject;
        }

        public bool Equals(
            ValueObject obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            return GetHashCode() == obj.GetHashCode();
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

            return Equals((ValueObject) obj);
        }

        public static bool operator ==(
            ValueObject left,
            ValueObject right)
        {
            return left?.Equals(right) ?? ReferenceEquals(
                right,
                null);
        }

        public static bool operator !=(
            ValueObject left,
            ValueObject right) => !(left == right);
    }
}
