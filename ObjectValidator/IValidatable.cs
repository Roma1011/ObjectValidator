namespace ObjectValidator;

public interface IValidatable
{
    IDictionary<string, List<string>> Validate();
}