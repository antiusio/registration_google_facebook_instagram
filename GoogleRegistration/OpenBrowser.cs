using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRegistration
{
    public partial class RegistrationIua
    {
        IWebDriver browser = null;
        string url = "https://www.i.ua/";
        public void OpenBrowser()
        {
            //FirefoxProfile firefoxProfile = new FirefoxProfile();
            //string pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\shape_shifter-0.0.2-an+fx.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.shape_shifter.currentVersion", "0.0.2");


            //pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\firebug-1.8.1.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.firebug.currentVersion", "1.8.1");
            ////var allProfiles = new FirefoxProfileManager();
            ////
            ////set the webdriver_assume_untrusted_issuer to false
            //firefoxProfile.SetPreference("webdriver_assume_untrusted_issuer", false);


            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0");
            //Mozilla/5.0 (Linux; U; Android 4.3; de-de; GT-I9300 Build/JSS15J) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30
            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (Linux; U; Android 4.3; de-de; GT-I9300 Build/JSS15J) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30");

            //if (!allProfiles.ExistingProfiles.Contains("SeleniumUser"))
            //{
            //   throw new Exception("SeleniumUser firefox profile does not exist, please create it first.");
            //}
            //var profile = allProfiles.GetProfile("SeleniumUser");
            //string s = Directory.GetCurrentDirectory();
            //File.Open(@"Plugins\shape_shifter-0.0.2-an+fx.xpi",FileMode.Open);
            //var fireFoxOptions = new FirefoxOptions();
            //fireFoxOptions.Profile = firefoxProfile;
            //fireFoxOptions.SetPreference("network.proxy.type", 1);
            //fireFoxOptions.SetPreference("network.proxy.socks", "127.0.0.1");


            //fireFoxOptions.SetPreference("network.proxy.socks_port", socksPort);


            //fireFoxOptions.SetPreference("network.proxy.http", "7951");
            //fireFoxOptions.SetPreference("network.proxy.socks_port", 40348);
            //fireFoxOptions.SetPreference("media.peerconnection.enabled", false);
            //network.proxy.socks_remote_dns

            //;
            //fireFoxOptions.SetPreference("network.proxy.socks_remote_dns", true);
            //;

            //fireFoxOptions.SetLoggingPreference
            //Mozilla/5.0 (Linux; U; Android 4.4 Andro-id Build/KRT16S; X11; FxOS armv7I rv:29.0) MyWebkit/537.51.1 (KHTML, like Gecko) Gecko/29.0 Firefox/29.0


            //profile.update_preferences()

            //# You would also like to block flash 
            //           profile.set_preference('dom.ipc.plugins.enabled.libflashplayer.so', False)
            //profile.set_preference("media.peerconnection.enabled", False)

            //browser = new FirefoxDriver(fireFoxOptions);
            //browser.Manage().Window.Size = new Size(1440, 900);

            //browser.Navigate().GoToUrl(url);
            browser = new ChromeDriver();
        }
    }
    public partial class RegistrationGoogle
    {
        IWebDriver browser = null;
        string url = "https://www.google.com/gmail/about/new/";
        public void OpenBrowser()
        {
            browser = new ChromeDriver();

            //FirefoxProfile firefoxProfile = new FirefoxProfile();
            //string pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\shape_shifter-0.0.2-an+fx.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.shape_shifter.currentVersion", "0.0.2");


            //pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\firebug-1.8.1.xpi";
            //firefoxProfile.AddExtension(pathToExtension);
            //firefoxProfile.SetPreference("extensions.firebug.currentVersion", "1.8.1");
            ////var allProfiles = new FirefoxProfileManager();
            ////
            ////set the webdriver_assume_untrusted_issuer to false
            //firefoxProfile.SetPreference("webdriver_assume_untrusted_issuer", false);




            ////media.video_stats.enabled
            //firefoxProfile.SetPreference("media.video_stats.enabled", false);
            ////privacy.resistFingerprinting.reduceTimerPrecision.microseconds
            //firefoxProfile.SetPreference("privacy.resistFingerprinting.reduceTimerPrecision.microseconds", 20);
            ////webgl.disabled
            //firefoxProfile.SetPreference("webgl.disabled", true);
            ////media.navigator.enabled
            //firefoxProfile.SetPreference("media.navigator.enabled", false);
            ////privacy.resistFingerprinting
            //firefoxProfile.SetPreference("privacy.resistFingerprinting", true);
            ////reader.font_size
            //firefoxProfile.SetPreference("reader.font_size", 7);
            ////canvas.path.enabled
            //firefoxProfile.SetPreference("canvas.path.enabled", false);
            ////media.navigator.audio.fake_frequency
            //firefoxProfile.SetPreference("media.navigator.audio.fake_frequency", 1240);
            ////media.recorder.audio_node.enabled
            //firefoxProfile.SetPreference("media.recorder.audio_node.enabled", false);
            ////dom.webaudio.enabled
            //firefoxProfile.SetPreference("dom.webaudio.enabled", false);
            ////gfx.downloadable_fonts.enabled
            //firefoxProfile.SetPreference("gfx.downloadable_fonts.enabled", false);





            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0");
            //Mozilla/5.0 (Linux; U; Android 4.3; de-de; GT-I9300 Build/JSS15J) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30
            //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (Linux; U; Android 4.3; de-de; GT-I9300 Build/JSS15J) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30");

            //if (!allProfiles.ExistingProfiles.Contains("SeleniumUser"))
            //{
            //   throw new Exception("SeleniumUser firefox profile does not exist, please create it first.");
            //}
            //var profile = allProfiles.GetProfile("SeleniumUser");
            //string s = Directory.GetCurrentDirectory();
            //File.Open(@"Plugins\shape_shifter-0.0.2-an+fx.xpi",FileMode.Open);


            //var fireFoxOptions = new FirefoxOptions();
            //fireFoxOptions.Profile = firefoxProfile;


            //fireFoxOptions.SetPreference("network.proxy.type", 1);
            //fireFoxOptions.SetPreference("network.proxy.socks", "127.0.0.1");


            //fireFoxOptions.SetPreference("network.proxy.socks_port", socksPort);


            //fireFoxOptions.SetPreference("network.proxy.http", "7951");
            //fireFoxOptions.SetPreference("network.proxy.socks_port", 40348);
            //fireFoxOptions.SetPreference("media.peerconnection.enabled", false);
            //network.proxy.socks_remote_dns

            //;
            //fireFoxOptions.SetPreference("network.proxy.socks_remote_dns", true);
            //;

            //fireFoxOptions.SetLoggingPreference
            //Mozilla/5.0 (Linux; U; Android 4.4 Andro-id Build/KRT16S; X11; FxOS armv7I rv:29.0) MyWebkit/537.51.1 (KHTML, like Gecko) Gecko/29.0 Firefox/29.0


            //profile.update_preferences()

            //# You would also like to block flash 
            //           profile.set_preference('dom.ipc.plugins.enabled.libflashplayer.so', False)
            //profile.set_preference("media.peerconnection.enabled", False)

            //browser = new FirefoxDriver(fireFoxOptions);
            //browser.Navigate().GoToUrl("https://www.olx.ua/");
            //var ele=browser.FindElement(By.Id("headerSearch"));
            //ele.SendKeys("ежедневная оплата");

            //ele = browser.FindElement(By.Id("cityField"));
            //ele.SendKeys("Харьков, Харьковская область");

            //ele = browser.FindElement(By.Id("submit-searchmain"));
            //Thread.Sleep(1000);
            //ele.Click();
            //ele.Click();
            //browser.Manage().Window.Size = new Size(1440, 900);

            //browser.Navigate().GoToUrl(url);
        }
    }
}
