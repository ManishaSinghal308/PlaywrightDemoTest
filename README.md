
PlaywrightDemoTest
Used NUnit framework, .NET 10.0 and the latest version(v1.63.0) of Microsoft Playwright.

Installation involved:
.NET 10.0 SDK and 
Git (to clone the repository)

Clone the repository:
git clone https://github.com/ManishaSinghal308/PlaywrightDemoTest.git
cd SnipeItDemoTests
Add Playwright package to your project:
dotnet add package Microsoft.Playwright.MSTest

Build the project so the playwright.ps1 is available inside the bin directory:
dotnet build

Install Playwright browsers:
cd bin/Debug/net10.0/playwright.ps1 install
cd.. to SnipeItDemoTests

Execute the following command to run the automation script:
dotnet test
