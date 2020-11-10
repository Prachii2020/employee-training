Employee Training app enables organizers to easily publish and distribute new events to targeted groups, keep track of interests for capacity planning, and send updates or reminders. With reminders, attendees stay updated about registered events, indicate interest, and can ask questions to organizers.

### Personas
- Learning and development team: Users of the app who will be creating the events and be responsible for making it mandatory for the selected users. Separate app manifest will be available for these users which will be installed in Teams channel. All users of the selected team can create, update or delete events.

- End Users: Attendees for the events, would be able to search and register themselves for the events as well as manage the mandatory events as nominated by the event creators.

App will be installed in personal scope for end-users and teams scope for L&D team.

### Create event
Events can be created by members of L&D team. Event creation is a 2-step process.

Step 1 consist of following metadata fields:
- Event name: Unique name of an event
- Event photo
- Event description: Details of an event
- Category: Each event will be tagged to a category. L&D team members can create categories (like Technical, Domain, Management etc.) and map them to an event.
- Event type: Three types of events can be created:
	- Teams meeting
	- In-person
	- Live event

- Maximum number of participants: Maximum number of participants who can register for an event
- Start date: Start date of event
- Start time: Start time of event
- End date: End date of event
- End time: End time of event
- Number of occurrences: Number of recurrences of an event.

[[/Images/CreateEventStep1.png|Create event - Step 1]]

Step 2 is used to select audience for an event.

Created event can have any of the following audience types:

- Public event: Event will be created for all users in the tenant and everyone can view and register for this event type.

- Private event: Event will be visible only to selected users. Event creator can select list of users and contact groups who can register for the event. Further, event can be marked as optional or mandatory for each user.

[[/Images/CreateEventStep2.png|Create event - Step 2]]

### Discover tab
- All events: All public events and private events created for logged-in user are shown in tiles. 
- Mandatory events: Mandatory events created for logged-in user are shown.
- Registered events: All events for which user is registered are shown.
- Completed events: All events which are completed by user are shown.

[[/Images/DiscoverTab.png|Discover tab]]

### Export event details
All event details including registered user names will be exported in csv format.
 - Event name
 - Event description
 - Event start and end date
 - Category
 - Registered user names

### Notifications
Notifications will be sent to end-users in personal scope in following scenarios:
- On successful registration of an event
- When user is auto-registered for an event
- On event cancellation (to all registered events)
- For upcoming events before 1 and 7 days (to all registered events)

### Messaging Extension

End-users will be able to query, select and share event details through the messaging extension. Following tabs will be shown in ME:
 - Recent: This tab will show all the recent events in the tenant.
 - Popular: This tab will show all the popular events created for logged in user.