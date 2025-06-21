using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

class MainProgram
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

               string ticker = args[0];
               double venda = double.Parse(args[1]);
               double compra = double.Parse(args[2]);

               string url = $"https://brapi.dev/api/quote/{ticker}?range=1mo&interval=1d";
               string token = "6sw4VV4yxYYQfy3QZoqwCf";

               EmailSender emailSender = new EmailSender(smtpServer, int.Parse(smtpPortEnv), smtpUser, smtpPassword);

               using (HttpClient client = new HttpClient())
               {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    try
                    {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();

                    if (json.StartsWith("<"))
                    {
                         Console.WriteLine("Erro: a resposta da API não é um JSON válido.");
                         Console.WriteLine($"Resposta da API: {json.Substring(0, Math.Min(1000, json.Length))}");
                         return;
                    }

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Stock apiResponse = JsonSerializer.Deserialize<Stock>(json, options);

                    if (apiResponse?.Results != null && apiResponse.Results.Count > 0)
                    {
                         var stock = apiResponse.Results[0];

                         var historical = stock.HistoricalDataPrice;
                              Console.WriteLine(historical);

                         if (historical != null && historical.Count > 0)
                         {
                              double max = historical.Max(x => x.Close); //Busca dentro do array o valor mais alto
                              double min = historical.Min(x => x.Close); //Busca dentro do array o valor mais baixo
                              double avg = historical.Average(x => x.Close); //Calcula dentro do array o valor médio

                              Console.WriteLine($"Máxima no período: {max}");
                              Console.WriteLine($"Mínima no período: {min}");
                              Console.WriteLine($"Média no período: {avg}");

                              // Verificação de venda ou compra
                              if (stock.RegularMarketPrice < venda)
                              {
                                   string subject = $"Relatório {ticker} - Venda";
                                   string body = $"O ativo {ticker} está sendo cotado a {stock.RegularMarketPrice}. Recomendação: VENDA.";
                                   Console.WriteLine(body);
                                   emailSender.SendEmail(from, to, subject, body);
                              }
                              else if (stock.RegularMarketPrice > compra)
                              {
                                   string subject = $"Relatório {ticker} - Compra";
                                   string body = $"O ativo {ticker} está sendo cotado a {stock.RegularMarketPrice}. Recomendação: COMPRA.";
                                   Console.WriteLine(body);
                                   emailSender.SendEmail(from, to, subject, body);
                              }
                              else
                              {
                                   Console.WriteLine($"Ação: {ticker}, Preço Atual: {stock.RegularMarketPrice}, mantenha posição.");
                              }
                         }
                         else
                         {
                              Console.WriteLine("Não há dados históricos disponíveis.");
                         }
                         }
                    }
                    catch (HttpRequestException e)
                    {
                         Console.WriteLine($"Erro: {e.Message}");
                    }
               }

               await Task.Delay(1000 * 60 * 2);
          }
     }
}
