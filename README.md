SnipeIt Demo Automation Script Assignment states:
The automation script should follow these steps:
1. Login to the snipeit demo at https://demo.snipeitapp.com/login
2. Create a new Macbook Pro 13" asset with the ready to deploy status and checked out to a random user
3. Find the asset you just created in the assets list to verify it was created successfully
4. Navigate to the asset page from the list and validate relevant details from the asset creation
5. Validate the details in the "History" tab on the asset page

Project Description
Used NUnit framework, .NET 10.0 and the latest version(v1.63.0) of Microsoft Playwright.
Project name: PlaywrightDemoTest
Automation Script Test File name: DemoTest.cs

Installation involved:
.NET 10.0 SDK and 
Git (to clone the repository)

Steps to run the script:
1. Clone the repository:
   git clone https://github.com/ManishaSinghal308/PlaywrightDemoTest.git
2. cd SnipeItDemoTests
3. Add Playwright package to your project:
   dotnet add package Microsoft.Playwright.MSTest
4. Build the project so the playwright.ps1 is available inside the bin directory:
   dotnet build
5. Install Playwright browsers:
   cd bin/Debug/net10.0/playwright.ps1 install
   cd.. to SnipeItDemoTests
6. Execute the following command to run the automation script:
   dotnet test
