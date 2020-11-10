

## Assumptions


The estimate below assumes:

-   5000 users in the tenant.

## [](/wiki/costestimate#sku-recommendations)SKU recommendations

The recommended SKUs for a production environment are:

-   App Service: Standard (S1)
- Search service: Basic
	- The Search service cannot be upgraded once it is provisioned, so select a tier that will meet your anticipated needs.
  

## [](/wiki/costestimate#estimated-load)Estimated load

**Data storage**: up to 2 GB usage of Azure Table Storage.


**Table data operations (monthly)**:

- User registers for 10 events per month
- L&D team creates/updates/cancels 500 events per month
- Total write calls = (5000 users * 10 events) + 500 L&D team = 50,500
- User views 50 events per month
- L&D team views/exports 1000 events per month
- Total read calls = (5000 users * 50 events) + 1000 L&D team = 2,51,000
- Azure Search service reads data for indexing
- For instantly reflecting data in Azure Search service, indexer is triggered manually whenever change happens to database

- Total storage calls = 50,500 + 2,51,000 = 3,01,500  

**Blob data operations (monthly)**:
- Blob storage is used to store and retrieve image.
- L&D team creates/updates/cancels 500 events per month 
- Total write calls = 500
- User views 50 events per month
- L&D team views/exports 1000 events per month
- Total read calls = (5000 users * 50 events) + 1000 L&D team = 2,51,000
- Total blob storage calls = 500 + 2,51,000 = 2,51,500

## Estimated cost

**IMPORTANT:** This is only an estimate, based on the assumptions above. Your actual costs may vary.

Prices were taken from the [Pricing](https://azure.microsoft.com/en-us/pricing/) on 01 Oct 2020, for the West US 2 region.

Use the [Azure Pricing Calculator](https://azure.com/e/c8628593214a424abba2b90067bf37a5) to model different service tiers and usage patterns.

| **Resource**  | **Tier**  | **Load**  | **Monthly price**  |
|--------------------------|-----------------|-------------------------|--------------------------------------|
| Bot Channels Registration | F0 | N/A | Free |
| App Service plan | S1 | 744 hours | $74.40 |
| App Service (Messaging Extension) | - |   | (charged to App Service plan) |
| Search service | B |   | $75.14 |
| Application Insights (Bot) | | | (free up to 5 GB) |
| Storage account (Table) | Standard_LRS | < 2GB data & 301,500 operations | $0.09 + $0.01 = $0.1 |
| Storage account (Blob) | Standard_LRS | < 2GB data & 500 write operations and 251,000 read operations | = $0.14 |
| Key vault | | 10000 operations | = $0.03 |
| Total | | | **$149.82** |