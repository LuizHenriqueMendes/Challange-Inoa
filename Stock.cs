using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

class Stock
{
     static async Task Main(string[] args)
     {
          if (args.Length < 3)
          {
               Console.WriteLine("Por favor, forneça os parâmetros corretamente.");
               return;
          }

          dotenv.net.DotEnv.Load();

          while (true)
          {
               string smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
               string smtpPortEnv = Environment.GetEnvironmentVariable("SMTP_PORT");
               string smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
               string smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
               string from = Environment.GetEnvironmentVariable("FROM");
               string to = Environment.GetEnvironmentVariable("TO");

               // Calculando a data de ontem
               string lastWeek = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
               

               string ticker = args[0];
               string token = "6sw4VV4yxYYQfy3QZoqwCf"; // Insira seu token aqui

               EmailSender emailSender = new EmailSender(smtpServer, int.Parse(smtpPortEnv), smtpUser, smtpPassword);

               double venda, compra;

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

               string range = "1mo"; // Última semana
               string interval = "1d"; // Intervalo diário
               string urlToday = $"https://brapi.dev/api/quote/{ticker}?range={range}&interval={interval}";


               using (HttpClient client = new HttpClient())
               {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    try
                    {
                         HttpResponseMessage response = await client.GetAsync(urlToday);
                         response.EnsureSuccessStatusCode();

                         string json = await response.Content.ReadAsStringAsync();

                         // Verificando se a resposta começa com '<' (indicando HTML)
                         if (json.StartsWith("<"))
                         {
                              Console.WriteLine("Erro: a resposta da API não é um JSON válido. A resposta pode ser HTML.");
                              Console.WriteLine($"Resposta da API: {json.Substring(0, Math.Min(1000, json.Length))}"); // Exibe até 1000 caracteres da resposta
                              return;
                         }

                         var options = new JsonSerializerOptions
                         {
                              PropertyNameCaseInsensitive = true
                         };

                         ApiResponse apiResponse = JsonSerializer.Deserialize<ApiResponse>(json, options);

                         if (apiResponse?.Results != null && apiResponse.Results.Count > 0)
                         {
                              var stock = apiResponse.Results[0];

                              //Console.WriteLine(json);

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

               await Task.Delay(1000 * 60 * 2); // Aguarda 2 minutos
          }
     }
}
