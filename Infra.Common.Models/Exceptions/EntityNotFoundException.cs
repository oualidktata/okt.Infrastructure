using System;

namespace Infra.Common.Models.Exceptions
{
    public class EntityNotFoundException : ApplicationException
    {
        public EntityNotFoundException() : base()
        {
        }

        public EntityNotFoundException(
            string message) : base(message)
        {
        }

        public EntityNotFoundException(
            string message,
            Exception ex) : base(
            message,
            ex)
        {
        }
    }
}
