using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;



internal class HistoricalPrice
{
     [JsonPropertyName("close")]
     public double Close { get; set; }
}

internal class HistoricalStock
{
     [JsonPropertyName("regularMarketPrice")]
     public double RegularMarketPrice { get; set; }

     [JsonPropertyName("historicalDataPrice")]
     public List<HistoricalPrice> HistoricalDataPrice { get; set; }
}

internal class Stock
{
     [JsonPropertyName("results")]
     public List<HistoricalStock> Results { get; set; }
}  