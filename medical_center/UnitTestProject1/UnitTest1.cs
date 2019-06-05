using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MED.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private IWebDriver driver;

        [TestMethod]
        public void Login()
        {
            string url = "http://localhost:59366/Account/Login";
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.Id("Email")).SendKeys("ariadna@mail.com");
            driver.FindElement(By.Id("Password")).SendKeys("Test123!");
            driver.FindElement(By.Id("LogIn")).Submit();

            var message =
                driver.FindElement(By.XPath("//*[@id='logoutForm']/ul/li[1]/a"));

            driver.Close();
        }

        [TestMethod]
        public void InvalidLogin()
        {
            string url = "http://localhost:59366/Account/Login";
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);

            driver.FindElement(By.Id("Email")).SendKeys("ariadna@mail.com");
            driver.FindElement(By.Id("Password")).SendKeys("Tesst123!");
            driver.FindElement(By.Id("LogIn")).Submit();

            var errorMessage =
                driver.FindElement(By.XPath("//*[@id='colorlib-footer']/div[2]/div/div[1]/ul/li"));

            driver.Close();
        }


    }
}
