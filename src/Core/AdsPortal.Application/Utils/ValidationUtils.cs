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

        public static async Task<ValidationResult> ValidateAsync<TValidator, TModel>(TModel data, CancellationToken cancellationToken = default)
            where TValidator : class, IValidator<TModel>, new()
            where TModel : class
        {
            return await new TValidator().ValidateAsync(data, cancellationToken: cancellationToken);
        }
    }
}
