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

  // GET: /Price/5
  [HttpGet("{id}")]
  public async Task<ActionResult<Price>> GetPrice(int id) {
    if (_context.Price == null) {
      return NotFound();
    }

    var price = await _context.Price.FindAsync(id);

    if (price == null) {
      return NotFound();
    }

    return price;
  }

  private bool PriceExists(int id) {
    return (_context.Price?.Any(e => e.PriceValueId == id)).GetValueOrDefault();
  }
}