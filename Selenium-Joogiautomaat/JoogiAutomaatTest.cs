using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Selenium_Joogiautomaat
{
    [TestFixture]
    public class Tests
    {
        private IWebDriver driver;


        [SetUp]
        public void SetUp()
        {
            var geckoDriverPath = @"C:\Users\mixus";
            var driverService = FirefoxDriverService.CreateDefaultService(geckoDriverPath, "geckodriver.exe");
            driver = new FirefoxDriver(driverService);
            driver.Url = "https://annabeljakubel22.thkit.ee/veebirakendused/Kohviautomaat/index.php?page=haldus";
        }

        [Test]
        public void FindDrinkInputElement()
        {
            IWebElement drinkInputBox = driver.FindElement(By.XPath(".//*[@id='jooginimi']"));
            string randomString = "Selenium" + new Random().Next(1, 100);

            drinkInputBox.SendKeys(randomString);
            string enteredText = drinkInputBox.GetAttribute("value");

            Assert.That(enteredText, Is.EqualTo(randomString));        
        }

        [Test]
        public void FindCupAmountInputElement()
        {
            IWebElement cupInputBox = driver.FindElement(By.XPath(".//*[@id='topsepakis']"));
            string randomNumberAsString = new Random().Next(1, 100).ToString();

            cupInputBox.SendKeys(randomNumberAsString);
            string enteredText = cupInputBox.GetAttribute("value");

            Assert.That(enteredText, Is.EqualTo(randomNumberAsString));
        }

        [Test]
        public void ClickAddDrinkButtonSuccessfully() 
        {

            IWebElement drinkInputBox = driver.FindElement(By.XPath(".//*[@id='jooginimi']"));
            string randomString = "Selenium" + new Random().Next(1, 100);

            IWebElement cupInputBox = driver.FindElement(By.XPath(".//*[@id='topsepakis']"));
            string randomNumberAsString = new Random().Next(1, 100).ToString();

            IWebElement addDrinkButton = driver.FindElement(By.XPath("/html/body/div[3]/form/div[3]/button"));

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int initialChildCount = driver.FindElements(By.XPath(".//div[@class='inventoryForm']/div")).Count;

            drinkInputBox.SendKeys(randomString);
            cupInputBox.SendKeys(randomNumberAsString);
            addDrinkButton.Click();

            wait.Until(driver =>
            {
                int currentChildCount = driver.FindElements(By.XPath("//div[@class='inventoryForm']/div")).Count;
                return currentChildCount > initialChildCount;
            });

            IWebElement lastChildDiv = driver.FindElement(By.XPath("//div[@class='inventoryForm']/div[last()]"));

            Assert.IsTrue(lastChildDiv.Text.Contains(randomString));
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
    }
}