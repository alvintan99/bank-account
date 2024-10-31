﻿namespace AwesomeGICBank.Domain.Exceptions
{
    public abstract class DomainExceptionBase : Exception
    {
        protected DomainExceptionBase(string message) : base(message) { }
    }
}
