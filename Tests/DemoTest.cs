using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace SnipeIT.Automation.Tests;

public class DemoTest
{
    private IPage page;
    private IBrowser browser;
    private IBrowserContext context;

    [SetUp]
    public async Task Setup()
    {
        var playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        context = await browser.NewContextAsync();
        page = await context.NewPageAsync();
        
    }

    [TearDown]
    public async Task TearDown()
    {
        await browser.CloseAsync();
    }

    [Test]
    public async Task Create_Validate_Asset()
    {
        string modelName = "Macbook Pro 13";
        string status = "Ready to Deploy";

        // Login
        await page.GotoAsync("https://demo.snipeitapp.com/login");
        await page.FillAsync("#username", "admin");
        await page.FillAsync("#password-field", "password");
        await page.ClickAsync("button[type='submit']");
        var title = await page.TitleAsync();
        Assert.That(title, Does.Contain("Dashboard"));
        Console.WriteLine("Successfully logged in!");


        Console.WriteLine("Creating Asset!");
        // Create Asset
        await page.ClickAsync("a.dropdown-toggle:has-text('Create New')");
        await page.ClickAsync("a[href='https://demo.snipeitapp.com/hardware/create']");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        string assetTag = await page.Locator("#asset_tag").InputValueAsync();
        //Wait for the page load
        await page.WaitForTimeoutAsync(3000);
        
       // 1. Locate the model field
        var modelSelect = page.Locator("#select2-model_select_id-container");
        
        // 2. Click to open the dropdown
        await modelSelect.ClickAsync();
        
        // 3. Wait for the search box
        var searchBox = page.Locator(".select2-search__field");
        
        // 4. Type the model name
        await searchBox.FillAsync(modelName);
        
        // 5. Select the matching option
        await page.Locator(".select2-results__option", new()
        {
            HasText = modelName
        }).ClickAsync();
        
        // 6. Wait for dropdown to close
        await page.Locator(".select2-results__options")
                   .WaitForAsync(new() { State = WaitForSelectorState.Detached });

       
        // 1. Locate the Status field
        var statusSelect = page.Locator("#select2-status_select_id-container");
       
        // 2. Click to open the dropdown
        await statusSelect.ClickAsync();
       
        // 4. Wait for the status search box to appear
        var searchBoxStatus = page.Locator(".select2-search__field");
       
        // 5. Type the status name
        await searchBox.FillAsync(status);
       
        // 6. Select the matching option
        await page.Locator(".select2-results__option", new()
        {
            HasText = status
        }).ClickAsync();
       
        // 7. Wait for dropdown to close
        await page.Locator(".select2-results__options")
                   .WaitForAsync(new() { State = WaitForSelectorState.Detached });

       
       //Random user selection            
       // 1. Select the "User" radio button
        var userRadio = page.Locator("input[name='checkout_to_type'][value='user']");
        await userRadio.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await userRadio.CheckAsync();   // Playwright auto-waits for enabled

        // 2. Open the dropdown
        var select2Container = page.Locator("#select2-assigned_user_select-container");
        await select2Container.WaitForAsync(new() { State = WaitForSelectorState.Visible });
        await select2Container.ClickAsync();

        // 3. Wait for the search box to appear
        var searchBoxUser = page.Locator(".select2-search__field");
        await searchBox.WaitForAsync(new() { State = WaitForSelectorState.Visible });

        // 4. Trigger loading of all users
        await searchBoxUser.FillAsync("");   // Shows full list

        // 5. Get all user options from the results
        var options = page.Locator(".select2-results__option");
        int count = await options.CountAsync();
        //Throw exception if no user is found
        if (count == 0)
            throw new Exception("No users found in Select2 dropdown.");

        // Step 6 — Pick a random user
        var random = new Random();
        int index = random.Next(count);
        string randomUser = await options.Nth(index).InnerTextAsync();

        // Step 7 — Select the random user
        await options.Nth(index).ClickAsync();
        
        // Step 8 — Wait for dropdown to close
        await page.Locator(".select2-results__options")
                   .WaitForAsync(new() { State = WaitForSelectorState.Detached });
        
        //Click on the "Save" button
        await page.ClickAsync("text=Save");
        //Wait until asset is created
        await page.WaitForTimeoutAsync(3000);
        Console.WriteLine("Asset created!");
    
        // Verify asset created in list
        Console.WriteLine("Verifying asset in the list!");
        await page.ClickAsync("text=Assets");
        //Search the asset
        await page.FillAsync("//input[@type='search']", assetTag);
        await page.WaitForTimeoutAsync(3000);
        await page.Keyboard.PressAsync("Enter"); 
        await page.WaitForTimeoutAsync(3000);
        if (await page.Locator($"//mark[text()='{assetTag}']").IsVisibleAsync()) //verify Asset Tag 
        {
            Console.WriteLine("Asset verified!");

        }
        else
        {
            Console.WriteLine("Asset not found!");
            return;
        }
        
        //Model details validation
        if (await page.Locator($"//div[@class='col-md-12' and contains(normalize-space(), '{modelName}')]").IsVisibleAsync()) // validates model
        {
            Console.WriteLine("Model name validated!");
        }
        
        //Status validation
        if (await page.Locator($"//div[@class='col-md-12' and contains(normalize-space(), '{status}')]").IsVisibleAsync()) //validates status
        {
            Console.WriteLine("Status validated!");
        }
        await page.WaitForTimeoutAsync(3000);

       //Validating details in the history tab
       //Navigate to the history tab
        await page.ClickAsync($"//mark[text()='{assetTag}']");
        await page.Locator("a[href='#history']").ClickAsync();
        await page.WaitForTimeoutAsync(3000);
        //Checks if "checkout" is visible
        if (await page.Locator($"//td[text()='checkout']").IsVisibleAsync())
        {
            Console.WriteLine("History validated!");
        }
       
    }
}
