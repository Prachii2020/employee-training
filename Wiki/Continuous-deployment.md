
Continuous Deployment service in Azure App Services offers the ability to merge the latest code changes to an Azure-hosted environment from GitHub. It helps to seamlessly update the Azure services without the need for a new deployment.

**Continuous deployment in Azure App Services**

Please follow below steps to deploy the latest changes to the app service:

- Log in to the Azure Portal for your subscription.

- Select App Services from left menu blade
[[https://github.com/OfficeDev/microsoft-teams-apps-employeetraining/wiki/Images/Azure-appservice-menu.png|Azure app service menu blade]]

- Search and select the app service name (search for the base resource name) which is created during the first deployment. For e.g. employeetraining.azurewebsites.net

- Select Deployment Center under menu blade
[[https://github.com/OfficeDev/microsoft-teams-apps-employeetraining/wiki/Images/Deployment-center.png|Deployment center in Azure app service]]

- Click on Sync to synchronize the latest bits from the GitHub master branch
[[https://github.com/OfficeDev/microsoft-teams-apps-employeetraining/wiki/Images/sync-github.png|Sync GitHub deployment]]

Note: please make sure that the Repository name is pointing to the correct OfficeDev repo git path.

- Wait for sync operation's success response.