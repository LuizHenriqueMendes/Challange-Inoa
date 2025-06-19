using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

class Stock
{
     static async Task Main(string[] args)
     {
          if (args.Length < 3)
          {
               Console.WriteLine("Por favor, forneça os parâmetros corretamente.");
               return;
          }

          while (true)
          {
                // Passando a ação de acordo com o argumento
          string url = $"https://brapi.dev/api/quote/{args[0]}";
          string token = "6sw4VV4yxYYQfy3QZoqwCf";

          string smtpServer = "smtp.gmail.com";
          int smtpPort = 587; // Geralmente 587 para envio com STARTTLS ou 465 para SSL
          string smtpUser = "luiz.mendescastro@gmail.com";
          string smtpPassword = "teste ";
          string from = "luiz.mendescastro@gmail.com";
          string to = "noemicho14@gmail.com";

          EmailSender emailSender = new EmailSender(smtpServer, smtpPort, smtpUser, smtpPassword);

          // Declarando as variáveis para venda e compra
               double venda, compra;

          // Tentando converter os parâmetros para valores double
          if (!double.TryParse(args[1], out venda))
          {
               Console.WriteLine("Erro: valor de venda inválido.");
               return;
          }

          if (!double.TryParse(args[2], out compra))
          {
               Console.WriteLine("Erro: valor de compra inválido.");
               return;
          }

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

                         // Comparando stock.RegularMarketPrice com venda e compra
                         if (stock.RegularMarketPrice < venda)
                         {
                              string subject = $"Relatório {args[0]} - Venda";
                              string body = $"Atenção! O(a) {args[0]} está sendo cotado à {stock.RegularMarketPrice} neste exato momento! É recomendável que você venda e aproveite o lucro!.";
                              Console.WriteLine($"Ação: {stock.Symbol}, Preço Atual: {stock.RegularMarketPrice}, venda imediatamente!");
                              emailSender.SendEmail(from, to, subject, body);
                         }
                         else if (stock.RegularMarketPrice > compra)
                         {
                              string subject = $"Relatório {args[0]} - Compra";
                              string body = $"Atenção! O(a) {args[0]} está sendo cotado à {stock.RegularMarketPrice} neste exato momento! É recomendável que você compre e aproveite o desconto!.";
                              Console.WriteLine($"Ação: {stock.Symbol}, Preço Atual: {stock.RegularMarketPrice}, compre-o imediatamente!");
                              emailSender.SendEmail(from, to, subject, body);
                         }
                         else
                         {
                              Console.WriteLine($"Ação: {stock.Symbol}, Preço Atual: {stock.RegularMarketPrice}, segure!!");
                         }
                    }
               }
               catch (HttpRequestException e)
               {
                    Console.WriteLine($"Erro: {e.Message}");
               }
          }
     }
          }

}
