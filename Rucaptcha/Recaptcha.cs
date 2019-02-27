using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rucaptcha
{
    public class Recaptcha
    {
        public void rucaptcha(string apiKey, Acc accProgram)
        {
            IWebElement webElement = null;
            webElement = browser.FindElement(By.TagName("iframe"));
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
                accProgram.StatusText = "Запрос к сервису капчи ";
                respond = httpClient.GetStringAsync("http://rucaptcha.com/in.php?key=" + apiKey + "&method=userrecaptcha&googlekey=" + k + "&pageurl=https://account.ncsoft.com/signup/index?serviceCode=13#").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                accProgram.StatusText = "Ошибка при отправке запроса: " + ex.Message;
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
                if (respond.IndexOf("CAPCHA_NOT_READY") == -1)
                {
                    break;
                }
                if (respond.IndexOf("ERROR_WRONG_CAPTCHA_ID") != -1)
                {
                    accProgram.StatusText = respond;
                    throw new Exception("ERROR_WRONG_CAPTCHA_ID");
                }
                if (respond.Equals("ERROR_CAPTCHA_UNSOLVABLE"))
                {
                    accProgram.StatusText = respond;
                    throw new Exception("ERROR_CAPTCHA_UNSOLVABLE");
                }

            }

            string result = respond.Split(new char[] { '|' })[1];
            webElement = browser.FindElement(By.Id("g-recaptcha-response"));
            ((IJavaScriptExecutor)browser).ExecuteScript("document.getElementById('g-recaptcha-response').value='" + result + "';", webElement);
            ;
            //webElement.
            //webElement.SendKeys(result);
        }
    }
}
