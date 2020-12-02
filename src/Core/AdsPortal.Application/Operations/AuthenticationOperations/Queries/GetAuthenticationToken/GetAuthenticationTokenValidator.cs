﻿namespace AdsPortal.Application.Operations.AuthenticationOperations.Queries.GetValidToken
{
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Common;
    using AdsPortal.Domain.Entities;
    using Application.Constants;
    using FluentValidation;

    public class GetAuthenticationTokenValidator : AbstractValidator<GetAuthenticationTokenValidator.Model>
    {
        public GetAuthenticationTokenValidator(IUserManagerService _userManager)
        {
            RuleFor(x => x.Data.Email).NotEmpty()
                                      .WithMessage(ValidationMessages.Email.IsEmpty);
            RuleFor(x => x.Data.Email).EmailAddress()
                                      .WithMessage(ValidationMessages.Email.HasWrongFormat);

            RuleFor(x => x.Data.Password).NotEmpty()
                                         .WithMessage(ValidationMessages.Password.IsEmpty);
            RuleFor(x => x.Data.Password).MinimumLength(GlobalAppConfig.MIN_PASSWORD_LENGTH)
                                         .WithMessage(string.Format(ValidationMessages.Password.IsTooShort, GlobalAppConfig.MIN_PASSWORD_LENGTH));
            RuleFor(x => x.Data.Password).MaximumLength(GlobalAppConfig.MAX_PASSWORD_LENGTH)
                                         .WithMessage(string.Format(ValidationMessages.Password.IsTooLong, GlobalAppConfig.MAX_PASSWORD_LENGTH));

            //TODO: this should be removed and checked in handler as it is a comples operation
            RuleFor(x => x.User).MustAsync(async (request, val, token) =>
            {
                if (val == null)
                    return false;

                return await _userManager.ValidatePassword(val, request.Data.Password ?? string.Empty);
            }).WithMessage(ValidationMessages.Auth.EmailOrPasswordIsIncorrect);
        }

        public class Model
        {
            public GetAuthenticationTokenQuery Data { get; init; }
            public User User { get; init; }

            public Model(GetAuthenticationTokenQuery data, User user)
            {
                Data = data;
                User = user;
            }
        }
    }
}
