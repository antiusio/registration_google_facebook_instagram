using Accounts.InterfaceAccs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRegistration.Services
{
    
    public class SmsRegApi
    {
        HttpClient httpClient;
        string apikey = null;
        Feedback acc;
        public SmsRegApi(Feedback acc, string apikey)
        {
            this.apikey = apikey;
            this.acc = acc;
            HttpClientHandler handler;
            //cookieContainer = new CookieContainer();
            //homePageHtml = new HtmlDocument();
            handler = new HttpClientHandler
            {
                //CookieContainer = cookieContainer,
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

            //httpClient.BaseAddress = new Uri(homePageUrl);
        }
        public Task<bool> GetNum() //true - номер получен false - нет, не получен
        {
            return Task<bool>.Run(() =>
            {
                string country = "ua";
                string service = "gmail";
                string appid = "";
                string response = "";
                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        response = Get("http://api.sms-reg.com/getNum.php?country=" + country + "&service=" + service + "&appid = " + appid + "&apikey=" + apikey).GetAwaiter().GetResult();
                        break;
                    }
                    catch (Exception e)
                    {
                        acc.StatusText = "Ошибка при получении номера " + i.ToString() + " еще одна попытка.";
                        Thread.Sleep(600);
                    }
                }
                if (response.Equals(""))
                    return false;
                var responseJson = JsonConvert.DeserializeObject<responseJson>(response);
                if (responseJson.response.Equals("1"))
                {
                    tzid = responseJson.tzid;
                    return true;
                }
                else
                {
                    if (responseJson.response.Equals("WARNING_WAIT15MIN"))
                    {
                        acc.StatusText = "Ошибка при получении номера " + responseJson.response + " окончено с ошибкой.";
                        return false;
                    }
                    acc.StatusText = "Ошибка при получении номера " + responseJson.response + " окончено с ошибкой.";
                    return false;
                }

            });
        }
        public Task<string> Get(string url)
        {
            return Task<string>.Run(() =>
            {
                return httpClient.GetStringAsync(url).GetAwaiter().GetResult();
            });
        }
        public Task<bool> setReady()
        {
            string response = "";
            return Task.Run<bool>(() =>
            {
                for (int i = 0; ; i++)
                {
                    try
                    {
                        response = Get("http://api.sms-reg.com/setReady.php?tzid=" + tzid + "&apikey=" + apikey).GetAwaiter().GetResult();
                        break;
                    }
                    catch (Exception e)
                    {
                        acc.StatusText = "Ошибка при отправке статуса готов " + i.ToString();
                    }
                }
                var responseJson = JsonConvert.DeserializeObject<responseJson>(response);

                if (responseJson.response.Equals("1"))
                {
                    return true;
                }
                else
                {
                    acc.StatusText = "Ошибка при установке статуса готов получить смс " + responseJson.response;
                    return false;
                }
                //return responseJson.response.Equals("1") ? true : false;

            });
        }
        public Task<State> getState()
        {
            return Task<State>.Run(() =>
            {
                State answerState = new State();
                string response = Get("http://api.sms-reg.com/getState.php?tzid=" + tzid + "&apikey=" + apikey).GetAwaiter().GetResult();
                var responseJson = JsonConvert.DeserializeObject<responseJson>(response);
                if (responseJson.response.Equals("WARNING_NO_NUMS"))
                {
                    acc.StatusText = "Ошибка, нету подходящих номеров, операция закончена";
                    return null;
                }

                answerState.number = responseJson.number;
                answerState.msg = responseJson.msg;
                return answerState;
            });

        }
        private string tzid;
        public Task<bool> setOperationOk()
        {
            string response = "";
            return Task.Run<bool>(() =>
            {
                for (int i = 0; ; i++)
                {
                    try
                    {
                        response = Get("http://api.sms-reg.com/setOperationOk.php?tzid=" + tzid + "&apikey=" + apikey).GetAwaiter().GetResult();
                        break;
                    }
                    catch (Exception e)
                    {
                        acc.StatusText = "Ошибка при отправке уведомления об успешн помолучении кода " + i.ToString();
                    }
                }
                var responseJson = JsonConvert.DeserializeObject<responseJson>(response);

                if (responseJson.response.Equals("1"))
                {
                    return true;
                }
                else
                {
                    acc.StatusText = "Ошибка при установке уведомления об успешном получении кода " + responseJson.response;
                    return false;
                }
            });

        }
        public class State
        {
            public string number { get; set; }
            public string msg { get; set; }
        }
        public class responseJson
        {
            public string response { get; set; }
            public string service { get; set; }
            public string number { get; set; }
            public string msg { get; set; }
            public string tzid { get; set; }
        }
    }
}
