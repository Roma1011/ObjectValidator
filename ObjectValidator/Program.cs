namespace ObjectValidator;

class Program
{
    public static void Main(string[] args)
    {
        var cat = new Cat
        {
            Name = "Roma",
            Age = 2,
            Color = "Red"
        };

        var validator = new ObjectValidator();

        var result = validator.Validate(cat);
        
        Console.WriteLine(result.IsValid ? "InValid":"Valid");

        foreach (var erro in result.Erros)
        {
            Console.WriteLine(erro.Key);

            foreach (var errorMessage in erro.Value)
            {
                Console.WriteLine(errorMessage);
            }
        }
    }
}