using OpenQA.Selenium;
using Packfortune.Tests.E2E.Pages;

public class CratesPage : BasePage
{
    private By NameField = By.CssSelector("[data-test='name']");
    private By PriceField = By.CssSelector("[data-test='price']");
    private By PictureField = By.CssSelector("[data-test='picture']");
    private By SubmitButton = By.CssSelector("[data-test='CreateCrateButton']");
    private By AddCrateButton = By.CssSelector("[data-test='AddCrateButton']");
    private By DeleteCrateButton = By.CssSelector("[data-test='DeleteCrateButton']");

    public CratesPage(IWebDriver driver) : base(driver) { }

    public void EnterName(string name)
    {
        _driver.FindElement(NameField).SendKeys(name);
    }

    public void EnterPrice(string price)
    {
        _driver.FindElement(PriceField).SendKeys(price);
    }

    public void EnterPicture(string filePath)
    {
        _driver.FindElement(PictureField).SendKeys(filePath);
    }

    public void ClickSubmit()
    {
        _driver.FindElement(SubmitButton).Click();
    }

    public void ClickAddCrateButton()
    {
        _driver.FindElement(AddCrateButton).Click();
    }

    public void ClickDeleteCrateButton()
    {
        _driver.FindElement(DeleteCrateButton).Click();
    }
}