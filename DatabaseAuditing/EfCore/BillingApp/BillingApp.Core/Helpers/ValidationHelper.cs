using System.ComponentModel.DataAnnotations;
using BillingApp.Core.Common;

namespace BillingApp.Core.Helpers;

public static class ValidationHelper
{
    public static ResultType<T> Validate<T>(T obj)
    {
        var validationContext = new ValidationContext(obj);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        if (isValid)
        {
            return ResultType<T>.Success(obj);
        }
        else
        {
            var errors = validationResults.Select(r => r.ErrorMessage).ToList();
            return ResultType<T>.Failure(errors);
        }
    }
}