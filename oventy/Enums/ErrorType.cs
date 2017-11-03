using System;
namespace oventy
{
    public enum ErrorType
    {
        NameInvalid,
        EmailInvalid,
        EmailAlreadyRegistered,
        EmailPasswordInvalid,
        PasswordInvalid,
        Failed,
        SessionTokenInvalid,
        Timeout
    }
}
