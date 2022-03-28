# Introduction 
You wanna record a sound and play it on a different phone with notification enable?
Here we go ;-)

# Getting Started
1.	Go to "appsettings.json" of the project "CherieAppBankSound" and add a SQL Connection String
2.	"Add-Migration Init" in the Package Manager Console
3.	"Update-Database"
4.	Deploy the "CherieAppBankSound" to an Azure Web App.
5.	Get its URL.
6.	Replace it in "CherieAppUploadZik" and "MAUICherieApp" appsettings.json
7.	Deploy the Sound Maker on a phone (=> CherieAppUploadZik).
8.	Deploy the Sound Listerner on a phone (=> MAUICherieApp).
9.	Enjoy making sounds and play it on another phone.
10.	[Facultative] Just use "debug-wifi.ps1" to deploy them wirelessly.

- [Maui](https://docs.microsoft.com/en-us/dotnet/maui/what-is-maui)