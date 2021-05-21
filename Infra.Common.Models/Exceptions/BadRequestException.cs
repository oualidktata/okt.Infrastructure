using System;

namespace Infra.Common.Models.Exceptions
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException() : base()
        {
        }

        public BadRequestException(
            string message) : base(message)
        {
        }

        public BadRequestException(
            string message,
            Exception ex) : base(
            message,
            ex)
        {
        }
    }
}
