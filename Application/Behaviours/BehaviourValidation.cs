using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours
{
    public class BehaviourValidation<T1, T2> : IPipelineBehavior<T1, T2>
    {
        private readonly IEnumerable<IValidator<T1>> _validators;

        public BehaviourValidation(IEnumerable<IValidator<T1>> validators)
        {
            _validators = validators;
        }
        public async Task<T2> Handle(T1 request, CancellationToken cancellationToken, RequestHandlerDelegate<T2> next)
        {
            var context = new ValidationContext<T1>(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count != 0)
            {
                throw new Exceptions.ValidationException(failures);
            }

            return await next();
        }
    }
}
