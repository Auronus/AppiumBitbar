using AppiumDotNetSamples.Helper;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;

namespace AppiumDotNetSamples
{
    [TestFixture()]
    public class AuthorizationPageTest
    {
        private AndroidDriver<AndroidElement> driver;
        private WebDriverWait wait;

        [SetUp()]
        public void BeforeAll()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(MobileCapabilityType.BrowserName, "");
            capabilities.SetCapability(MobileCapabilityType.PlatformName, App.AndroidDeviceName());
            capabilities.SetCapability(MobileCapabilityType.PlatformVersion, App.AndroidPlatformVersion());
            capabilities.SetCapability(MobileCapabilityType.AutomationName, "UIAutomator2");
            capabilities.SetCapability(MobileCapabilityType.DeviceName, "Nokia");
            capabilities.SetCapability(MobileCapabilityType.App, App.AndroidApp());

            driver = new AndroidDriver<AndroidElement>(Env.ServerUri(), capabilities, Env.INIT_TIMEOUT_SEC);
            driver.Manage().Timeouts().ImplicitWait = Env.IMPLICIT_TIMEOUT_SEC;
            wait = new WebDriverWait(driver, Env.IMPLICIT_TIMEOUT_SEC);

            AndroidElement menuButton = driver.FindElementById("com.alibaba.aliexpresshd:id/left_action");
            menuButton.Click();

            AndroidElement authorizationMenuButton = driver.FindElementById("com.alibaba.aliexpresshd:id/chosen_account_content_view");
            authorizationMenuButton.Click();

            AndroidElement loginMenuButton = driver.FindElementById("com.alibaba.aliexpresshd:id/btn_sign_in");
            loginMenuButton.Click();

            AndroidElement list = driver.FindElementById("android:id/autofill_dataset_list");
            TouchAction action = new TouchAction(driver);
            action.Press(300, 100).Release().Perform();
        }

        [TearDown()]
        public void AfterAll()
        {
            driver.Quit();
        }

        [Test(Description = "Т-1. Неуспешная авторизация")]
        public void Authorization_NegativeTest()
        {
            string login = "tgdj@gmail.com";
            string password = "qqqqqqq";
            string errorMessageText="";

            wait.Until(ExpectedConditions.ElementExists(By.Id("com.alibaba.aliexpresshd:id/et_email")));
            AndroidElement emailField = driver.FindElementById("com.alibaba.aliexpresshd:id/et_email");
            emailField.SendKeys(login);

            AndroidElement passwordField = driver.FindElementById("com.alibaba.aliexpresshd:id/et_password");
            passwordField.SendKeys(password);

            AndroidElement loginButton = driver.FindElementById("com.alibaba.aliexpresshd:id/tv_signin_btn_label");
            loginButton.Click();

            try
            {
                AndroidElement errorMessage = driver.FindElementById("com.alibaba.aliexpresshd:id/textinput_error");
                errorMessageText = errorMessage.Text;
            }
            catch (Exception)
            {
                throw new Exception("Элемент с сообщением об ошибке не представлен на странице");
            }

            Assert.IsTrue(errorMessageText == "???", "Текст сообщения об ошибке - '" + errorMessageText + "' отличается от ожидаемого" +
                "???");
        }
    }
}
