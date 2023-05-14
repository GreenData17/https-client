using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using http_client_core;

namespace http_client_console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task<string[]> task = Task.Run(SendRequest);

            string[] result = new string[]{};
            bool finishedLoading = false;

            while (!finishedLoading)
            {
                if (task.IsCompletedSuccessfully)
                {
                    result = task.Result;
                    finishedLoading = true;
                }else if (task.IsFaulted)
                {
                    finishedLoading = true;
                }
            }

            if (result.Length == 0)
            {
                Console.WriteLine("result is NULL");
                Console.Read();
            }

            HtmlUtils.PrintRespond(result);
            Console.Read();
        }

        public static async Task<string[]> SendRequest()
        {
            Task<string[]> task = Requests.GetHtml("www.amazon.com", "/-/de/dp/B073XMNQC2/ref=sr_1_5?keywords=programming+socks&qid=1666556043&qu=eyJxc2MiOiI0LjY5IiwicXNhIjoiNC4xMyIsInFzcCI6IjMuODYifQ%3D%3D&sr=8-5");
            string[] result;
            try
            {
                result = await task;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.Read();
                Console.ForegroundColor = ConsoleColor.White;
                return new string[]{};
            }
            return result;
        }
    }
}
