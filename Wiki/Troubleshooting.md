
### **Potential issues**

  

**1. The app shows the error message 'You are not authorized to access this App. Please contact administrator.'**

  

- Please verify that the account you are using in Microsoft Teams is of the same tenant.

  

- Check whether tenant ID mentioned in the application settings section of the configuration menu for bot app service matches your Tenant ID. Ref: [https://docs.microsoft.com/en-us/azure/app-service/configure-common](https://docs.microsoft.com/en-us/azure/app-service/configure-common)

  

![Checking application settings in app service](https://docs.microsoft.com/en-us/azure/app-service/media/configure-common/open-ui.png)

**2. The app is currently not supported on iOS devices with Azure Front Door. Please use custom domains while deployment for the app to work on iOS devices.**  

### **Problems while deploying to Azure**

  

**1. Error when attempting to reuse a Microsoft Azure Active Directory application ID for the bot registration**

  

Bot is not valid. Errors: MsaAppId is already in use.

  

- Creating the resource of type Microsoft.BotService/botServices failed with status "BadRequest"

  

This happens when the Microsoft Azure application ID entered during the setup of the deployment has already been used and registered for a bot.

  

**Fix:**

  

Either register a new Microsoft Azure Active Directory application or delete the bot registration that is currently using the attempted Microsoft Azure application ID.

  
  

### **Problems while customizing bot**

  

**1. Client app packages restoration failure for first build**

  

Once the repository is cloned and you try to build the solution in Visual Studio, build fails due to multiple NPM packages fails to install.

  

**Fix:**

  

If you have cloned using Visual Studio, please retry by building the solution after closing and re-opening the Visual Studio.

  

If you still get errors, navigate to the 'ClientApp' folder in the project directory using file explorer. Open a command prompt at this location and type 'npm install'. This will install the app the packages for the client app. Once it is done, try to rebuild the solution from Visual Studio.

  

## Problems in bot

  

If facing any issues related to bot/Messaging Extension.

  

**Fix**

  

Please go to app-insights and check for errors.

  

- Go to [azure portal](http://portal.azure.com/)

- Go to App-insights related to your app.

- Open Logs (Analytics)

- Select Time Range & fire the query from different tables like exceptions, customEvent etc. like below:

  

**Didn't find your problem here?**

  

Please, report the issue [here](https://github.com/OfficeDev/microsoft-teams-apps-employeetraining/issues/new)