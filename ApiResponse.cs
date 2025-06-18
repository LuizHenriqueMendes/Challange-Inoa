using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class ApiResponse
{
     [JsonPropertyName("results")]
     public List<StockInfo> Results { get; set; }

     [JsonPropertyName("requestedAt")]
     public DateTime RequestedAt { get; set; }

     [JsonPropertyName("took")]
     public string Took { get; set; }
}

public class StockInfo
{
     [JsonPropertyName("currency")]
     public string Currency { get; set; }

     [JsonPropertyName("marketCap")]
     public long MarketCap { get; set; }

     [JsonPropertyName("shortName")]
     public string ShortName { get; set; }

     [JsonPropertyName("longName")]
     public string LongName { get; set; }

     [JsonPropertyName("regularMarketChange")]
     public double RegularMarketChange { get; set; }

     [JsonPropertyName("regularMarketChangePercent")]
     public double RegularMarketChangePercent { get; set; }

     [JsonPropertyName("regularMarketTime")]
     public DateTime RegularMarketTime { get; set; }

     [JsonPropertyName("regularMarketPrice")]
     public double RegularMarketPrice { get; set; }

     [JsonPropertyName("regularMarketDayHigh")]
     public double RegularMarketDayHigh { get; set; }

     [JsonPropertyName("regularMarketDayRange")]
     public string RegularMarketDayRange { get; set; }

     [JsonPropertyName("regularMarketDayLow")]
     public double RegularMarketDayLow { get; set; }

     [JsonPropertyName("regularMarketVolume")]
     public long RegularMarketVolume { get; set; }

     [JsonPropertyName("regularMarketPreviousClose")]
     public double RegularMarketPreviousClose { get; set; }

     [JsonPropertyName("regularMarketOpen")]
     public double RegularMarketOpen { get; set; }

     [JsonPropertyName("fiftyTwoWeekRange")]
     public string FiftyTwoWeekRange { get; set; }

     [JsonPropertyName("fiftyTwoWeekLow")]
     public double FiftyTwoWeekLow { get; set; }

     [JsonPropertyName("fiftyTwoWeekHigh")]
     public double FiftyTwoWeekHigh { get; set; }

     [JsonPropertyName("symbol")]
     public string Symbol { get; set; }

     [JsonPropertyName("logourl")]
     public string LogoUrl { get; set; }

     [JsonPropertyName("priceEarnings")]
     public double PriceEarnings { get; set; }

     [JsonPropertyName("earningsPerShare")]
     public double EarningsPerShare { get; set; }
}
