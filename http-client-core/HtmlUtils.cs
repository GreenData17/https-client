using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace http_client_core
{
    public class HtmlUtils
    {
        public static void PrintRespond(string[] respond)
        {
            Console.Clear();

            string full = StringArrayToString(respond.ToArray());
            string[] code = full.Split('\n');

            //int offset = 20;
            int offset = 0;
            foreach (var s in code)
            {
                if (code.Length <= 19) offset = 0;

                if (offset != 0)
                {
                    offset--;
                    continue;
                }

                Console.WriteLine(s);
            }

            Requests.AbortLoading();
        }

        private static string StringArrayToString(string[] array)
        {
            string result = "";

            foreach (var s in array)
            {
                result += s;
            }

            return result;
        }
    }
}
