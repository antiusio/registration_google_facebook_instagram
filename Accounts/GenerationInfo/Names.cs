using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Accounts.Data;

namespace Accounts.GenerationInfo
{
    public class Names
    {
        public class Info
        {
            public string FirstName;
            public string LastName;
        }
        HttpClient httpClient;
        //private CookieContainer cookieContainer;

        //Origin: http://xn--90aihhxfgb.xn--p1ai
        string host;
        public Names(string host = "http://xn--90aihhxfgb.xn--p1ai/")
        {
            this.host = host;

            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(host);
        }
        public Info GetFirstLastNameInfo(Sex sex=Sex.Female,/* string gender = "woman"man,*/ string type = "namefamily")
        {
            string gender = "";
            if (sex == Sex.Female)
                gender = "woman";
            if (sex == Sex.Male)
                gender = "man";
            var postData = new FormUrlEncodedContent(new[]
                        {
                new KeyValuePair<string, string>("type",type),
                new KeyValuePair<string, string>("gender",gender)
                });
            string s = Post(postData);

            HtmlDocument doc = null;

            if (s != null)
            {
                doc = new HtmlDocument();
                doc.LoadHtml(s);
            }
            if (doc != null)
            {
                string firstLastName = doc.DocumentNode.InnerText.Trim();
                firstLastName = firstLastName.Replace("\r\n", "");
                firstLastName = firstLastName.Replace("" + '\ufeff', "");
                string[] mass = firstLastName.Split(new char[] { ' ' });
                Info rezInfo = new Info() { FirstName = mass[0], LastName = mass[1] };
                return rezInfo;
            }
            return null;

        }
        private string Post(FormUrlEncodedContent postData, string url = "random_name.php")
        {
            string content = null;
            try
            {
                HttpResponseMessage httpResponseMessage;
                httpResponseMessage = httpClient.PostAsync(url, postData).GetAwaiter().GetResult();
                httpResponseMessage.EnsureSuccessStatusCode();

                var byteArray = httpResponseMessage.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
                var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                return responseString;
                //content = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            catch (TimeoutException e)
            {
                return null;
                //throw new PostGetException("Timeout exception", CauseException.Timeout);
            }
            catch (Exception e)
            {
                return null;
                //throw new PostGetException("Server exception", CauseException.ServerException);
            }
        }
    }
}
