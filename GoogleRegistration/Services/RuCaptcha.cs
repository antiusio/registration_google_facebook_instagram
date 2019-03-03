using Accounts.InterfaceAccs;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRegistration.Services
{
    public static class RuCaptcha
    {
        public static void SolveRecaptcha(string apiKey, Feedback acc, IWebDriver driver, string idTextRecaptcha = "g-recaptcha-response")
        {
            IWebElement webElement = null;
            webElement = driver.FindElements(By.XPath("//body//iframe"))[0];
            string src = "";
            if (webElement != null)
            {
                src = webElement.GetAttribute("src");
            }
            string k = src.Remove(0, src.IndexOf("k=") + 2);
            k = k.Remove(k.IndexOf("&"));
            //http://rucaptcha.com/in.php?key=1abc234de56fab7c89012d34e56fa7b8&method=userrecaptcha&googlekey=6Le-wvkSVVABCPBMRTvw0Q4Muexq1bi0DJwx_mJ-&pageurl=http://mysite.com/page/with/recaptcha?appear=1&here=now
            HttpClient httpClient = new HttpClient();
            B:
            string respond = "";
            try
            {
                acc.StatusText = "Запрос к сервису капчи ";
                respond = httpClient.GetStringAsync("http://rucaptcha.com/in.php?key=" + apiKey + "&method=userrecaptcha&googlekey=" + k + "&pageurl=" + driver.Url).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                acc.StatusText = "Ошибка при отправке запроса к сервису капчи: " + ex.Message;
                Thread.Sleep(6000);
                goto B;
            }
            string idCaptcha = "";
            if (respond.IndexOf("OK") != -1)
            {
                idCaptcha = respond.Remove(0, respond.IndexOf('|') + 1);
            }

            for (; ; )
            {
                Thread.Sleep(6000);
                respond = httpClient.GetStringAsync("http://rucaptcha.com/res.php?key=" + apiKey + "&action=get&id=" + idCaptcha).GetAwaiter().GetResult();

                if (respond.IndexOf("ERROR_WRONG_CAPTCHA_ID") != -1)
                {
                    acc.StatusText = respond;
                    throw new Exception("ERROR_WRONG_CAPTCHA_ID");
                }
                if (respond.Equals("ERROR_CAPTCHA_UNSOLVABLE"))
                {
                    acc.StatusText = respond;
                    throw new Exception("ERROR_CAPTCHA_UNSOLVABLE");
                }
                if (respond.IndexOf("CAPCHA_NOT_READY") == -1)
                {
                    break;
                }

            }

            string result = respond.Split(new char[] { '|' })[1];
            webElement = driver.FindElement(By.Id(idTextRecaptcha));
            ((IJavaScriptExecutor)driver).ExecuteScript("document.getElementById('" + idTextRecaptcha + "').value='" + result + "';", webElement);
            ;
            //webElement.
            //webElement.SendKeys(result);
        }
        public static string SolveRecaptcha(string apiKey, string gKey,string url)
        {
            HttpClient httpClient = new HttpClient();
            B:
            string respond = "";
            try
            {
                respond = httpClient.GetStringAsync("http://rucaptcha.com/in.php?key=" + apiKey + "&method=userrecaptcha&googlekey=" + gKey + "&pageurl=" + url).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Thread.Sleep(6000);
                goto B;
            }
            string idCaptcha = "";
            if (respond.IndexOf("OK") != -1)
            {
                idCaptcha = respond.Remove(0, respond.IndexOf('|') + 1);
            }

            for (; ; )
            {
                Thread.Sleep(6000);
                respond = httpClient.GetStringAsync("http://rucaptcha.com/res.php?key=" + apiKey + "&action=get&id=" + idCaptcha).GetAwaiter().GetResult();

                if (respond.IndexOf("ERROR_WRONG_CAPTCHA_ID") != -1)
                {
                    throw new Exception("ERROR_WRONG_CAPTCHA_ID");
                }
                if (respond.Equals("ERROR_CAPTCHA_UNSOLVABLE"))
                {
                    throw new Exception("ERROR_CAPTCHA_UNSOLVABLE");
                }
                if (respond.IndexOf("CAPCHA_NOT_READY") == -1)
                {
                    break;
                }

            }

            string result = respond.Split(new char[] { '|' })[1];
            return result;
        }
    }
}
