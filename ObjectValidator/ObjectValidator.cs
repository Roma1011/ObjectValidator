using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ObjectValidator;

public class ObjectValidator
{
    private readonly static IDictionary<Type, List<PropertyAttributes>> Cache 
        = new Dictionary<Type, List<PropertyAttributes>>();
    public ValidationResult Validate(object obj)
    {
        if (obj is null)
        {
            var result = new ValidationResult();
            result.AddError("Object","Object is null.");
            return result;
        }

        
        var validationResult = new ValidationResult();
        
        this.ValidateValidatable(obj, validationResult);
        this.ValidateProperties(obj, validationResult);
        
        return validationResult;
    }

    private void ValidateValidatable(object obj, ValidationResult validationResult)
    {
        if (!(obj is IValidatable validatable))
        {
            return;
        }
        var additionalErrors = validatable.Validate();
        foreach (var error in additionalErrors)
        {
            foreach (var value in error.Value)
            {
                validationResult.AddError(error.Key,value);
            }
        }
    }

    private void ValidateProperties(object obj, ValidationResult validationResult)
    {
        var objectType=obj.GetType();
        this.CacheObjectProperties(objectType);
        var objectProperties = Cache[objectType];
        foreach (var property in objectProperties)
        {
            var atributes = property.Attributes;
            var propertyValue = property.Info;
            
            foreach (var attribute in atributes)
            {
                var isValid = attribute.IsValid(propertyValue);
                if (!isValid)
                {
                    var errorMessage = attribute.FormatErrorMessage(property.Name);
                    validationResult.AddError(property.Name,errorMessage);
                }
            }
        }
    }

    private void CacheObjectProperties(Type objectType)
    {
        if (Cache.ContainsKey(objectType)) 
            return;
        
        var typeProperties = objectType.GetProperties();
        Cache[objectType] = new List<PropertyAttributes>();
        foreach (var property in typeProperties)
        {
                var attributes=property.GetCustomAttributes<ValidationAttribute>();
                Cache[objectType].Add(new PropertyAttributes
                {
                    Name=property.Name ,
                    Info=property ,
                    Attributes=attributes
                });
        }
    }

    private class PropertyAttributes
    {
        public string Name { get; set; }
        public PropertyInfo Info { get; set; }
        public IEnumerable<ValidationAttribute> Attributes { get; set; }
    }
}

