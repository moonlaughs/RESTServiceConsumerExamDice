using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RESTServiceConsumerExamDice
{
    class Program
    {
        static readonly HttpClient Client = new HttpClient();
        static readonly DiceRoll NewDice = new DiceRoll();
        private static string _uri = "http://localhost:54109/api/dice";

        static void Main(string[] args)
        {
            try
            {
                RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                Console.ReadKey();
            }
        }

        static async Task RunAsync()
        {
            Console.WriteLine("You are now connected to the: " + _uri);

            Client.BaseAddress = new Uri(_uri);

            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine("Enter: get/byPerson/stop");
            string httpVerb = Console.ReadLine();

            while (!string.Equals(httpVerb, "stop", StringComparison.Ordinal))
            {
                if (httpVerb != null)
                {
                    IList<DiceRoll> dicerolls;
                    switch (httpVerb.ToUpper())
                    {
                        case "GET":
                            dicerolls = await GetDiceAsync();
                            ShowDice(dicerolls);
                            break;
                        case "BYPERSON":
                            Console.WriteLine("inseart the name:");
                            string name = Console.ReadLine();
                            var itemByName = await GetDiceByNameAsync(name); //GetItemByName(name);
                            ShowDice(itemByName);
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine("Enter: get/byPerson/stop");
                httpVerb = Console.ReadLine();
            }
            Console.WriteLine("Program ends! Bye Bye... Press Enter to end the program");
            Console.ReadLine();
        }


        //static async Task<DiceRoll> GetItemByName(string name)
        //{
        //    string newUri = _uri + "/byPerson/" + name;
        //    HttpResponseMessage response = await Client.GetAsync(newUri);
        //    if (response.StatusCode == HttpStatusCode.NotFound)
        //    {
        //        throw new Exception("Dice not found. Try another name");
        //    }
        //    response.EnsureSuccessStatusCode();
        //    string str = await response.Content.ReadAsStringAsync();
        //    var item = JsonConvert.DeserializeObject<DiceRoll>(str);
        //    return item;
        //}
                 
        //static void ShowDiceByName(DiceRoll itemByName)
        //{
        //    Console.WriteLine($"ID:{itemByName.Id}, Name: {itemByName.Name}, Number: {itemByName.Number}, Guess: {itemByName.Guess}, Result: {itemByName.Result}");
        //}


        static async Task<IList<DiceRoll>> GetDiceByNameAsync(string name)
        {
            string newUri = _uri + "/byPerson/" + name;
            HttpResponseMessage response = await Client.GetAsync(newUri);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Dice not found. Try another name");
            }
            response.EnsureSuccessStatusCode();
            string str = await response.Content.ReadAsStringAsync();
            IList<DiceRoll> dList = JsonConvert.DeserializeObject<IList<DiceRoll>>(str);
            return dList;
        }

        static void ShowDice(IList<DiceRoll> dicerolls)
        {
            foreach (var item in dicerolls)
            {
                Console.WriteLine($"ID:{item.Id}, Name: {item.Name}, Number: {item.Number}, Guess: {item.Guess}, Result: {item.Result}");
            }
            Console.WriteLine("======================");
        }

        static async Task<IList<DiceRoll>> GetDiceAsync()
        {
            string content = await Client.GetStringAsync(_uri);
            IList<DiceRoll> dList = JsonConvert.DeserializeObject<IList<DiceRoll>>(content);
            return dList;
        }
    }
}
