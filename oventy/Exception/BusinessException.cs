using System;
namespace oventy
{
    public class BusinessException : Exception
    {
        public BusinessException(ErrorType error)
        {
            Error = error;
        }

        public ErrorType Error { get; set; }
    }
}
