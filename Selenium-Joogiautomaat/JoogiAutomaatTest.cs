using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Selenium_Joogiautomaat
{
    [TestFixture]
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait? wait;


        [SetUp]
        public void SetUp()
        {
            var geckoDriverPath = @"C:\Users\mixus";
            var driverService = FirefoxDriverService.CreateDefaultService(geckoDriverPath, "geckodriver.exe");
            driver = new FirefoxDriver(driverService);
            driver.Url = "https://annabeljakubel22.thkit.ee/veebirakendused/Kohviautomaat/index.php?page=haldus";
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void FindDrinkInputElement()
        {
            IWebElement drinkInputBox = driver.FindElement(By.XPath(".//*[@id='jooginimi']"));
            string randomString = "Selenium" + new Random().Next(1, 100);

            drinkInputBox.SendKeys(randomString);
            Thread.Sleep(1000);
            string enteredText = drinkInputBox.GetAttribute("value");

            Assert.That(enteredText, Is.EqualTo(randomString));        
        }

        [Test]
        public void FindCupAmountInputElement()
        {
            IWebElement cupInputBox = driver.FindElement(By.XPath(".//*[@id='topsepakis']"));
            string randomNumberAsString = new Random().Next(1, 100).ToString();

            cupInputBox.SendKeys(randomNumberAsString);
            Thread.Sleep(1000);
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

            int initialDrinkCount = driver.FindElements(By.XPath(".//div[@class='inventoryForm']/div")).Count;

            drinkInputBox.SendKeys(randomString);
            cupInputBox.SendKeys(randomNumberAsString);
            Thread.Sleep(1000);
            addDrinkButton.Click();

            wait.Until(driver =>
            {
                int currentDrinkCount = driver.FindElements(By.XPath("//div[@class='inventoryForm']/div")).Count;
                return currentDrinkCount > initialDrinkCount;
            });

            IWebElement lastDrinkDiv = driver.FindElement(By.XPath("//div[@class='inventoryForm']/div[last()]"));

            Assert.IsTrue(lastDrinkDiv.Text.Contains(randomString));
        }

        [Test]
        public void FillLastDrink() 
        {
            IWebElement lastDrink = driver.FindElement(By.XPath("//div[@class='inventoryForm']/div[last()]"));
            IWebElement valueElement = lastDrink.FindElement(By.XPath(".//div[@class='col-md-2 larger-text']"));
            string initialValue = valueElement.Text.Split(' ')[1];

            IWebElement fillButton = lastDrink.FindElement(By.XPath(".//button[@name='eventTaida']"));
            fillButton.Click();

            wait.Until(driver =>
            {
                IWebElement updatedLastDrink = driver.FindElement(By.XPath("//div[@class='inventoryForm']/div[last()]"));
                string updatedValue = updatedLastDrink.FindElement(By.XPath(".//div[@class='col-md-2 larger-text']")).Text.Split(' ')[1];

                return updatedValue != initialValue;
            });

            IWebElement lastDrinkAfterFill = driver.FindElement(By.XPath("//div[@class='inventoryForm']/div[last()]"));
            string newValue = lastDrinkAfterFill.FindElement(By.XPath(".//div[@class='col-md-2 larger-text']")).Text.Split(' ')[1];

            Console.WriteLine($"Initial Value: {initialValue}");
            Console.WriteLine($"New Value: {newValue}");

            Assert.Greater(int.Parse(newValue), int.Parse(initialValue));
        }

        [Test]
        public void DeleteLastDrink()
        {
            IWebElement lastDrink = driver.FindElement(By.XPath("//div[@class='inventoryForm']/div[last()]"));
            string initialText = lastDrink.Text;

            int initialDrinkCount = driver.FindElements(By.XPath("//div[@class='inventoryForm']/div")).Count;

            IWebElement deleteButton = lastDrink.FindElement(By.XPath(".//button[@name='eventKustuta']"));
            deleteButton.Click();

            wait.Until(driver =>
            {
                int currentDrinkCount = driver.FindElements(By.XPath("//div[@class='inventoryForm']/div")).Count;
                return currentDrinkCount < initialDrinkCount;
            });

            IWebElement newLastDrink = driver.FindElement(By.XPath("//div[@class='inventoryForm']/div[last()]"));


            Assert.That(newLastDrink.Text, Is.Not.EqualTo(initialText));
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
