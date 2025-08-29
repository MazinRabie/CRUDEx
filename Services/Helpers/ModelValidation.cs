using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    public class ModelValidation
    {
        public static void ValidateModel<T>(T obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();
            var IsValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
            if (!IsValid) throw new ArgumentException(string.Join("\n", validationResults.Select(x => x.ErrorMessage)));
        }
    }
}
