using System;
using System.Collections.Generic;

namespace Infra.DomainDrivenDesign.Base
{
    public abstract class EntityId : ValueObject,
        IEntityId
    {
        private Guid _guid;

        protected EntityId()
        {
            _guid = Guid.NewGuid();
        }

        protected EntityId(
            string id)
        {
            _guid = Guid.Parse(id);
        }

        protected EntityId(
            Guid guid)
        {
            _guid = guid;
        }

        public override string ToString()
        {
            return _guid.ToString();
        }

        public Guid Key
        {
            get { return _guid; }
            set { _guid = value; }
        }

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return ToString();
        }
    }
}
