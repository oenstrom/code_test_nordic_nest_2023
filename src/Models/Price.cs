using System.ComponentModel.DataAnnotations;

namespace src.Models;

public class Price {
  [Key] public int PriceValueId { get; set; }
  public DateTime Created { get; set; }
  public DateTime Modified { get; set; }
  public string? CatalogEntryCode { get; set; }
  public string? MarketId { get; set; }
  public string? CurrencyCode { get; set; }
  public DateTime? ValidFrom { get; set; }
  public DateTime? ValidUntil { get; set; }
  public decimal UnitPrice { get; set; }
  // Set columntype to decimal(18,6) here instead of in ApplicationDbContext.cs
  
  public Price Clone() {
    return new Price {
      Created = Created,
      Modified = Modified,
      CatalogEntryCode = CatalogEntryCode,
      MarketId = MarketId,
      CurrencyCode = CurrencyCode,
      ValidFrom = ValidFrom,
      ValidUntil = ValidUntil,
      UnitPrice = UnitPrice
    };
  }
}