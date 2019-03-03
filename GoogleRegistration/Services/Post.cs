using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRegistration.Services
{
    public static class PostApi
    {
        static HttpClient httpClient;
        private static CookieContainer cookieContainer;
        static void createBrowser(string ip = null, int port = 0)
        {
            cookieContainer = new CookieContainer();
            HttpClientHandler handler;
            cookieContainer = new CookieContainer();
            //HtmlDocument myaccountDocument = new HtmlDocument();
            if (!(ip is null))
                handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    Proxy = new WebProxy("127.0.0.1", 8888),
                    PreAuthenticate = true,
                    UseDefaultCredentials = false,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
            else
                handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    //Proxy = new WebProxy("127.0.0.1", 8888),
                    PreAuthenticate = true,
                    UseDefaultCredentials = false,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("UserAgent", "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7,uk;q=0.6");
        }
        public static string Post(string url, FormUrlEncodedContent postData)
        {
            createBrowser();
            httpClient.BaseAddress = new Uri(url.Remove(url.IndexOf("registration/")));
            string content = null;
            try
            {
                HttpResponseMessage httpResponseMessage;
                
                httpResponseMessage = httpClient.PostAsync(new Uri(url).PathAndQuery, postData).GetAwaiter().GetResult();
                content = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (TimeoutException e)
            {
                //throw new PostGetException("Timeout exception", CauseException.Timeout);
            }
            catch (Exception e)
            {
                //throw new PostGetException("Server exception", CauseException.ServerException);
            }

            return content;
        }
    }
}
