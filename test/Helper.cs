using System;

namespace AppiumDotNetSamples.Helper
{
    public static class Env
    {
        public static String rootDirectory = System.IO.Path.GetFullPath($"{System.AppDomain.CurrentDomain.BaseDirectory.ToString()}/../../../..");

        static public Uri ServerUri()
        {
            String sauceUserName = Environment.GetEnvironmentVariable("SAUCE_USERNAME");
            String sauceAccessKey = Environment.GetEnvironmentVariable("SAUCE_ACCESS_KEY");

            return (sauceUserName == null) || (sauceAccessKey == null)
                ? new Uri("http://localhost:4723/wd/hub")
                : new Uri($"https://{sauceUserName}:{sauceAccessKey}@ondemand.saucelabs.com:80/wd/hub");
        }

        public static TimeSpan INIT_TIMEOUT_SEC = TimeSpan.FromSeconds(180);
        public static TimeSpan IMPLICIT_TIMEOUT_SEC = TimeSpan.FromSeconds(10);
    }

    public static class App
    {
        static public String AndroidApp()
        {
            return $"{Env.rootDirectory}/Ali.apk";
        }

        static public String AndroidDeviceName()
        {
            return Environment.GetEnvironmentVariable("ANDROID_DEVICE_VERSION") ?? "Android";
        }

        static public String AndroidPlatformVersion()
        {
            return Environment.GetEnvironmentVariable("ANDROID_PLATFORM_VERSION") ?? "9";
        }
    }
}
