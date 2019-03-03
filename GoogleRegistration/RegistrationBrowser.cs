using Accounts;
using Accounts.GenerationInfo;
using DataBase;
using DataBase.DataStructures;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using ServiceRegistration.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRegistration
{
    public enum TypeBrowserEnum
    {
        Chrome=1,
        FireFox = 2,

    }
    public class RegistrationBrowser
    {
        public static Settings settingsDB;
        public IWebDriver driver;
        //Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.96 Safari/537.36
        public RegistrationBrowser(string ip = null, int port = 0,string userAgent= "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.96 Safari/537.36", TypeBrowserEnum typeBrowser= TypeBrowserEnum.FireFox)
        {
            if (settingsDB is null)
                settingsDB = new Settings();
            if (typeBrowser == TypeBrowserEnum.FireFox)
            {


                //CResolution ChangeRes = new CResolution(1280,768);
                //Environment.SetEnvironmentVariable();
                FirefoxProfile firefoxProfile = new FirefoxProfile();
                Random r = new Random(DateTime.Now.Millisecond);
                //media.video_stats.enabled
                firefoxProfile.SetPreference("media.video_stats.enabled", false);
                //privacy.resistFingerprinting.reduceTimerPrecision.microseconds
                firefoxProfile.SetPreference("privacy.resistFingerprinting.reduceTimerPrecision.microseconds", r.Next(20, 100));
                //webgl.disabled
                firefoxProfile.SetPreference("webgl.disabled", true);
                //media.navigator.enabled
                firefoxProfile.SetPreference("media.navigator.enabled", false);
                //privacy.resistFingerprinting
                //firefoxProfile.SetPreference("privacy.resistFingerprinting", true);
                //reader.font_size
                firefoxProfile.SetPreference("reader.font_size", r.Next(1, 20));
                //canvas.path.enabled
                firefoxProfile.SetPreference("canvas.path.enabled", false);
                //media.navigator.audio.fake_frequency
                firefoxProfile.SetPreference("media.navigator.audio.fake_frequency", r.Next(800, 1200));
                //media.recorder.audio_node.enabled
                firefoxProfile.SetPreference("media.recorder.audio_node.enabled", false);
                //dom.webaudio.enabled
                firefoxProfile.SetPreference("dom.webaudio.enabled", false);
                //gfx.downloadable_fonts.enabled
                firefoxProfile.SetPreference("gfx.downloadable_fonts.enabled", false);


                //string pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\shape_shifter-0.0.2-an+fx.xpi";
                //firefoxProfile.AddExtension(pathToExtension);
                //firefoxProfile.SetPreference("extensions.shape_shifter.currentVersion", "0.0.2");


                //pathToExtension = Directory.GetCurrentDirectory() + @"\Plugins\firebug-1.8.1.xpi";
                //firefoxProfile.AddExtension(pathToExtension);
                //firefoxProfile.SetPreference("extensions.firebug.currentVersion", "1.8.1");
                //var allProfiles = new FirefoxProfileManager();
                //
                //set the webdriver_assume_untrusted_issuer to false
                firefoxProfile.SetPreference("webdriver_assume_untrusted_issuer", false);


                //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Linux x86_64; rv:52.0) Gecko/20100101 Firefox/52.0");
                //Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:61.0) Gecko/20100101 Firefox/61.0
                //firefoxProfile.SetPreference("general.useragent.override", "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:61.0) Gecko/20100101 Firefox/61.0");
                firefoxProfile.SetPreference("general.useragent.override", userAgent);



                //if (!allProfiles.ExistingProfiles.Contains("SeleniumUser"))
                //{
                //   throw new Exception("SeleniumUser firefox profile does not exist, please create it first.");
                //}
                //var profile = allProfiles.GetProfile("SeleniumUser");
                //string s = Directory.GetCurrentDirectory();
                //File.Open(@"Plugins\shape_shifter-0.0.2-an+fx.xpi",FileMode.Open);
                var fireFoxOptions = new FirefoxOptions();
                fireFoxOptions.Profile = firefoxProfile;
                
                if (!(ip is null))
                {
                    fireFoxOptions.SetPreference("network.proxy.type", 1);
                    fireFoxOptions.SetPreference("network.proxy.http", ip);
                    fireFoxOptions.SetPreference("network.proxy.ssl", ip);

                    fireFoxOptions.SetPreference("network.proxy.http_port", port);
                    fireFoxOptions.SetPreference("network.proxy.ssl_port", port);
                }


                //fireFoxOptions.SetPreference("network.proxy.http", "7951");
                //fireFoxOptions.SetPreference("network.proxy.socks_port", 40348);

                fireFoxOptions.SetPreference("media.peerconnection.enabled", false);

                fireFoxOptions.SetPreference("permissions.default.image", 2);
                //fireFoxOptions.SetPreference("media.peerconnection.use_document_iceservers", false);
                //network.http.sendRefererHeader
                //fireFoxOptions.SetPreference("network.http.sendRefererHeader", 0);
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

                driver = new FirefoxDriver(fireFoxOptions);
                //driver.Navigate().GoToUrl("https://whoer.net/");
            }
            if(typeBrowser== TypeBrowserEnum.Chrome)
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                if (!(ip is null))
                {
                    chromeOptions.AddArguments("--proxy-server=https://" + ip+":"+port);
                    //chromeOptions.SetPreference("network.proxy.type", 1);
                    //chromeOptions.SetPreference("network.proxy.http", ip);
                    //chromeOptions.SetPreference("network.proxy.ssl", ip);

                    //chromeOptions.SetPreference("network.proxy.http_port", port);
                    //chromeOptions.SetPreference("network.proxy.ssl_port", port);
                }
                driver = new ChromeDriver(chromeOptions);
            }
        }
        public void ChangeProxy()
        {
            //((FirefoxDriver)driver)
        }

        
    }
}
