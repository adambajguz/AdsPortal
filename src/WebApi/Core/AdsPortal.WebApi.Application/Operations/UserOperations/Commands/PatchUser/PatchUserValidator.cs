namespace AdsPortal.WebApi.Application.Operations.UserOperations.Commands.PatchUser
{
    using FluentValidation;

    //TODO: add patch user validator
    public sealed class PatchUserValidator : AbstractValidator<PatchUserCommand>
    {
        public PatchUserValidator()
        {
            //RuleFor(x => x.Data.Email).NotEmpty()
            //                          .WithMessage(ValidationMessages.Email.IsEmpty);
            //RuleFor(x => x.Data.Email).EmailAddress()
            //                          .WithMessage(ValidationMessages.Email.HasWrongFormat);

            //RuleFor(x => x.Data.Email).MustAsync(async (request, val, token) =>
            //{
            //    User? userResult = request.User;

            //    if (userResult.Email.Equals(val))
            //        return true;

            //    bool checkInUse = await uow.Users.IsEmailInUseAsync(val!);

            //    return !checkInUse;

            //}).WithMessage(ValidationMessages.Email.IsInUse);
        }
    }
}
