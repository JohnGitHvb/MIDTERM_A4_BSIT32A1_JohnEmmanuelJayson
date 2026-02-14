using System.ComponentModel.DataAnnotations;

namespace RandomQuoteApi.Data;

public class Quote
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    public int CategoryId { get; set; }

    public Category? Category { get; set; }
}