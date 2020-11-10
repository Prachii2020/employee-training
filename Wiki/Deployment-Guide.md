
## Prerequisites

To begin, you will need:  

 - An Azure subscription where you can create the following kind of resources:

 	- App Service 

 	- App Service plan

  	- Bot Channels Registration

 	- Storage account

 	- Search service

 	- Application Insights

 - A copy of the Employee Training app GitHub [repo](https://github.com/OfficeDev/microsoft-teams-apps-employeetraining)
 
## Step 1: Register Azure AD application

Register one Azure AD applications in your tenant's directory: for the bot and tab app authentication.

1. Log in to the Azure portal from your subscription, and go to the "App registrations" blade [here](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/RegisteredApps).

2. Click on "New registration", and create an Azure AD application.

3. **Name:** The name of your Teams app - if you are following the template for a default deployment, we recommend "Employee Training".

4. **Supported account types:** Select "Accounts in any organizational directory"

5. Leave the "Redirect URL" field blank.

	[[/Images/multitenant_app_creation.PNG | Multi-tenant application registration]]

6. Click on the "Register" button.

7. When the app is registered, you'll be taken to the app's "Overview" page. Copy the **Application (client) ID**; we will need it later. Verify that the "Supported account types" is set to **Multiple organizations**.

	[[/Images/MultitenantAppOverview.PNG|App regisrations page overview]]

8. On the side rail in the Manage section, navigate to the "Certificates & secrets" section. In the Client secrets section, click on "+ New client secret". Add a description for the secret and select Expires as "Never". Click "Add".

	[[/Images/multitenant_app_secret.PNG|App secret registration screen]]
 
9. Once the client secret is created, copy its **Value**, please take a note of the secret as it will be required later.

At this point you have 3 unique values:

- Application (client) ID which will be later used during ARM deployment as Bot Client id and in manifest files as `<<botid>>`

- Client secret for the bot which will be later used during ARM deployment as Bot Client secret

- Directory (tenant) ID

We recommend that you copy these values into a text file, using an application like Notepad. We will need these values later.

## Step 2: Deploy to your Azure subscription

 1. Click on the "Deploy to Azure" button below.

    [![Deploy to Azure](https://camo.githubusercontent.com/8305b5cc13691600fbda2c857999c4153bee5e43/68747470733a2f2f617a7572656465706c6f792e6e65742f6465706c6f79627574746f6e2e706e67)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FOfficeDev%2Fmicrosoft-teams-apps-employeetraining%2Fmaster%2FDeployment%2Fazuredeploy.json)

 2. When prompted, log in to your Azure subscription.

 3. Azure will create a "Custom deployment" based on the ARM template and ask you to fill in the template parameters.

 4. Select a subscription and resource group.
	- We recommend creating a new Resource group.

	- The Resource group location MUST be in a datacenter that supports: Application Insights, Storage Account, Search service. For an up-to-date list, click [here](https://azure.microsoft.com/en-us/global-infrastructure/services/?products=functions,cognitive-services,search,monitor), and select a region where the following services are available:

	- Application Insights

	- Search service

 5. Enter a "Base Resource Name", which the template uses to generate names for the other resources.

	- The app service names `[Base Resource Name]` must be available. For example, if you select `employeetraining` as the base name, the name `employeetraining` must be available (not taken); otherwise, the deployment will fail with a Conflict error.

	- Remember the base resource name that you selected. We will need it later.

 6. Fill in the various IDs in the template:

	- **Bot client ID**: The application (client) ID registered in Step 1.

	- **Bot client secret**: The client secret registered in Step 1.

	- **Tenant ID**: The tenant ID registered in Step 1. If your Microsoft Teams tenant is same as Azure subscription tenant, then we would recommend to keep the default values.
        
Make sure that the values are copied as-is, with no extra spaces. The template checks that GUIDs are exactly 36 characters.

 7. If you wish to change the app name, description, and icon from the defaults, modify the corresponding template parameters.

	> NOTE: If you plan to use a custom domain name instead of relying on Azure Front Door, read the instructions [here](https://github.com/OfficeDev/microsoft-teams-apps-employeetraining/wiki/Custom-domain-option) first. The app is not supported on iOS devices with Azure Front Door. Please use custom domain for the app to work on iOS devices.

 8. Click on "Review + create" to start the deployment. It will validate the parameters provided in the template. Once the validation is passed, click on create to start the deployment.

 9. Wait for the deployment to finish. You can check the progress of the deployment from the "Notifications" pane of the Azure Portal. It can take more than 30 minutes for the deployment to finish.

 10. After the deployment is successful, open the deployment details blade and click on "Outputs" option visible in the navigation menu on the left and note down the mentioned values below. We will need them later in the deployment process.
botId
appDomain

## Step 3: Set up authentication for the app

1. Go back to the "App Registrations" page [here](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/RegisteredApps)

2. Enter the Bot client id created in Step 1 under Owned applications search box.

3. Click on the application (this should be the same application registered in step 1)

4. Under left menu, select **Authentication** under **Manage** section.

5. Select 'Accounts in any organizational directory (Any Azure AD directory - Multitenant)' under Supported account types and click "+Add a platform".

6. On the flyout menu, Select "Web"
	[[/Images/RedirectUrlMenu.PNG|Redirect URL menu screen]]

7. Add `https://[baseresourcename].azurefd.net/signin-simple-end` under Redirect URLs and click Configure button. For e.g. https://employeetraining.azurefd.net/signin-simple-end
	[[/Images/RedirectUrlConfiguration.PNG|Redirect URL configuration]]

8. Once the flyout menu close, scroll bottom to section 'Implicit Grant' and select check boxes "Access tokens" and "ID tokens" and click "Save" at the top bar.
	[[/Images/ImplicitGrant.PNG|Access token implict grant permission section]]

9. Under left menu, select **Expose an API** under **Manage**.
	[[/Images/ExposeAnApiMenu.PNG|Expose an API menu section screen]]

10. Select the **Set** link to generate the Application ID URI in the form of `api://{BotID}`. Insert your fully qualified domain name (with a forward slash "/" appended to the end) between the double forward slashes and the GUID. The entire ID should have the form of: `api://[baseresourcename].azurefd.net/{botid}`

	- for e.g.: `api://employeetraining.azurefd.net/c6c1f32b-5e55-4997-881a-753cc1d563b7`.

11. Select the **Add a scope** button. In the panel that opens, enter `access_as_user` as the **Scope name**.

12. Set Who can consent? to "Admins and users"

13. Fill in the fields for configuring the admin and user consent prompts with values that are appropriate for the `access_as_user` scope. Suggestions:

	- **Admin consent display name:** Employee Training Admin
	- **Admin consent description:** Allows Teams to call the app’s web APIs as the current user.
	- **User consent display name:** Employee Training User
	- **User consent description:** Enable Teams to call this app’s APIs with the same rights that you have.


14. Ensure that **State** is set to **Enabled**

15. Select **Add scope**

	- Note: The domain part of the **Scope name** displayed just below the text field should automatically match the **Application ID** URI set in the previous step, with `/access_as_user` appended to the end; for example:
		- `api://employeetraining.azurefd.net/c6c1f32b-5e55-4997-881a-753cc1d563b7/access_as_user`


16. In the same page in below section **Authorized client applications**, you identify the applications that you want to authorize to your app’s web application. Each of the following IDs needs to be entered. Click "+Add a client application" and copy-paste the below id and select checkbox "Authorized scopes". Repeat the step for second GUID.

- `1fec8e78-bce4-4aaf-ab1b-5451cc387264` (Teams mobile/desktop application)
- `5e3ce6c0-2b1f-4285-8d4b-75ee78787346` (Teams web application)

17. Under left menu, navigate to **API Permissions**, and make sure to add the following permissions of Microsoft Graph API > Delegated permissions:

- offline_access
- openid
- profile
- User.ReadBasic.All 
- People.Read 
- Directory.Read.All 
- Calendars.ReadWrite

Microsoft Graph API > Application permissions
- Calendars.ReadWrite

Click on “Add Permissions” to commit your changes.

18. If you are logged in as the Global Administrator, click on the “Grant admin consent for %tenant-name%” button to grant admin consent else, inform your Admin to do the same through the portal or follow the steps provided here to create a link and sent it to your Admin for consent.

19. Global Administrator can also grant consent using following link: https://login.microsoftonline.com/common/adminconsent?client_id=<%appId%>

**Note:** The detailed guidelines for registering an application for SSO Microsoft Teams tab can be found [here](https://docs.microsoft.com/en-us/microsoftteams/platform/tabs/how-to/authentication/auth-aad-sso)

## Step 4: Create the Teams app packages

This step covers the Teams application package creation for teams scope and make it ready to install in Teams.
Create two Teams app packages: one for end-users to install personally, and one to be installed to the Learning and Development team.

1. Open the `Manifest\manifest.json` file in a text editor.

2. Change the placeholder fields in the manifest to values appropriate for your organization.

    - `developer.name` ([What's this?](https://docs.microsoft.com/en-us/microsoftteams/platform/resources/schema/manifest-schema#developer))

    - `developer.websiteUrl`

    - `developer.privacyUrl`

    - `developer.termsOfUseUrl`

3. Change the `<<botId>>` placeholder to your Azure AD application's ID from above. This is the same GUID that you entered in the template under "Bot Client ID".

4. Replace `<appbaseurl>` placeholder with the Azure front door URL created i.e.'https://[BaseResourceName].azurefd.net'. For example if you chose employeetraining then the appbaseurl will be https://employeetraining.azurefd.net.

5. In the "validDomains" section, replace the `<<appDomain>>` with your Bot App Service's domain. This will be `[BaseResourceName].azurefd.net`. For example if you chose "employeetraining" as the base name, change the placeholder to `employeetraining.azurefd.net`.

> Note : please make sure to not add https:// in valid domains.

6. In the "webApplicationInfo" section, replace the `<<clientId>>` with Client ID of the app created in Step 1. Also replace `api://<<applicationurl>>/<<clientId>>` with following Application URI appended with client id. This will be as follows for example `api://employeetraining.azurefd.net/19c1102a-fffe-46c4-9a85-016bec13e0ab` where employeetraining is the base resource URL used under valid domains and tabs and 19c1102a-fffe-46c4-9a85-016bec13e0ab is the client id.

7. Create a ZIP package with the `manifest.json`,`color.png`, and `outline.png`. The two image files are the icons for your app in Teams.

- Name this package employeetraining-enduser.zip, so you know that this is the app for end-users.
- Make sure that the 3 files are the top level of the ZIP package, with no nested folders.

[[/Images/manifest-Files-UI.PNG|Manifest files in explorer]]

8. Rename the manifest.json file to manifest_enduser.json for reusing the file.

9. Open the Manifest\manifest_lnd.json file in a text editor.

10. Repeat the steps from 2 to 6 to replace all the placeholders in the file.

11. Save and Rename manifest_lnd.json file to a file named manifest.json.

12. Create a ZIP package with the manifest.json,color.png, and outline.png. The two image files are the icons for your app in Teams.

- Name this package employeetraining-lnd.zip, so you know that this is the app for end-users.
- Make sure that the 3 files are the top level of the ZIP package, with no nested folders.

13. Rename the manifest.json file to manifest_lnd.json for reusing the file.

## Step 5: Run the apps in Microsoft Teams

1. If your tenant has side loading apps enabled, you can install your app by following the instructions [here](https://docs.microsoft.com/en-us/microsoftteams/platform/concepts/apps/apps-upload#load-your-package-into-teams)

2. You can also upload it to your tenant's app catalog, so that it can be available for everyone in your tenant to install. See [here](https://docs.microsoft.com/en-us/microsoftteams/tenant-apps-catalog-teams)
	
	- We recommend using [app permission policies](https://docs.microsoft.com/en-us/microsoftteams/teams-app-permission-policies) to restrict access to this app to the members of the experts team.

3. Install the app (the employeetraining.zip package) to your team.

# Troubleshooting
Please see our [Troubleshooting](https://github.com/OfficeDev/microsoft-teams-apps-employeetraining/wiki/Troubleshooting) page.