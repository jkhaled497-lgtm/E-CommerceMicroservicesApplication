using Buildingblocks.CQRS;
using Buildingblocks.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buildingblocks.Behaviour
{
    public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = 
                await Task.WhenAll(validators.Select(s => s.ValidateAsync(context, cancellationToken)));

            var failures = validationResults.SelectMany(s => s.Errors).Where(f => f != null).ToList();


            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

          return  await next();

        }
    }
}
