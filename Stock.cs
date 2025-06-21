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

               string url = $"https://brapi.dev/api/quote/{ticker}?range=3mo&interval=1d";
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

                    //Verifica se o Json veio como <html>
                    if (json.StartsWith("<"))
                    {
                         Console.WriteLine("Erro: a resposta da API não é um JSON válido.");
                         Console.WriteLine($"Resposta da API: {json.Substring(0, Math.Min(1000, json.Length))}");
                         return;
                    }

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Stock apiResponse = JsonSerializer.Deserialize<Stock>(json, options);


                    //Verifica se o a resposta está no formato correto
                    if (apiResponse?.Results != null && apiResponse.Results.Count > 0)
                    {
                         var stock = apiResponse.Results[0];

                         var historical = stock.HistoricalDataPrice;
                         //Console.WriteLine(historical);

                         if (historical != null && historical.Count > 0)
                         {
                              var historicalWithDate = historical.Select(x => new
                              {
                                   Date = DateTimeOffset.FromUnixTimeSeconds(x.Date).DateTime.Date,
                                   Close = x.Close
                              }).ToList();

                              DateTime today = DateTime.Now.Date;
                              DateTime sevenDaysAgo = today.AddDays(-7);
                              DateTime thirtyDaysAgo = today.AddDays(-30);

                              double max = historical.Max(x => x.Close); //Busca dentro do array o valor mais alto
                              double min = historical.Min(x => x.Close); //Busca dentro do array o valor mais baixo
                              double avg = Math.Round(historical.Average(x => x.Close), 2); //Calcula dentro do array o valor médio

                              // Console.WriteLine($"Máxima no período: {max}");
                              // Console.WriteLine($"Mínima no período: {min}");
                              // Console.WriteLine($"Média no período: {avg}");
                              
                              double max30Days = 0, min30Days = 0, avg30Days = 0;                              
                              var last30Days = historicalWithDate.Where(x => x.Date >= thirtyDaysAgo).ToList();
                              if (last30Days.Count > 0)
                              {
                                   max30Days = last30Days.Max(x => x.Close);
                                   min30Days = last30Days.Min(x => x.Close);
                                   avg30Days = Math.Round(last30Days.Average(x => x.Close),2);
                              }
                              else
                              {
                                   Console.WriteLine("Sem dados para os últimos 30 dias.");
                                   
                              }

                              double max7Days = 0, min7Days = 0, avg7Days = 0;
                              var last7Days = historicalWithDate.Where(x => x.Date >= sevenDaysAgo).ToList();
                              if (last7Days.Count > 0)
                              {                                   
                                   max7Days = last7Days.Max(x => x.Close);
                                   min7Days = last7Days.Min(x => x.Close);
                                   avg7Days = Math.Round(last7Days.Average(x => x.Close), 2);
                              }
                              else
                              {
                                   Console.WriteLine("Sem dados para os últimos 7 dias.");
                              }

                              // Verificação de venda ou compra
                              if (stock.RegularMarketPrice < venda)
                              {
                                   string subject = $"Relatório {ticker} - Venda";
                                   string parte1 = $"O ativo {ticker} está sendo cotado a {stock.RegularMarketPrice}. Recomendação: VENDA.\n";
                                   string parte2 = $"Caso queira analisar de maneira mais profunda observe os dados do {ticker} nos últimos 7 dias, 30 dias e 3 meses e \n";
                                   string parte3 = $"07 dias  => Valor mínimo: {min7Days} ; Valor máximo: {max7Days} ; Média do período: {avg7Days}\n";
                                   string parte4 = $"30 dias  => Valor mínimo: {min30Days} ; Valor máximo: {max30Days} ; Média do período: {avg30Days}\n";
                                   string parte5 = $"03 meses => Valor mínimo: {min} ; Valor máximo: {max} ; Média do período: {avg}\n";
                                   string body = $"{parte1} {parte2} {parte3} {parte4} {parte5}";
                                   emailSender.SendEmail(from, to, subject, body);
                              }
                              else if (stock.RegularMarketPrice > compra)
                              {
                                   string subject = $"Relatório {ticker} - Compra";
                                   string parte1 = $"O ativo {ticker} está sendo cotado a {stock.RegularMarketPrice}. Recomendação: VENDA.\n";
                                   string parte2 = $"Caso queira analisar de maneira mais profunda observe os dados do {ticker} nos últimos 7 dias, 30 dias e 3 meses: \n";
                                   string parte3 = $"07 dias    => Valor mínimo: {min7Days} ; Valor máximo: {max7Days} ; Média do período: {avg7Days}\n";
                                   string parte4 = $"30 dias    => Valor mínimo: {min30Days} ; Valor máximo: {max30Days} ; Média do período: {avg30Days}\n";
                                   string parte5 = $"03 meses => Valor mínimo: {min} ; Valor máximo: {max} ; Média do período: {avg}\n";
                                   string body = $"{parte1} {parte2} {parte3} {parte4} {parte5}";
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
               //Roda novamente a cada 5 minutos
               await Task.Delay(1000 * 60 * 5);
          }
     }
}
