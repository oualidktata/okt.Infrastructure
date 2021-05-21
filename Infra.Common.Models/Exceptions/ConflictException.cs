using System;

namespace Infra.Common.Models.Exceptions
{
    public class ConflictException : ApplicationException
    {
        public ConflictException() : base()
        {
        }

        public ConflictException(
            string message) : base(message)
        {
        }

        public ConflictException(
            string message,
            Exception ex) : base(
            message,
            ex)
        {
        }
    }
}
