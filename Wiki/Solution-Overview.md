Employee Training app enables organizations to easily publish and distribute new events to targeted groups, keep track of interests for capacity planning, and send updates or reminders. With reminders, attendees stay updated about registered events, indicate interest, and can ask questions to organizers. 

[[/Images/ArchitectureDiagram.png|Solution Overview]]

**Employee Training App:** This is a web application built using the [Bot Framework SDK v4 for .NET](https://docs.microsoft.com/en-us/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0) and [ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1). Employee Training app will work in personal scope for end users and team scope for L&D team. End users will have tabs for all events & registered events, and Messaging Extension to share public & private events. L&D team will have tab to view details of all the created events and a bot to create new event. 

**Azure solution**  
- The App Service implements the Bot and Messaging Extension experience by providing end points for user communication. The app service hosts the React app in order to display events created by Learning and Development team through tab. User can register for an event.
- App endpoint is registered as messaging end point in bot registration portal and provides an endpoint /api/messages to process bot and Messaging Extension requests/response.
- App service hosts React application for tabs and provides custom APIs in back end for fetching and storing data securely.
- Single Sign-On experience is implemented in React application for seamless user authentication.

**Azure Bot Service:** Azure Bot Service is developed using BOT SDK v4. Employee Training web app endpoint is registered as messaging end point in bot registration portal.

**Azure Search service** 
- Leveraging querying and indexing capabilities of Azure Search service for faster retrieval of projects as per user demand. It provides robust queries over indexed data which overcomes query limitation of table storage.

**Application Insights:** Application Insights is used for tracking and logging. Details are provided in section [Telemetry](https://github.com/OfficeDev/microsoft-teams-apps-employeetraining/wiki/Telemetry.md).

**Data stores:** 
 - The web app is using Azure Table Storage and blob storage for data and image storage due to its cost-effective pricing model and providing support for No-SQL data models.
 - Azure Table Storage 
    - Azure table storage is used to store event details, L&D team details and user configuration values.
    - Below are the relationship dependencies between multiple entities involved in app  
        - Each L&D team member can create multiple events (1:many) 
        - Each L&D team member can add multiple categories (1:many) 
        - Each end user can register for multiple events (1:many) 
        - End user’s basic details will be saved once on bot installation for notification purpose. (1:1) 
        - Team’s basic details will be saved on bot installation (1:1)
 - Azure Blob Storage
    - Will provide the file store where event images will be maintained. As per current requirement, there will be only 1 image uploaded per event.
    - Images with same name when uploaded to blob container gets replaced.
    - Images will be stored in public blob container. 

**Microsoft Graph APIs:**
    - The app leverage Microsoft Graph APIs for [user details](https://docs.microsoft.com/en-us/graph/api/user-get?view=graph-rest-1.0&tabs=http), [create event](https://docs.microsoft.com/en-us/graph/api/calendar-post-events?view=graph-rest-beta&tabs=http), [recent collaborators](https://docs.microsoft.com/en-us/graph/people-example), [search users](https://docs.microsoft.com/en-us/graph/api/user-list?view=graph-rest-1.0&tabs=http) & [groups](https://docs.microsoft.com/en-us/graph/api/group-list?view=graph-rest-1.0&tabs=http) etc.

| Sr No | Use case | API | Delegated permissions | Application permissions |
|--|--|--|--|--|
| 1 | Get user details | https://docs.microsoft.com/en-us/graph/api/user-get?view=graph-rest-1.0&tabs=http | User.ReadBasic.All | NA |
| 2 | Recent collaborators (People API) | https://docs.microsoft.com/en-us/graph/people-example | People.Read | NA |
| 3 | Search users | https://docs.microsoft.com/en-us/graph/api/user-list?view=graph-rest-1.0 | User.ReadBasic.All | NA |
| 4 | Search groups | https://docs.microsoft.com/en-us/graph/api/group-list?view=graph-rest-1.0 | Directory.Read.All | NA |
| 5 | Create event | https://docs.microsoft.com/en-us/graph/api/calendar-post-events?view=graph-rest-1.0&tabs=http | NA | Calendars.ReadWrite |

**Authentication:**
- App Service Authentication: Single Sign-On experience is implemented in React application for seamless user authentication. The configuration setting is done at Bot channel registration in azure portal with defined graph scopes of User.ReadBasic.All, offline_access, openid and Calendars.ReadWrite which can be consented by tenant users.
- Task Module Authentication: The Task Module rendering custom UI page to fetch list of users from Graph APIs uses the token passed by bot handler during task/invoke action.

**API Authorization:**
- Must be part of L&D team: While creating and managing events through tab, it needs to be validated whether logged in user is part of L&D team where app is installed. This can be done using Graph API to check members of a Team.   

**Background service: (IHostService):**
- Recurrence triggered IHostService to hit Table Storage for performing data sync operations on every 24 hours and send digest notifications daily.
