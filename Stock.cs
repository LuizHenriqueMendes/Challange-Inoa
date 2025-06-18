using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

class Stock
{
     static async Task Main()
     {
          string url = "https://brapi.dev/api/quote/PETR4";
          string token = "6sw4VV4yxYYQfy3QZoqwCf";

     using (HttpClient client = new HttpClient())
     {
          client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

          try
          {
               HttpResponseMessage response = await client.GetAsync(url);
               response.EnsureSuccessStatusCode();

               string json = await response.Content.ReadAsStringAsync();

               var options = new JsonSerializerOptions
               {
                    PropertyNameCaseInsensitive = true
               };

               ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(json, options);

               if (apiResponse?.Results != null && apiResponse.Results.Count > 0)
               {
                    var stock = apiResponse.Results[0];
                    Console.WriteLine($"Ação: {stock.Symbol}, Preço Atual: {stock.RegularMarketPrice}");
               }
          }
          catch (HttpRequestException e)
          {
               Console.WriteLine($"Erro: {e.Message}");
          }
     }
}
}
