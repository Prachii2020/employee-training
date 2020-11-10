The app uses the following data store:

All these resources are created in your Azure subscription. None are hosted directly by Microsoft.

- **Azure Table Storage**
    - [EventEntity] to store event related details.
    - [Categories] to store categories created by L&D team. 
    - [UserConfiguration] to store user's bot details for sending notifications.
    - [LnDTeamConfiguration] to store details of L&D teams where bot is installed.

- **Azure Blob Storage**
    - Storage for images uploaded while creating event.

## Storage account tables

**1. EventEntity**

The table has following rows:

| Attribute | Comment |
|--|--|
| PartitionKey | Team Id of team which has created the event. |
| RowKey | Unique identifier (GUID) for each event (Event Id). | 
| Timestamp | Date and time stamp when the entity row is modified. |
| EventName | Name of event. |
| Description | Description of event. |
| ImageUrl | URL of image uploaded on Azure Blob. |
| StartDate | Start date of event. |
| StartTime | Start time of event. | 
| EndDate | End date of event. |
| EndTime | End time of event. |
| EventType | Type of event: In person, Live event or Teams meeting. |
| Location | Location of event. Will be null for Teams meeting and Live event. |
| MeetingLink | Link of Teams/Outlook meeting created for event. |
| MaximumNumberOfParticipants | Maximum number of participants who can register for an event. |
| CategoryId | Id of category assigned selected for event. |
| CategoryName | Name of category assigned selected for event. |
| Audience | Type of audience for event. It can be either Public or Private. |
| IsAutoRegister | True if users needs to be auto registered for event. |
| Status | Current status of event. It can be Draft, Active or Completed. |
| MandatoryAttendees | Semicolon separated AAD user ID of mandatory users. |
| OptionalAttendees | Semicolon separated AAD user ID of optional users. |
| RegisteredAttendees | Semicolon separated AAD user ID of registered users. |
| CreatedBy | AAD Id of user who has created event. |
| CreatedOn | DateTime of event creation. |
| UpdatedBy | AAD Id of user who has updated event recently. |
| UpdatedOn | DateTime of recent event modification. |

**2. Categories**

The table has following rows:

| Attribute | Comment |
|--|--|
| PartitionKey | Team Id of team which has created the category. |
| RowKey | Unique identifier(GUID) for each category [CategoryId]. | 
| TimeStamp | Timestamp of actual record insertion (Done by Azure). |
| CategoryId | Unique identifier(GUID) for each category. |
| CategoryName | Name of the category. |
| CategoryDescription | Description of the category. |
| CreatedByUserId | User AAD object id who created the category. |
| ModifiedByUserId | User AAD object id who modified the category. |
| CreatedOn | DateTime at which category is created. |
| ModifiedOn | DateTime of recent event modification. |

**3. UserConfiguration**

The table has following rows:

|Attribute|Comment |
|--|--|
| PartitionKey | User’s AAD Id. |
| RowKey | User’s AAD Id. |
| ConversationId | Bot’s conversation Id. |
| ServiceUrl | Service URL for sending notification. |
| BotInstalledOn | DateTime of bot installation. |

**4. LnDTeamConfiguration**

The table has following rows:

|Attribute|Comment |
|--|--|
|PartitionKey| Team Id where bot is installed.|
|RowKey| Team Id where bot is installed.|
|BotInstalledOn| DateTime when bot is installed in Team.|
