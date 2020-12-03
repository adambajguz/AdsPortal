﻿namespace AdsPortal.CLI.Services
{
    using System;
    using AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken;

    public sealed class AuthTokenHolder
    {
        public string? Token { get; private set; }
        public TimeSpan? Lease { get; private set; }
        public DateTime? ValidTo { get; private set; }

        public bool HasToken => !string.IsNullOrWhiteSpace(Token);

        public void Set(JwtTokenModel? model)
        {
            Token = model?.Token;
            Lease = model?.Lease;
            ValidTo = model?.ValidTo;
        }

        public void Clear()
        {
            Token = null;
            Lease = null;
            ValidTo = null;
        }
    }
}
