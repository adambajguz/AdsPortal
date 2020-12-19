namespace AdsPortal.Application.Utils
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;
    using FluentValidation.Results;

    public static class ValidationUtils
    {
        public static async Task ValidateAndThrowAsync<TValidator, TModel>(TModel data, CancellationToken cancellationToken = default)
            where TValidator : class, IValidator<TModel>, new()
            where TModel : class
        {
            await new TValidator().ValidateAndThrowAsync(data, cancellationToken: cancellationToken);
        }
    }
}
