using System;

namespace Infra.Common.Models.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException() : this(String.Empty) { }

        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, string value ) : base(message) { }

        public ApplicationException(string message, Exception ex) : base(message, ex) { }
    }
}
