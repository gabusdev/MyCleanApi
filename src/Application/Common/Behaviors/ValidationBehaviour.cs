using Microsoft.Extensions.Localization;

namespace Application.Common.Behaviors;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly IStringLocalizer<ValidationBehaviour<TRequest, TResponse>> _localizer;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators, IStringLocalizer<ValidationBehaviour<TRequest, TResponse>> localozer)
    {
        _validators = validators;
        _localizer = localozer;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v =>
                    v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
                throw new FluentValidationException(_localizer["validation.errors"] ,failures);
        }
        return await next();
    }
}
