using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceController : ControllerBase {
  private readonly ApplicationDbContext _context;

  public PriceController(ApplicationDbContext context) {
    _context = context;
  }

  // GET: /Price
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Price>>> GetPrice() {
    // return NotFound();
    // if (_context.Price == null) {
    //   return NotFound();
    // }

    return await _context.Price.ToListAsync();
  }
  
  /// <summary>Get prices by SKU (CatalogEntryCode).</summary>
  /// <param name="sku">The SKU (CatalogEntryCode)</param>
  /// <returns>List of prices.</returns>
  /// <remarks>Path: /Price/:sku</remarks>
  [HttpGet("{sku}")]
  public async Task<ActionResult<IEnumerable<Price>>> GetPrice(string sku) {
    var prices = await _context.Price.Where(p => p.CatalogEntryCode == sku).ToListAsync();

    return prices;
  }

  private bool PriceExists(int id) {
    return (_context.Price?.Any(e => e.PriceValueId == id)).GetValueOrDefault();
  }
}