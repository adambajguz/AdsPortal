﻿namespace AdsPortal.Application.Exceptions
{
    using System;

    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base()
        {

        }

        public ForbiddenException(string message) : base(message)
        {

        }

        public ForbiddenException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
