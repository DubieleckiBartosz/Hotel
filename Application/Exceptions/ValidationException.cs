using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class ValidationException:Exception
    {
        public List<string> Errors { get; set; }
        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new ();
        }
        public ValidationException(IEnumerable<ValidationFailure> failures):this()
        {
            foreach (var item in failures)
            {
                Errors.Add(item.ErrorMessage);
            }
        }
    }
}
