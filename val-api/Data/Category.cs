using System.ComponentModel.DataAnnotations;

namespace RandomQuoteApi.Data;

public class Category
{
    public int Id { get; set; }

    
    [Required]
    public string Name { get; set; } = string.Empty;


    public List<Quote> Quotes { get; set; } = new();
}