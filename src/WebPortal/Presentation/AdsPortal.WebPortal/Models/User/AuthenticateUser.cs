﻿namespace AdsPortal.WebPortal.Models.User
{
    using System;
    using MagicModels.Attributes;
    using MagicModels.Components.Defaults.PropertyRenderers;
    using MagicOperations.Attributes;
    using Newtonsoft.Json;

    [OperationGroup(OperationGroups.User)]
    [LoginOperation(ResponseType = typeof(AuthenticateUserResponse), DisplayName = "Login user", Action = "auth")]
    public class AuthenticateUser
    {
        [JsonIgnore]
        [RenderableProperty(Mode = PropertyMode.Read)]
        public string? TestProp { get; set; } = "admin@adsportal.com // Pass123$";

        [RenderableProperty(DisplayName = "e-mail")]
        public string? Email { get; set; }

        [RenderableProperty(Renderer = typeof(PasswordRenderer))]
        public string? Password { get; set; }
    }

    [RenderableClass]
    public class AuthenticateUserResponse
    {
        public string Token { get; init; } = default!;
        public TimeSpan Lease { get; init; }
        public DateTime ValidTo { get; init; }
    }
}
