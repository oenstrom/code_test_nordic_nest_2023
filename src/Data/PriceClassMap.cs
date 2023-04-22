using System.Globalization;
using CsvHelper.Configuration;
using src.Models;

namespace src.Data; 

public sealed class PriceClassMap : ClassMap<Price> {
  public PriceClassMap() {
    AutoMap(CultureInfo.InvariantCulture);
    Map(m => m.ValidUntil).TypeConverter<NullableDateTimeConverter>();
  }
}