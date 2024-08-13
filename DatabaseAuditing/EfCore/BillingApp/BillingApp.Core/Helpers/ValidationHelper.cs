using System.ComponentModel.DataAnnotations;

namespace BillingApp.Core.Helpers;

public static class ValidationHelper
{
    public static (bool IsValid, List<string> ErrorMessages) Validate<T>(T obj)
    {
        var validationContext = new ValidationContext(obj);
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

        return (isValid, validationResults.Select(r => r.ErrorMessage).ToList());
    }
}