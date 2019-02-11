using AppiumDotNetSamples.Helper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace AppiumDotNetSamples
{
    [TestFixture()]
    public class MainPageTest
    {
        private AndroidDriver<AndroidElement> driver;
        private WebDriverWait wait;

        [SetUp()]
        public void BeforeAll()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability("device", "Android");
            capabilities.SetCapability("deviceName", "Android");
            capabilities.SetCapability("platformName", "Android");
            capabilities.SetCapability("testdroid_apiKey", "11r25DpLM2g6Ek0hn751HGkJPbC1S71s");
            capabilities.SetCapability("testdroid_target", "Android");
            capabilities.SetCapability("testdroid_project", "Appium cource");
            capabilities.SetCapability("testdroid_testrun", "Android Run 1");
            capabilities.SetCapability("testdroid_device", "Google Pixel 9.0 -US");
            capabilities.SetCapability("testdroid_app", "ae928e7d-5014-44f9-9405-3ba596e9f99b/Ali.apk");

            driver = new AndroidDriver<AndroidElement>(Env.ServerUri(), capabilities, Env.INIT_TIMEOUT_SEC);
            driver.Manage().Timeouts().ImplicitWait = Env.IMPLICIT_TIMEOUT_SEC;
            driver.StartActivity("com.alibaba.aliexpresshd", "com.aliexpress.module.home.MainActivity");
            wait = new WebDriverWait(driver, Env.IMPLICIT_TIMEOUT_SEC);
        }

        [TearDown()]
        public void AfterAll()
        {
            driver.Quit();
        }

        [Test(Description = "Т-7. Открытие бокового меню свайпом")]
        [TestCase(TestName = "Т-7. Открытие бокового меню свайпом")]
        public void OpedSideMenuWithSwipeTest()
        {
            int start_x; int end_x;
            int start_y; int end_y;
            int duration = 1000;

            Thread.Sleep(duration);
            driver.Swipe(start_x = 1, start_y = 400, end_x = 700, end_y = 400, duration);
            wait.Until(ExpectedConditions.ElementExists(By.Id("com.alibaba.aliexpresshd:id/navdrawer_items_list")));

            driver.Swipe(start_x = 700, start_y = 400, end_x = 1, end_y = 400, duration);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("com.alibaba.aliexpresshd:id/navdrawer_items_list")));
        }

        [Test(Description = "Т-8. Открытие бокового меню кнопкой")]
        public void OpedSideMenuWithButtonTest()
        {
            int duration = 1000;

            AndroidElement menuButton = driver.FindElementById("com.alibaba.aliexpresshd:id/left_action");
            menuButton.Click();
            wait.Until(ExpectedConditions.ElementExists(By.Id("com.alibaba.aliexpresshd:id/navdrawer_items_list")));

            TouchAction action = new TouchAction(driver);
            action.Tap(719, 400).Perform();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("com.alibaba.aliexpresshd:id/navdrawer_items_list")));
        }

        [Test(Description = "Т-9. Выполнение поиска через кнопку клавиатуры")]
        public void SearchTest()
        {
            int duration = 1000;
            string text_to_search = "RDA";

            Thread.Sleep(duration);

            AndroidElement searchMenuButton = driver.FindElementById("com.alibaba.aliexpresshd:id/search_hint");
            searchMenuButton.Click();

            AndroidElement searchField = driver.FindElementByAccessibilityId("Поисковый запрос");
            searchField.SendKeys(text_to_search);
            searchField.Click();
            driver.PressKeyCode(AndroidKeyCode.Enter);
            wait.Until(ExpectedConditions.ElementExists(By.Id("com.alibaba.aliexpresshd:id/search_result_list")));
        }

        [Test(Description = "Т - 11 Очистка памяти приложения")]
        public void CrearAppMemoryTest()
        {
            AndroidElement menuButton = driver.FindElementById("com.alibaba.aliexpresshd:id/left_action");
            menuButton.Click();
            wait.Until(ExpectedConditions.ElementExists(By.Id("com.alibaba.aliexpresshd:id/navdrawer_items_list")));
            AndroidElement settingsMenuButton = driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/" +
                "android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/" +
                "android.support.v4.widget.DrawerLayout/android.widget.ScrollView/android.widget.LinearLayout/" +
                "android.widget.LinearLayout/android.widget.LinearLayout/android.widget.RelativeLayout[9]/" +
                "android.widget.TextView");
            wait.Until(ExpectedConditions.ElementToBeClickable(settingsMenuButton));
            settingsMenuButton.Click();

            wait.Until(ExpectedConditions.ElementExists(By.Id("com.alibaba.aliexpresshd:id/rl_clear_memory_settings")));
            AndroidElement clearMemoryButton = driver.FindElementById("com.alibaba.aliexpresshd:id/rl_clear_memory_settings");
            clearMemoryButton.Click();

            string popupText = driver.FindElementByXPath("/hierarchy/android.widget.Toast").Text;
            Assert.IsTrue(popupText == "Память приложения очищена.", "Текст сообщения - '" + popupText + "' отличается от ожидаемого");
        }

        [Test(Description = "Т - 13 Закрытие приложения")]
        public void CloseAppTest()
        {
            string activity = driver.CurrentActivity;
            driver.PressKeyCode(AndroidKeyCode.Back);
            Assert.IsTrue(activity != driver.CurrentActivity,"Приложение не закрылось");
        }

    }
}
