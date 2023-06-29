using System;
using System.ComponentModel.DataAnnotations;
namespace VisusCore.AidStack.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public sealed class RequiredIfAttribute<TValue> : ValidationAttribute
{
    public TValue PropertyValue { get; set; }

    public string PropertyName { get; set; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var model = validationContext.ObjectInstance;
        if (model == null)
        {
            return ValidationResult.Success;
        }

        var currentValue = model.GetType()
            .GetProperty(PropertyName)
            ?.GetValue(model, index: null);
        if (currentValue is TValue currentPropertyValue
            && currentPropertyValue.Equals(PropertyValue)
            && (value == null
                || (value is string stringValue
                    && string.IsNullOrEmpty(stringValue))))
        {
            var propertyInfo = validationContext.ObjectType.GetProperty(validationContext.MemberName);

            return new ValidationResult(
                    $"{propertyInfo.Name} is required for the current {PropertyName} value {currentValue}");
        }

        return ValidationResult.Success;
    }
}
