using Newtonsoft.Json;
using Quokka.Public.Tools;
using QuokkaIntegrationTests;
using System;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfHost
{
    class Config
    {
        public string Key;
        public string Port;
    }

    class ReadPayload
    {
        public string value;
    }

    class Program
    {
        static Config Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(PathTools.ProjectPath, "config.json")));
        static HttpClient KVStoreClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("kvstoreio_api_key", Config.Key);
            return client;
        }

        static void Create()
        {
            using (var client = KVStoreClient())
            {
                var payoad = new { collection = "fpgaconf" };
                client.PostAsync("https://api.kvstore.io/collections", new StringContent(JsonConvert.SerializeObject(payoad), Encoding.UTF8, "application/json")).Wait();
            }
        }

        static bool Check()
        {
            using (var client = KVStoreClient())
            {
                var data = client.GetAsync("https://api.kvstore.io/collections/fpgaconf/items/trigger").Result.Content.ReadAsStringAsync().Result;
                var payload = JsonConvert.DeserializeObject<ReadPayload>(data);
                return payload.value == "true";
            }
        }

        static void Set()
        {
            Console.Write($"Set ...");
            using (var client = KVStoreClient())
            {
                client.PutAsync("https://api.kvstore.io/collections/fpgaconf/items/trigger", new StringContent("true")).Wait();
            }
            Console.WriteLine();
        }

        static void Reset()
        {
            Console.Write("Reset ...");
            using (var client = KVStoreClient())
            {
                client.PutAsync("https://api.kvstore.io/collections/fpgaconf/items/trigger", new StringContent("false")).Wait();
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            try
            {
                using (var port = new QuokkaPort(Config.Port, 115200))
                {
                    using (var subscription = Observable
                        .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
                        .ObserveOn(Scheduler.Default)
                        .Subscribe(_ =>
                        {
                            if (Check())
                            {
                                port.Write(1);
                                Reset();
                            }
                        }))
                    {
                        Console.WriteLine("Press space to send to board, esc to exit");
                        while (true)
                        {
                            switch (Console.ReadKey(true).Key)
                            {
                                case ConsoleKey.Escape:
                                    return;
                                case ConsoleKey.Spacebar:
                                    Set();
                                    break;
                                case ConsoleKey.Enter:
                                    Create();
                                    break;
                                case ConsoleKey.C:
                                    Console.WriteLine($"Value is {Check()}");
                                    break;
                                case ConsoleKey.R:
                                    Reset();
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }
    }
}
