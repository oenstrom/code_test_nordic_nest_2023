using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace src.Data; 

public class NullableDateTimeConverter : DateTimeConverter {
  public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData) {
    return string.Equals(text, "NULL", StringComparison.OrdinalIgnoreCase)
      ? null
      : base.ConvertFromString(text, row, memberMapData);
  }
}