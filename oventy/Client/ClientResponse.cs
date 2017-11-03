using System;
namespace oventy
{
    public class ClientResponse<T> : ClientResponse
    {
        public ClientResponse()
        {
        }

        public ClientResponse(
            T value,
            ErrorType? error = null) : base(error)
        {
            Value = value;
        }

        public T Value { get; set; }
    }

    /// <summary>
    ///     Represents the result of a web service call.
    /// </summary>
    public class ClientResponse
    {
        public ClientResponse(ErrorType? error)
        {
            ErrorCode = error;
        }

        public ClientResponse()
        {
        }

        public bool Success => ErrorCode == null;

        public ErrorType? ErrorCode { get; set; }
    }
}
