using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace http_client_core
{
    public class Requests
    {
        private static bool _abortAllTasks;

        /// <summary>
        /// Sends a GET request to a website and gets the html code.
        /// </summary>
        /// <param name="domainName">the website's domain name (like: www.google.com)</param>
        /// <param name="websitePath">the file you want to access (like: "index.html" or "cat.jpg")</param>
        /// <param name="protocolType">the protocol the website uses. (default = https)</param>
        /// <returns>Returns a string[] with the website's html code.</returns>
        public static async Task<string[]> GetHtml(string domainName, string websitePath = "/", string protocolType = "https")
        {
            List<string> FullRespond = new List<string>();

            TcpClient page = new TcpClient();
            page.Connect(domainName, 443);

            SslStream sslStream = new SslStream(
                page.GetStream(),
                false,
                new RemoteCertificateValidationCallback(ValidateServerCertificate),
                null
            );

            // The server name must match the name on the server certificate.
            try
            {
                await sslStream.AuthenticateAsClientAsync(domainName, null, SslProtocols.Tls12, false);
            }
            catch (AuthenticationException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                page.Close();
                Console.ForegroundColor = ConsoleColor.White;
                throw new ConnectionFailedException("SSL/TSL Authentication failed - Expected HTTPS.");
            }

            // Send request
            byte[] buffer = new byte[2048];
            int bytes;
            byte[] request = Encoding.UTF8.GetBytes(String.Format($"GET {websitePath}  HTTP/1.1\r\nHost: {domainName}\r\n\r\n"));
            sslStream.Write(request, 0, request.Length);
            sslStream.Flush();

            // Read response
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);
                string respond = Encoding.UTF8.GetString(buffer, 0, bytes);
                FullRespond.Add(respond);

                if (respond.Contains("</html>")) break;
                if (_abortAllTasks)
                {
                    throw new ConnectionFailedException("Loading Aborted by the browser!");
                }
            } while (true);

            return FullRespond.ToArray();
        }

        
        private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        public static void AbortLoading() => _abortAllTasks = true;
    }
}