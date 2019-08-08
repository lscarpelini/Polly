using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Default
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Retry().GetAwaiter().GetResult();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(new string('-', 50));
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Serviço intivo, Favor tentar novamente mais tarde." + ex.Message);
                Console.ReadLine();
            }

        }

        static async Task Retry()
        {
            //Contador
            int count = 1;

            //Politica de Execução
            var policy = Policy.Handle<Exception>(e => {
                Console.WriteLine("ERRO: " + e.Message);
                return true;
            }).WaitAndRetryAsync(4, i => TimeSpan.FromTicks(2));


            int ix = 50000;
            for (int i = 0; i < ix; i++)
            {
                //Execução
                await policy.ExecuteAsync(async () =>
                {
                    Console.WriteLine("Tentativa de Execução Numero: " + count);
                    count++;
                    await RequestSeller("RetryBilling", i);
                    //await Request("Retry");
                });
            }

            ////Execução
            //await policy.ExecuteAsync(async () =>
            //{
            //    Console.WriteLine("Tentativa de Execução Numero: " + count);
            //    count++;
            //    await RequestSeller("RetryBilling");
            //    //await Request("Retry");
            //});
        }

        static async Task Request(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            Console.WriteLine($"Request para o metodo { url }");
            var client = new HttpClient();
            var response = await client.GetAsync($"https://localhost:44317/api/values/{url}", cancellationToken);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<string[]>(json);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(new string('-', 50));
            Console.WriteLine();
        }

        static async Task RequestSeller(string url, int number, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {

                Console.WriteLine($"Request para o metodo { url }");
                var client = new HttpClient();
            
                string billingFile = JsonConvert.SerializeObject(new
                {
                    billingDate = "2019-06-19T13:52:21.170Z",
                    partinerId = 3519572,
                    cnpj = "51226936000176",
                    storeId = "567",
                    productId = 16,
                    quantity = number,
                    amount = 69.69
                });
                var content = new StringContent(billingFile, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"http://localhost:63211/v1/billing", content, cancellationToken);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                //var result = JsonConvert.DeserializeObject<string[]>(json);

                Console.WriteLine(json.ToString());

                Console.WriteLine(new string('-', 50));
                Console.WriteLine();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(new string('-', 50));
                Console.WriteLine();
            }

        }





    }
}
