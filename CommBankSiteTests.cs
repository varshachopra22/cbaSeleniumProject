using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using Xunit;

namespace SeleniumProject
{
    public class CommBankSiteTests
    {
        [Fact]
        public void WhenNavigatingToCommBankUrl_ThenItShouldGotoCommBankHomePage()
        {
            var browser = CreateBrowser();

            browser.Navigate().GoToUrl("https://www.commbank.com.au");

            browser.EnsureElementExists(By.ClassName("commbank-logo"));
        }

        [Fact]
        public void WhenClickingOnTravelProducts_ThenItShouldHaveDiscoverMoreLinkOnTravelMoneyCardSection()
        {
            var browser = CreateBrowser();

            browser.Navigate().GoToUrl("https://www.commbank.com.au");

            browser.EnsureElementExists(By.ClassName("commbank-logo"));

            browser.ScrollTo(By.XPath("//*[@id='products']/div/div/div[1]/div[8]/div/a/div[1]/img"));

            browser.Click(By.XPath("//*[@id='products']/div/div/div[1]/div[8]/div/a/div[1]/img"));
        }

        [Fact]
        public void WhenClickingOnLogonButton_ThenItShouldOpenNetBankLoginPageWithUserNameAndPassword()
        {
            var browser = CreateBrowser();

            browser.Navigate().GoToUrl("https://www.commbank.com.au");

            browser.EnsureElementExists(By.ClassName("commbank-logo"));

            browser.Click(By.ClassName("logged-state-button"));

            //browser.SwitchTo().ActiveElement();
            string winHandle = browser.CurrentWindowHandle;
            
            browser.SwitchTo().Window(winHandle);           

            browser.EnsureElementExists(By.Id("txtMyClientNumber_field"));
            
            browser.EnsureElementExists(By.Id("txtMyPassword_field"));
        }

        private RemoteWebDriver CreateBrowser()
        {
            var options = new ChromeOptions();
            options.AddArgument("--incognito");
            options.AddArgument("--verbose");
            options.AddArgument("--window-size=1920,1080");

            var browser = new ChromeDriver(options);

            return browser;
        }
    }
}