using FluentValidation;
using MediatR;

namespace LibraryManagement.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Count > 0)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count > 0)
            {
                var errors = failures.Select(f => f.ErrorMessage).ToList();
                
                // Use reflection to create Result<T>.Failure or Result.Failure
                var responseType = typeof(TResponse);
                if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
                {
                    var resultType = responseType.GetGenericArguments()[0];
                    var failureMethod = typeof(Result<>).MakeGenericType(resultType)
                        .GetMethod(nameof(Result<object>.Failure), new[] { typeof(List<string>) });
                    return (TResponse)failureMethod!.Invoke(null, new object[] { errors })!;
                }
                else if (responseType == typeof(Result))
                {
                    return (TResponse)(object)Result.Failure(errors);
                }
            }
        }

        return await next();
    }
}