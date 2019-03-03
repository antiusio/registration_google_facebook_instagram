using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServiceRegistration.PostGetApi
{
    public class RegistrationIua
    {
        private string mainUrl = "https://www.i.ua/";
        private static string RegisterLink = null;
        private HttpClient httpClient;
        private CookieContainer cookieContainer;
        public RegistrationIua(string ip=null, int port = 0, string host = "https://www.i.ua/")
        {
            HttpClientHandler handler;
            cookieContainer = new CookieContainer();
            //HtmlDocument myaccountDocument = new HtmlDocument();
            if(!(ip is null))
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

            httpClient.BaseAddress = new Uri(host);
        }
        private string Post(string url, FormUrlEncodedContent postData)
        {
            string content = null;
            try
            {
                HttpResponseMessage httpResponseMessage;
                httpResponseMessage = httpClient.PostAsync(url, postData).GetAwaiter().GetResult();
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
        private string Step1Registration(string googleKey)
        {
            MainPage mainPage = new MainPage(httpClient);
            Register1Page register1Page = new Register1Page(httpClient,mainPage.RegisterPageUrl);
            string s = register1Page.GoToNext();
            ;

            //string rez = Post(url.PathAndQuery, postData);

            return null;
        }
        public class MainPage
        {
            private string mainUrl = "https://www.i.ua/";
            private HttpClient httpClient;
            private string registerLinkPath = "//div[@class='Header clear']//li[@class='right']//a[@href]";
            private HtmlAgilityPack.HtmlDocument doc;
            private string GetMainPage()
            {
                return httpClient.GetStringAsync(mainUrl).GetAwaiter().GetResult();
            }
            public MainPage(HttpClient httpClient, string mainUrl = "https://www.i.ua/")
            {
                this.httpClient = httpClient;
                this.mainUrl = mainUrl;
                doc = new HtmlAgilityPack.HtmlDocument();
                string mainPage = GetMainPage();
                doc.LoadHtml(mainPage);
            }
            private string GetRegisterPageUrl()
            {
                var node = doc.DocumentNode.SelectSingleNode(registerLinkPath);
                string link = node.GetAttributeValue("href", null);
                return link;
            }
            public string RegisterPageUrl
            {
                get { return GetRegisterPageUrl(); }
            }
        }
        public class Register1Page
        {
            #region Xpath
            private string GRecaptchaXpath = "//div[@class='g-recaptcha' and @data-sitekey]";
            private string PpcXpath = "//input[@name='ppc']";
            private string CtXpath = "//input[@name='ct']";
            private string FormXpath = "//form[@name='rform']";
            private string _submXpath = "//input[@name='_subm']";
            private string typeXpath = "//input[@name='type']";
            private string socialUserInfoXpath = "//input[@name='socialUserInfo']";
            private string socialKeyXpath = "//input[@name='socialKey']";
            private string soc_emailXpath = "//input[@name='soc_email']";
            private string _urlXpath = "//input[@name='_url']";
            private string submXpath = "//input[@name='subm']";
            private string elementFormNoName1Xpath = "//div[@class='necessary']/input[@type='text' and @name and @value and @style='display:none']";
            private string elementFormNoName2Xpath = "//div[@class='necessary']/input[@type='text' and @name and @value and @style='']";
            private string emailXpath = "//input[@name='email']";
            private string userXpath = "//input[@name='user']";
            private string loginXpath = "//input[@name='login']";
            private string passwordXpath = "//input[@type='password']";
            #endregion

            private string getCountFuncrion = "function getCount(){return document.getElementsByName('rform')[0].elements.length}";
            private HttpClient httpClient;
            private string url;
            private HtmlAgilityPack.HtmlDocument doc;
            public Register1Page(HttpClient httpClient, string url)
            {
                this.httpClient = httpClient;
                this.url = url;
                doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(GetPageHtml());
            }
            private string GetPageHtml()
            {
                string html = httpClient.GetStringAsync(url).GetAwaiter().GetResult();
                return html;
            }
            private string getRecaptchaKey()
            {
                return getValue(GRecaptchaXpath);
            }
            public string GRecaptchaKey
            {
                get
                {
                    return getRecaptchaKey();
                }
            }
            #region GetElements
            private HtmlNode getElement(string xpath)
            {
                return doc.DocumentNode.SelectSingleNode(xpath);
            }
            private string getValue(string xpath)
            {
                return getElement(xpath).GetAttributeValue("value", null);
            }
            private string _subm
            {
                get { return getValue(_submXpath); }
            }
            private string type
            {
                get { return getValue(typeXpath); }
            }
            private string socialUserInfo
            {
                get { return getValue(socialUserInfoXpath); }
            }
            private string socialKey
            {
                get { return getValue(socialKeyXpath); }
            }
            private string soc_email
            {
                get { return getValue(soc_emailXpath); }
            }
            private string _url
            {
                get { return getValue(_urlXpath); }
            }
            private string ppc
            {
                get { return getValue(PpcXpath); }
            }
            private string ct
            {
                get { return getValue(CtXpath); }
            }
            public object getFormCountElements()
            {
                WebBrowser w = null;
                int n=-2;
                Thread t = new Thread(()=> 
                {
                    w = new WebBrowser();
                    w.Navigate("about:blank");
                    w.Document.Write("<html><script>"+getCountFuncrion+"</script><body>" + getElement(FormXpath).OuterHtml + "</body></html>");
                    w.Refresh();
                    var rez = w.Document.InvokeScript("getCount");
                    if (!(rez is null))
                    {
                        n = (int)rez;
                    }
                    else
                        n = -1;
                });
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
                while(n==-2)
                {
                    Thread.Sleep(600);
                }
                return n;
            }
            private string crg
            {
                get { return getFormCountElements().ToString(); }
            }
            private string getName(string xpath)
            {
                return getElement(xpath).GetAttributeValue("name", null);
            }
            private string getName(HtmlNode node)
            {
                return node.GetAttributeValue("name",null);
            }
            private string getValue(HtmlNode node)
            {
                return node.GetAttributeValue("value", null);
            }
            private HtmlNode elementFormNoName1
            {
                get { return doc.DocumentNode.SelectSingleNode(elementFormNoName1Xpath); }
            }
            private HtmlNode elementFormNoName2
            {
                get { return doc.DocumentNode.SelectSingleNode(elementFormNoName2Xpath); }
            }
            private HtmlNode elementFormNoName3
            {
                get { return doc.DocumentNode.SelectNodes(elementFormNoName1Xpath)[1]; }
            }
            private string email
            {
                get { return getValue(emailXpath); }
            }
            private string user
            {
                get { return getValue(userXpath); }
            }
            private string login
            {
                get { return getValue(loginXpath); }
            }
            private HtmlNodeCollection getElements(string xpath)
            {
                return doc.DocumentNode.SelectNodes(xpath);
            }
            private string subm
            {
                get { return getValue(submXpath); }
            }
            #endregion
            private string Post(string url, FormUrlEncodedContent postData)
            {
                string content = null;
                try
                {
                    HttpResponseMessage httpResponseMessage;
                    httpResponseMessage = httpClient.PostAsync(url, postData).GetAwaiter().GetResult();
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
            public string GoToNext()
            {
                string domen = "i.ua";
                string recaptcha = "";
                string password = "";
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("_subm",_subm),
                    new KeyValuePair<string, string>("type",type),
                    new KeyValuePair<string, string>("socialUserInfo",socialUserInfo),
                    new KeyValuePair<string, string>("socialKey",socialKey),
                    new KeyValuePair<string, string>("soc_email",soc_email),
                    new KeyValuePair<string, string>("_url",_url),
                    new KeyValuePair<string, string>("crg",crg),
                    new KeyValuePair<string, string>("ppc",ppc),
                    new KeyValuePair<string, string>("ct",ct),
                    new KeyValuePair<string, string>("login_alternate","1"),
                    new KeyValuePair<string, string>(getName(elementFormNoName1),getValue(elementFormNoName1Xpath)),
                    new KeyValuePair<string, string>(getName(elementFormNoName2),login),
                    new KeyValuePair<string, string>(getName(elementFormNoName3),getValue(elementFormNoName3)),
                    new KeyValuePair<string, string>("domn",domen),
                    new KeyValuePair<string, string>("email",email),
                    new KeyValuePair<string, string>("g-recaptcha-response",recaptcha),
                    new KeyValuePair<string, string>("login",""),

                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[0]),getValue(getElements(passwordXpath)[0])),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[1]),getValue(getElements(passwordXpath)[1])),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[2]),password),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[3]),getValue(getElements(passwordXpath)[3])),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[4]),getValue(getElements(passwordXpath)[4])),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[5]),password),
                    new KeyValuePair<string, string>("subm",subm),
                });
                string nextText = Post(new Uri(url).PathAndQuery, postData);
                return nextText;
            }


        }
        public Task<bool> OpenRegister()
        {
            return Task.Run(()=> 
            {
                if (RegisterLink is null)
                {
                    MainPage mainPage = new MainPage(httpClient);
                    RegisterLink = mainPage.RegisterPageUrl;
                }
                Register1Page register1Page = new Register1Page(httpClient, RegisterLink);
                string s = register1Page.GoToNext();
                
                return false;
            });
        }

    }
}
