using System.ComponentModel.DataAnnotations;

namespace ObjectValidator;

public class Cat:IValidatable
{
    [Required]
    [StringLength(20,MinimumLength = 2)]
    public string Name { get; set; }
    
    [Range(1,20)]
    public int Age { get; set; }
    
    [StringLength(10)]
    [Color("Red")]
    public string Color { get; set; }


    public IDictionary<string, List<string>> Validate()
    {
        var result = new Dictionary<string, List<string>>();
        if (this.Age < 2 && this.Name != "Unknown")
        {
            result[this.GetType().Name] = new List<string> {"This cat is to young to have a name."};
        }

        return result;
    }
}