namespace ObjectValidator;

public class ValidationResult
{
    public ValidationResult()=>
        this.Erros = new Dictionary<string, List<string>>();

    public bool IsValid => this.Erros.Any();
    
    public IDictionary<string,List<string>>Erros { get;}

    public void AddError(string errorName, string errorMessage)
    {
        if (!this.Erros.ContainsKey(errorName))
            this.Erros[errorName] = new List<string>();
        
        this.Erros[errorName].Add(errorMessage);
    }
}