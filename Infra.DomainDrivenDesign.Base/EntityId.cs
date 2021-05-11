using System;
using System.Collections.Generic;

namespace Infra.DomainDrivenDesign.Base
{
    public abstract class EntityId : ValueObject, IEntityId
    {
        private Guid _guid;

        protected EntityId()
        {
            _guid = Guid.NewGuid();  
        }

        protected EntityId(string id) {
            _guid = Guid.Parse(id);
        }

        public override string ToString()
        {
            return _guid.ToString();
        }

        protected override IEnumerable<object> GetEqualityValues()
        {
            yield return ToString();
        }
    }
}
