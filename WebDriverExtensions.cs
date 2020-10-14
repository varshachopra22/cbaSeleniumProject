using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace SeleniumProject
{
    public static class WebDriverExtensions
    {
        public static void InputText(this IWebDriver driver, By element, string text)
        {
            driver.EnsureElementExists(element);
            driver.ScrollTo(element);
            driver.EnsureElementIsVisible(element);
            driver.FindElement(element).Clear();
            driver.FindElement(element).SendKeys(text);
        }

        public static void SelectTodaysDate(this IWebDriver driver, By element)
        {
            driver.EnsureElementExists(element);
            driver.ScrollTo(element);
            driver.Click(element);
            Thread.Sleep(200);  // This sleep seems to make a difference, oddly enough the 'ensure element exists' fails intermittently without it
            driver.EnsureElementExists(By.ClassName("datepicker"));
            driver.Click(By.ClassName("today"));
        }

        public static void SelectFromDropDown(this IWebDriver driver, By element, string value)
        {
            driver.EnsureElementExists(element);
            driver.ScrollTo(element);

            var webElement = driver.FindElement(element);
            var selectElement = new SelectElement(webElement);
            try
            {
                selectElement.SelectByValue(value);
            }
            catch (NoSuchElementException)
            {
                selectElement.SelectByText(value);
            }
        }

        public static void Click(this IWebDriver driver, By element, bool scrollToElement = true)
        {
            driver.EnsureElementExists(element);
            if (scrollToElement)
            {
                driver.ScrollTo(element);
            }
            driver.EnsureElementIsClickable(element);
            driver.FindElement(element).Click();
        }

        public static void Hover(this IWebDriver driver, By element)
        {
            driver.EnsureElementExists(element);

            var actions = new Actions(driver);
            actions.MoveToElement(driver.FindElement(element)).Perform();
        }

        public static void ScrollTo(this IWebDriver driver, By element)
        {
            var webElement = driver.FindElement(element);
            var javaScriptExecutor = (IJavaScriptExecutor)driver;
            javaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView();", webElement);
        }

        public static void Navigate(this IWebDriver driver, string url, By elementInPage, int timeoutMilliseconds = 15000)
        {
            driver.Navigate().GoToUrl(url);
            var elementExists = WaitForElementToExist(driver, elementInPage, timeoutMilliseconds);
            var errorMessage = $"Failed to navigate to {url}, because the element({elementInPage}) is not found after {timeoutMilliseconds}ms";
            Assert.True(elementExists, errorMessage);
        }

        public static void EnsureElementExists(this IWebDriver driver, By element, int timeoutMilliseconds = 15000)
        {
            var elementExists = driver.WaitForElementToExist(element, timeoutMilliseconds);
            var errorMessage = $"The element({element}) is not found after {timeoutMilliseconds}ms";
            Assert.True(elementExists, errorMessage);
        }

        public static void EnsureElementIsVisible(this IWebDriver driver, By element, int timeoutMilliseconds = 15000)
        {
            var elementIsVisible = driver.WaitForElementToBeVisible(element, timeoutMilliseconds);
            var errorMessage = $"The element({element}) is not was not visible after {timeoutMilliseconds}ms";
            Assert.True(elementIsVisible, errorMessage);
        }

        public static void EnsureElementIsClickable(this IWebDriver driver, By element, int timeoutMilliseconds = 15000)
        {
            var elementIsVisible = driver.WaitForElementToBeClickable(element, timeoutMilliseconds);
            var errorMessage = $"The element({element}) is not clickable after {timeoutMilliseconds}ms";
            Assert.True(elementIsVisible, errorMessage);
        }

        public static void EnsureElementContainsText(this IWebDriver driver, By element, string text, int timeoutMilliseconds = 15000)
        {
            var elementContainsText = driver.WaitForElementToContainText(element, text, timeoutMilliseconds);
            var errorMessage = $"The element({element}) does not contain \"{text}\" after {timeoutMilliseconds}ms";
            Assert.True(elementContainsText, errorMessage);
        }

        private static bool WaitForElementToExist(this IWebDriver driver, By element, int timeoutMilliseconds)
        {
            return driver.WaitForElement(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(element), timeoutMilliseconds);
        }

        private static bool WaitForElementToBeVisible(this IWebDriver driver, By element, int timeoutMilliseconds)
        {
            return driver.WaitForElement(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(element), timeoutMilliseconds);
        }

        private static bool WaitForElementToBeClickable(this IWebDriver driver, By element, int timeoutMilliseconds)
        {
            return driver.WaitForElement(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element), timeoutMilliseconds);
        }

        private static bool WaitForElementToContainText(this IWebDriver driver, By element, string text, int timeoutMilliseconds)
        {
            return driver.Wait(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElementLocated(element, text), timeoutMilliseconds);
        }

        public static bool WaitForElement(this IWebDriver driver, Func<IWebDriver, IWebElement> waitCondition, int timeoutMilliseconds = 15000)
        {
            return driver.Wait(waitCondition, timeoutMilliseconds);
        }

        public static bool Wait<TConditionResult>(this IWebDriver driver, Func<IWebDriver, TConditionResult> waitCondition, int timeoutMilliseconds = 15000)
        {
            try
            {
                var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 0, 0, timeoutMilliseconds));
                wait.Until(waitCondition);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static void SendKeys(this IWebDriver driver, By elememnt, string text)
        {
            driver.FindElement(elememnt).SendKeys(text);
        }

        public static void AcceptAlert(this IWebDriver driver)
        {
            var alert = driver.SwitchTo().Alert();
            alert.Accept();
        }

        public static string GetSelectedTextFromDropdown(this IWebDriver driver, By element)
        {
            driver.EnsureElementExists(element);
            var dropdownElement = driver.FindElement(element);
            var selectedValue = new SelectElement(dropdownElement);
            return selectedValue.SelectedOption.Text;
        }

        public static IWebElement GetParent(this IWebDriver driver, By element)
        {
            driver.EnsureElementExists(element);
            return driver.FindElement(element).FindElement(By.XPath("parent::*"));
        }
    }
}