using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomQuoteApi.Data;
using RandomQuoteApi.Models;

namespace RandomQuoteApi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuotesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuotes([FromQuery] string? category)
    {

        var query = _context.Quotes.Include(q => q.Category).AsQueryable();

        if (!string.IsNullOrEmpty(category) && category != "all")
        {
            query = query.Where(q => q.Category!.Name == category);
        }

        var quotes = await query.ToListAsync();

        var dtos = quotes.Select(q => new QuoteDto
        {
            Id = q.Id,
            Text = q.Text,
            Author = q.Author,
            Category = q.Category!.Name 
        });

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<QuoteDto>> CreateQuote(CreateQuoteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Text) || string.IsNullOrWhiteSpace(dto.Category))
        {
            return BadRequest("Text and Category are required.");
        }

        var categoryName = dto.Category.Trim().ToLower();

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == categoryName);

        if (category == null)
        {
            return BadRequest($"Category '{dto.Category}' does not exist. Please use: sweet, funny, dark, sarcastic");
        }

        var quote = new Quote
        {
            Text = dto.Text,
            Author = dto.Author,
            Category = category 
        };

        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync();

        var responseDto = new QuoteDto
        {
            Id = quote.Id,
            Text = quote.Text,
            Author = quote.Author,
            Category = category.Name
        };

        return CreatedAtAction(nameof(GetQuotes), new { id = quote.Id }, responseDto);
    }
}

