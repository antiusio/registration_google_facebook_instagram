using Accounts;
using Accounts.Data;
using DataBase.DataStructures;
using HtmlAgilityPack;
using ServiceRegistration.Services;
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
        string ip;
        int port;
        private static string RegisterLink = null;
        private HttpClient httpClient;
        private CookieContainer cookieContainer;
        public RegistrationIua(string ip=null, int port = 0, string baseAdress = "https://www.i.ua/")
        {
            this.ip = ip;
            this.port = port;
            HttpClientHandler handler;
            cookieContainer = new CookieContainer();
            //HtmlDocument myaccountDocument = new HtmlDocument();
            if(!(ip is null))
                handler = new HttpClientHandler
                {
                    CookieContainer = cookieContainer,
                    Proxy = new WebProxy(ip, port),
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

            //httpClient.BaseAddress = new Uri(baseAdress);
        }
        public string GetWhoer()
        {
            var byteArray =  httpClient.GetByteArrayAsync("https://f.vision").GetAwaiter().GetResult();
            var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
            return responseString;
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
        public abstract class RegisterPage
        {
            protected HttpClient httpClient;
            protected string url;
            protected HtmlAgilityPack.HtmlDocument doc;

            protected HtmlNode getElement(string xpath)
            {
                return doc.DocumentNode.SelectSingleNode(xpath);
            }
            protected string getValue(string xpath)
            {
                return getElement(xpath).GetAttributeValue("value", null);
            }

            protected string getName(string xpath)
            {
                return getElement(xpath).GetAttributeValue("name", null);
            }
            protected string getName(HtmlNode node)
            {
                return node.GetAttributeValue("name", null);
            }
            protected string getValue(HtmlNode node)
            {
                return node.GetAttributeValue("value", null);
            }
            protected HtmlNodeCollection getElements(string xpath)
            {
                return doc.DocumentNode.SelectNodes(xpath);
            }
        }
        public class RegisterPage1: RegisterPage
        {
            public string GToken { get; set; }
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
            private string elementFormNoName1Xpath = "//div[@class='necessary']/input[@type='text' and @name and @value]";
            private string emailXpath = "//input[@name='email']";
            private string userXpath = "//input[@name='user']";
            private string loginXpath = "//input[@name='login']";
            private string passwordXpath = "//input[@type='password']";
            private string errorTextXpath = "//div[@id='rform_errCtrl']//div[@class='content clear']";
            

            private string getCountFunction = "function getCount(){return document.getElementsByName('rform')[0].elements.length}";
            #endregion
            public RegisterPage1(HttpClient httpClient, string url)
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
            private string[] errors = new string[] 
            {
                "Регистрация временно не доступна. Попробуйте позже.",
                "При заполнении формы были допущены ошибки!"
            };
            private string[] whatYouDo = new string[] 
            {
                "Change proxy",
                "GenerateNewData"
            };
            #region GetElements
            private string getRecaptchaKey()
            {
                return doc.DocumentNode.SelectSingleNode(GRecaptchaXpath).GetAttributeValue("data-sitekey", null);
            }
            public string GRecaptchaKey
            {
                get
                {
                    return getRecaptchaKey();
                }
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
                    var node = doc.CreateElement("script");
                    doc.DocumentNode.SelectSingleNode("//head").ChildNodes.Add(node);
                    w.Document.Write("<html><script>"+getCountFunction+"</script><body>" + getElement(FormXpath).OuterHtml + "</body></html>");
                    //w.Document.Write(doc.DocumentNode.OuterHtml);
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
            private HtmlNode elementFormNoName1
            {
                get { return doc.DocumentNode.SelectSingleNode(elementFormNoName1Xpath); }
            }
            private HtmlNode elementFormNoName2
            {
                get { return doc.DocumentNode.SelectNodes(elementFormNoName1Xpath)[1]; }
            }
            private HtmlNode elementFormNoName3
            {
                get { return doc.DocumentNode.SelectNodes(elementFormNoName1Xpath)[2]; }
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
            private string subm
            {
                get { return getValue(submXpath); }
            }
            #endregion

            private bool countEntryMoreOnce(string substr, string str)
            {
                int index1 = str.IndexOf(substr);
                int index2 = str.LastIndexOf(substr);
                if (index1 != index2)
                    return true;
                return false;
            }
            public string GoToNext(string yourLogin, string yourPassword,string ip=null, int port=0)
            {
                string domen = "i.ua";
                Settings s = new Settings();
                string recaptcha = "";
                //httpClient.BaseAddress = new Uri(RegisterLink.Remove(RegisterLink.IndexOf("registration/")));
                try
                {
                    recaptcha = RuCaptcha.SolveRecaptcha(s.RuCaptchaApiKey, GRecaptchaKey, RegisterLink);
                    GToken = recaptcha;
                }
                catch { }
                ;
                
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("_subm",_subm),
                    new KeyValuePair<string, string>("type",type),
                    new KeyValuePair<string, string>("socialUserInfo",socialUserInfo),
                    new KeyValuePair<string, string>("socialKey",socialKey),
                    new KeyValuePair<string, string>("soc_email",soc_email),
                    new KeyValuePair<string, string>("_url",_url),
                    new KeyValuePair<string, string>("crg","33"),
                    new KeyValuePair<string, string>("ppc",ppc),
                    new KeyValuePair<string, string>("ct",ct),
                    new KeyValuePair<string, string>("login_alternate","1"),
                    new KeyValuePair<string, string>(getName(elementFormNoName1), !countEntryMoreOnce(getName(elementFormNoName1),doc.DocumentNode.OuterHtml)? getValue(elementFormNoName1):yourLogin),
                    new KeyValuePair<string, string>(getName(elementFormNoName2),!countEntryMoreOnce(getName(elementFormNoName2),doc.DocumentNode.OuterHtml)? getValue(elementFormNoName2):yourLogin),
                    new KeyValuePair<string, string>(getName(elementFormNoName3),!countEntryMoreOnce(getName(elementFormNoName3),doc.DocumentNode.OuterHtml)? getValue(elementFormNoName3):yourLogin),
                    new KeyValuePair<string, string>("domn",domen),
                    new KeyValuePair<string, string>("email",email),
                    new KeyValuePair<string, string>("g-recaptcha-response",recaptcha),
                    new KeyValuePair<string, string>("login",""),

                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[0]),!countEntryMoreOnce(getName(getElements(passwordXpath)[0]),doc.DocumentNode.OuterHtml)? getValue(getElements(passwordXpath)[0]):yourPassword),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[1]),!countEntryMoreOnce(getName(getElements(passwordXpath)[1]),doc.DocumentNode.OuterHtml)? getValue(getElements(passwordXpath)[1]):yourPassword),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[2]),!countEntryMoreOnce(getName(getElements(passwordXpath)[2]),doc.DocumentNode.OuterHtml)? getValue(getElements(passwordXpath)[2]):yourPassword),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[3]),!countEntryMoreOnce(getName(getElements(passwordXpath)[3]),doc.DocumentNode.OuterHtml)? getValue(getElements(passwordXpath)[3]):yourPassword),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[4]),!countEntryMoreOnce(getName(getElements(passwordXpath)[4]),doc.DocumentNode.OuterHtml)? getValue(getElements(passwordXpath)[4]):yourPassword),
                    new KeyValuePair<string, string>(getName(getElements(passwordXpath)[5]),!countEntryMoreOnce(getName(getElements(passwordXpath)[5]),doc.DocumentNode.OuterHtml)? getValue(getElements(passwordXpath)[5]):yourPassword),
                    new KeyValuePair<string, string>("subm",subm),
                });
                
                string nextText = PostApi.Post(url, postData,ip,port);
                doc.LoadHtml(nextText);
                int n = whatDo();
                string what;
                if (n != -1)
                {
                    what = whatYouDo[n];
                }
                return nextText;
            }
            private int whatDo()
            {
                string errorText = getPageErrorString();
                if (errorText is null)
                    return -1;
                for (int i = 0; i < errors.Length; i++)
                {
                    if (errors[i].Equals(errorText))
                        return i;
                }
                return -1;
            }
            private string getPageErrorString()
            {
                try
                {
                    var element = doc.DocumentNode.SelectSingleNode(errorTextXpath);
                    return element.InnerText;
                }
                catch { return null; }
            }
        }
        
        public class RegisterPage2: RegisterPage
        {
            
            public RegisterPage2(HttpClient httpClient, string outherHtml)
            {
                httpClient = new HttpClient();
                doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(outherHtml);
            }
            #region Xpath
            private static string getXpathForName(string name) { return "//*[@name='" + name + "']"; }
            private string _submXpath = getXpathForName("_subm");
            private string _urlXpath = getXpathForName("_url");
            private string noNameElementXpath = "//form[@name]//input[@name and @value]";
            private string ppcXpath = getXpathForName("ppc");
            private string submXpath = getXpathForName("subm");
            private string urlXpath = getXpathForName("url");
            private string sectionXpath = getXpathForName("section");
            private string inviteXpath = getXpathForName("invite");
            private string soc_emailXpath = getXpathForName("soc_email");
            private string countryOptionsXpath = getXpathForName("country")+"//option[@value]";
            private string cityOptionsXpath = getXpathForName("city")+ "//option[@value]";
            private string questOptionsXpath = getXpathForName("quest") + "//option[@value]";
            #endregion
            #region GetElements
            private string _subm
            {
                get { return getValue(_submXpath); }
            }
            private string _url
            {
                get { return getValue(_urlXpath); }
            }
            private string ppc
            {
                get { return getValue(ppcXpath); }
            }
            private string subm
            {
                get { return getValue(submXpath); }
            }
            private string url
            {
                get { return getValue(urlXpath); }
            }
            private string section
            {
                get { return getValue(sectionXpath); }
            }
            private string invite
            {
                get { return getValue(inviteXpath); }
            }
            private string soc_email
            {
                get { return getValue(soc_emailXpath); }
            }
            private string getCountryId(string country)
            {
                var countryOptions = getElements(countryOptionsXpath);
                var countryElements = countryOptions.Where(x => x.InnerText.Equals(country));
                if (countryElements.Count() == 0)
                    return null;
                return countryElements.First().Attributes["value"].Value;
            }
            private string getCityId(string city)
            {
                var cityOptions = getElements(cityOptionsXpath);
                var cityElements = cityOptions.Where(x=>x.InnerText.Equals(city));
                if (cityElements.Count() == 0)
                    return null;
                return cityElements.First().Attributes["value"].Value;
            }
            private string getQuestId(string quest)
            {
                var questOptions = getElements(questOptionsXpath);
                var questElements = questOptions.Where(x => x.InnerText.Equals(quest));
                if (questElements.Count() == 0)
                    return null;
                return questElements.First().Attributes["value"].Value;
            }
            #endregion
            public string GoNext(AccIua accIua, string gToken, string ip = null, int port = 0)
            {
                string fname = accIua.FirstName;//имя
                string lname = accIua.LastName;//фамилия
                string s = ((int)accIua.Sex).ToString();
                string day = accIua.DateBirth.Day.ToString();
                string month = accIua.DateBirth.Month.ToString();
                string year = accIua.DateBirth.Year.ToString();
                string country = accIua.Country;
                string quest = accIua.SecretQuestion;
                string answer = accIua.Answer;
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("_subm",_subm),
                    new KeyValuePair<string, string>("_url",_url),
                    new KeyValuePair<string, string>(getName(getElements(noNameElementXpath)[2]),getValue(getElements(noNameElementXpath)[2])),
                    new KeyValuePair<string, string>(getName(getElements(noNameElementXpath)[3]),getValue(getElements(noNameElementXpath)[3])),
                    new KeyValuePair<string, string>("ppc",ppc),
                    new KeyValuePair<string, string>("subm",subm),
                    new KeyValuePair<string, string>("url",url),
                    new KeyValuePair<string, string>("section",section),
                    new KeyValuePair<string, string>("invite",invite),
                    new KeyValuePair<string, string>("soc_email",soc_email),
                    new KeyValuePair<string, string>("fname",fname),
                    new KeyValuePair<string, string>("lname",lname),
                    new KeyValuePair<string, string>("s",s),
                    new KeyValuePair<string, string>("day",day),
                    new KeyValuePair<string, string>("month",month),
                    new KeyValuePair<string, string>("year",year),
                    new KeyValuePair<string, string>("country",getCountryId(country)),
                    new KeyValuePair<string, string>("lang","1"),
                    new KeyValuePair<string, string>("agree","1"),
                    new KeyValuePair<string, string>("alt_email",""),
                    new KeyValuePair<string, string>("quest",getQuestId(quest)),
                    new KeyValuePair<string, string>("answer",answer),
                    new KeyValuePair<string, string>("phone",""),
                    new KeyValuePair<string, string>("g-token",gToken),
                });
                string rez = PostApi.Post(url, postData, ip, port);
                return rez;
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
                RegisterPage1 registerPage1 = new RegisterPage1(httpClient, RegisterLink);
                AccIua acc = new AccIua(SexIua.мужcкой);
                string s = registerPage1.GoToNext(acc.Login, acc.Password,ip,port);
                ;
                RegisterPage2 registerPage2 = new RegisterPage2(httpClient, s);
                ;
                string s2 = registerPage2.GoNext(acc, registerPage1.GToken,ip,port);

                return false;
            });
        }

    }
}
