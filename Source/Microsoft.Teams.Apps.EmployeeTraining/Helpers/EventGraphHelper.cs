// <copyright file="EventGraphHelper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.EmployeeTraining.Helpers
{
    extern alias BetaLib;

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Localization;
    using Microsoft.Graph;
    using Microsoft.Teams.Apps.EmployeeTraining.Models;
#pragma warning disable SA1135 // Referring BETA package of MS Graph SDK.
    using Beta = BetaLib.Microsoft.Graph;
#pragma warning restore SA1135 // Referring BETA package of MS Graph SDK.
    using DayOfWeek = Microsoft.Graph.DayOfWeek;
    using EventType = Microsoft.Teams.Apps.EmployeeTraining.Models.EventType;
    using User = Microsoft.Graph.User;

    /// <summary>
    /// Implements the methods that are defined in <see cref="IGraphHelper"/>.
    /// </summary>
    public class EventGraphHelper : IEventGraphHelper
    {
        /// <summary>
        /// MS Graph batch limit is 20
        /// https://docs.microsoft.com/en-us/graph/known-issues#json-batching.
        /// </summary>
        private const int BatchSplitCount = 20;

        /// <summary>
        /// Maximum result count for search user and group request.
        /// </summary>
        private const int MaxResultCountForSearch = 10;

        /// <summary>
        /// Maximum count for recent collaborators
        /// </summary>
        private const int MaxCountForRecentCollaborators = 1000;

        /// <summary>
        /// The filter condition to get recent collaborators for sorting events those are popular in logged-in user's network
        /// </summary>
        private const string RecentCollaboratorsFilterForPopularInMyNetwork = "personType/class eq 'Person' and personType/subclass eq 'OrganizationUser'";

        /// <summary>
        /// Instance of graph service client for delegated requests.
        /// </summary>
        private readonly GraphServiceClient delegatedGraphClient;

        /// <summary>
        /// Instance of graph service client for application level requests.
        /// </summary>
        private readonly GraphServiceClient applicationGraphClient;

        /// <summary>
        /// Instance of BETA graph service client for application level requests.
        /// </summary>
        private readonly Beta.GraphServiceClient applicationBetaGraphClient;

        /// <summary>
        /// The current culture's string localizer
        /// </summary>
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventGraphHelper"/> class.
        /// </summary>
        /// <param name="tokenAcquisitionHelper">Helper to get user access token for specified Graph scopes.</param>
        /// <param name="httpContextAccessor">HTTP context accessor for getting user claims.</param>
        /// <param name="localizer">The current culture's string localizer</param>
        public EventGraphHelper(
            ITokenAcquisitionHelper tokenAcquisitionHelper,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<Strings> localizer)
        {
            this.localizer = localizer;
            httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

            var oidClaimType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
            var userObjectId = httpContextAccessor.HttpContext.User.Claims?
                .FirstOrDefault(claim => oidClaimType.Equals(claim.Type, StringComparison.OrdinalIgnoreCase))?.Value;

            if (!string.IsNullOrEmpty(userObjectId))
            {
                var jwtToken = AuthenticationHeaderValue.Parse(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString()).Parameter;

                this.delegatedGraphClient = GraphServiceClientFactory
                    .GetAuthenticatedGraphClient(async () =>
                    {
                        return await tokenAcquisitionHelper.GetUserAccessTokenAsync(userObjectId, jwtToken);
                    });

                this.applicationBetaGraphClient = GraphServiceClientFactory
                    .GetAuthenticatedBetaGraphClient(async () =>
                    {
                        return await tokenAcquisitionHelper.GetApplicationAccessTokenAsync();
                    });

                this.applicationGraphClient = GraphServiceClientFactory
                    .GetAuthenticatedGraphClient(async () =>
                    {
                        return await tokenAcquisitionHelper.GetApplicationAccessTokenAsync();
                    });
            }
        }

        /// <summary>
        /// Get users information from graph API.
        /// </summary>
        /// <param name="userObjectIds">Collection of AAD Object ids of users.</param>
        /// <returns>A task that returns collection of user information.</returns>
        public async Task<IEnumerable<User>> GetUserDetailsAsync(IEnumerable<string> userObjectIds)
        {
            userObjectIds = userObjectIds ?? throw new ArgumentNullException(nameof(userObjectIds));
            var userDetails = new List<User>();
            var userObjectIdsBatch = userObjectIds.ToList().SplitList(BatchSplitCount);

            BatchRequestContent batchRequestContent;
            foreach (var userObjectIdBatch in userObjectIdsBatch)
            {
                var batchIds = new List<string>();
                var userDetailsBatch = new List<User>();
                batchRequestContent = new BatchRequestContent();

                foreach (string userObjectId in userObjectIdBatch)
                {
                    var request = this.delegatedGraphClient
                        .Users[userObjectId]
                        .Request();

                    batchIds.Add(batchRequestContent.AddBatchRequestStep(request));
                }

                var response = await this.delegatedGraphClient.Batch.Request().PostAsync(batchRequestContent);
                for (int i = 0; i < batchIds.Count; i++)
                {
                    userDetailsBatch.Add(await response.GetResponseByIdAsync<User>(batchIds[i]));
                }

                userDetails.AddRange(userDetailsBatch);
                batchRequestContent.Dispose();
            }

            return userDetails;
        }

        /// <summary>
        /// Get group members for a group.
        /// </summary>
        /// <param name="groupId">AAD Object id of group.</param>
        /// <returns>A task that returns collection of group members.</returns>
        public async Task<IEnumerable<DirectoryObject>> GetGroupMembersAsync(string groupId)
        {
            groupId = groupId ?? throw new ArgumentNullException(nameof(groupId));
            var result = await this.delegatedGraphClient
                       .Groups[groupId].TransitiveMembers
                       .Request().GetAsync();

            var groupMembers = new List<DirectoryObject>();
            while (result.Count > 0)
            {
                groupMembers.AddRange(result);
                if (result.NextPageRequest != null)
                {
                    result = await result.NextPageRequest.GetAsync();
                }
                else
                {
                    break;
                }
            }

            groupMembers = groupMembers.Where(user => user.ODataType == "#microsoft.graph.user").ToList();

            return groupMembers;
        }

        /// <summary>
        /// Get user display name.
        /// </summary>
        /// <param name="userObjectId">AAD Object id of user.</param>
        /// <returns>A task that returns user information.</returns>
        public async Task<User> GetUserDetailAsync(string userObjectId)
        {
            return await this.delegatedGraphClient.Users[userObjectId].Request().GetAsync();
        }

        /// <summary>
        /// Get recent collaborators used to sort events by 'Popular in my network'
        /// </summary>
        /// <returns>List of recent collaborators</returns>
        public async Task<IEnumerable<Person>> GetRecentCollaboratorsForPopularInMyNetworkAsync()
        {
            var recentCollaboratorsResult = await this.delegatedGraphClient.Me.People.Request()
                .Filter(RecentCollaboratorsFilterForPopularInMyNetwork)
                .Top(MaxCountForRecentCollaborators)
                .Select("id, scoredEmailAddresses")
                .GetAsync();

            return recentCollaboratorsResult.CurrentPage;
        }

        /// <summary>
        /// Cancel calendar event.
        /// </summary>
        /// <param name="eventGraphId">Event Id received from Graph.</param>
        /// <param name="createdByUserId">User Id who created event.</param>
        /// <param name="comment">Cancellation comment.</param>
        /// <returns>True if event cancellation is successful.</returns>
        public async Task<bool> CancelEventAsync(string eventGraphId, string createdByUserId, string comment)
        {
            await this.applicationBetaGraphClient.Users[createdByUserId].Events[eventGraphId]
                .Cancel(comment).Request().PostAsync();

            return true;
        }

        /// <summary>
        /// Get top 10 users according to user search query.
        /// </summary>
        /// <param name="searchText">Search query entered by user.</param>
        /// <returns>List of users.</returns>
        public async Task<List<User>> SearchUsersAsync(string searchText)
        {
            searchText = searchText ?? throw new ArgumentNullException(nameof(searchText), "search text cannot be null");

            IGraphServiceUsersCollectionPage searchedUsers;
            if (searchText.Length > 0)
            {
                searchedUsers = await this.delegatedGraphClient.Users.Request()
                    .Top(MaxResultCountForSearch)
                    .Filter($"startsWith(displayName,'{searchText}') or startsWith(mail,'{searchText}')")
                    .Select("id,displayName,userPrincipalName,mail")
                    .GetAsync();
            }
            else
            {
                searchedUsers = await this.delegatedGraphClient.Users.Request()
                    .Top(MaxResultCountForSearch)
                    .Select("id,displayName,userPrincipalName,mail")
                    .GetAsync();
            }

            return searchedUsers.ToList();
        }

        /// <summary>
        /// Get top 10 groups according to user search query.
        /// </summary>
        /// <param name="searchText">Search query entered by user.</param>
        /// <returns>List of users.</returns>
        public async Task<List<Group>> SearchGroupsAsync(string searchText)
        {
            searchText = searchText ?? throw new ArgumentNullException(nameof(searchText), "search text cannot be null");
            IGraphServiceGroupsCollectionPage groups;

            if (searchText.Length > 0)
            {
                groups = await this.delegatedGraphClient.Groups.Request()
                    .Top(MaxResultCountForSearch)
                    .Filter($"startsWith(displayName,'{searchText}') or startsWith(mail,'{searchText}')")
                    .Select("id,displayName,userPrincipalName,mail")
                    .GetAsync();
            }
            else
            {
                groups = await this.delegatedGraphClient.Groups.Request()
                       .Top(MaxResultCountForSearch)
                       .Select("id,displayName,userPrincipalName,mail")
                       .GetAsync();
            }

            return groups.ToList();
        }

        /// <summary>
        /// Create teams event.
        /// </summary>
        /// <param name="eventEntity">Event details from user for which event needs to be created.</param>
        /// <returns>Created event details.</returns>
        public async Task<Event> CreateEventAsync(EventEntity eventEntity)
        {
            eventEntity = eventEntity ??
                throw new ArgumentNullException(nameof(eventEntity), "Event details cannot be null");

            var teamsEvent = new Event
            {
                Subject = eventEntity.Name,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = this.GetEventBodyContent(eventEntity),
                },
                Attendees = eventEntity.IsAutoRegister && eventEntity.Audience == (int)EventAudience.Private ?
                    await this.GetEventAttendeesTemplateAsync(eventEntity) :
                    new List<Attendee>(),
                OnlineMeetingUrl = eventEntity.Type == (int)EventType.LiveEvent ? eventEntity.MeetingLink : null,
                IsReminderOn = true,
                Location = eventEntity.Type == (int)EventType.InPerson ? new Location
                {
                    Address = new PhysicalAddress { Street = eventEntity.Venue },
                }
                    : null,
                AllowNewTimeProposals = false,
                IsOnlineMeeting = eventEntity.Type == (int)EventType.Teams,
                OnlineMeetingProvider = eventEntity.Type == (int)EventType.Teams ? OnlineMeetingProviderType.TeamsForBusiness : OnlineMeetingProviderType.Unknown,
            };

            teamsEvent.Start = new DateTimeTimeZone
            {
                DateTime = eventEntity.StartDate?.ToString("s", CultureInfo.InvariantCulture),
                TimeZone = TimeZoneInfo.Utc.Id,
            };
            teamsEvent.End = new DateTimeTimeZone
            {
                DateTime = eventEntity.StartDate.Value.Date.Add(
                new TimeSpan(eventEntity.EndTime.Hour, eventEntity.EndTime.Minute, eventEntity.EndTime.Second)).ToString("s", CultureInfo.InvariantCulture),
                TimeZone = TimeZoneInfo.Utc.Id,
            };

            if (eventEntity.NumberOfOccurrences > 1)
            {
                // Create recurring event.
                teamsEvent = this.GetRecurringEventTemplate(teamsEvent, eventEntity);
            }

            return await this.delegatedGraphClient.Me.Events.Request()
                .Header("Prefer", $"outlook.timezone=\"{TimeZoneInfo.Utc.Id}\"").AddAsync(teamsEvent);
        }

        /// <summary>
        /// Update teams event.
        /// </summary>
        /// <param name="eventEntity">Event details from user for which event needs to be updated.</param>
        /// <returns>Updated event details.</returns>
        public async Task<Event> UpdateEventAsync(EventEntity eventEntity)
        {
            eventEntity = eventEntity ??
                throw new ArgumentNullException(nameof(eventEntity), "Event details cannot be null");

            var teamsEvent = new Event
            {
                Subject = eventEntity.Name,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = this.GetEventBodyContent(eventEntity),
                },
                Attendees = await this.GetEventAttendeesTemplateAsync(eventEntity),
                OnlineMeetingUrl = eventEntity.Type == (int)EventType.LiveEvent ? eventEntity.MeetingLink : null,
                IsReminderOn = true,
                Location = eventEntity.Type == (int)EventType.InPerson ? new Location
                {
                    Address = new PhysicalAddress { Street = eventEntity.Venue },
                }
                    : null,
                AllowNewTimeProposals = false,
                IsOnlineMeeting = eventEntity.Type == (int)EventType.Teams,
                OnlineMeetingProvider = eventEntity.Type == (int)EventType.Teams ? OnlineMeetingProviderType.TeamsForBusiness : OnlineMeetingProviderType.Unknown,
            };

            teamsEvent.Start = new DateTimeTimeZone
            {
                DateTime = eventEntity.StartDate?.ToString("s", CultureInfo.InvariantCulture),
                TimeZone = TimeZoneInfo.Utc.Id,
            };
            teamsEvent.End = new DateTimeTimeZone
            {
                DateTime = eventEntity.StartDate.Value.Date.Add(
                new TimeSpan(eventEntity.EndTime.Hour, eventEntity.EndTime.Minute, eventEntity.EndTime.Second)).ToString("s", CultureInfo.InvariantCulture),
                TimeZone = TimeZoneInfo.Utc.Id,
            };

            if (eventEntity.NumberOfOccurrences > 1)
            {
                // Create recurring event.
                teamsEvent = this.GetRecurringEventTemplate(teamsEvent, eventEntity);
            }

            return await this.applicationGraphClient.Users[eventEntity.CreatedBy].Events[eventEntity.GraphEventId].Request()
                .Header("Prefer", $"outlook.timezone=\"{TimeZoneInfo.Utc.Id}\"").UpdateAsync(teamsEvent);
        }

        /// <summary>
        /// Modify event details for recurring event creation.
        /// </summary>
        /// <param name="teamsEvent">Event details which will be sent to Graph API.</param>
        /// <param name="eventEntity">Event details from user for which event needs to be created.</param>
        /// <returns>Event details to be sent to Graph API.</returns>
        private Event GetRecurringEventTemplate(Event teamsEvent, EventEntity eventEntity)
        {
            // Create recurring event.
            teamsEvent.Recurrence = new PatternedRecurrence
            {
                Pattern = new RecurrencePattern
                {
                    // Exclude weekends
                    Type = RecurrencePatternType.Weekly,
                    Interval = 1,
                    FirstDayOfWeek = DayOfWeek.Monday,
                    DaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday },
                },
                Range = new RecurrenceRange
                {
                    Type = RecurrenceRangeType.EndDate,
                    EndDate = new Date((int)eventEntity.EndDate?.Year, (int)eventEntity.EndDate?.Month, (int)eventEntity.EndDate?.Day),
                    StartDate = new Date((int)eventEntity.StartDate?.Year, (int)eventEntity.StartDate?.Month, (int)eventEntity.StartDate?.Day),
                },
            };

            return teamsEvent;
        }

        /// <summary>
        /// Get list of event attendees for creating teams event.
        /// </summary>
        /// <param name="eventEntity">Event details containing registered attendees.</param>
        /// <returns>List of attendees.</returns>
        private async Task<List<Attendee>> GetEventAttendeesTemplateAsync(EventEntity eventEntity)
        {
            var attendees = new List<Attendee>();

            if (string.IsNullOrEmpty(eventEntity.RegisteredAttendees) && string.IsNullOrEmpty(eventEntity.AutoRegisteredAttendees))
            {
                return attendees;
            }

            if (!string.IsNullOrEmpty(eventEntity.RegisteredAttendees))
            {
                var registeredAttendeesList = eventEntity.RegisteredAttendees.Trim().Split(";");
                if (registeredAttendeesList.Any())
                {
                    var userProfiles = await this.GetUserDetailsAsync(registeredAttendeesList);

                    foreach (var userProfile in userProfiles)
                    {
                        attendees.Add(new Attendee
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = userProfile.UserPrincipalName,
                                Name = userProfile.DisplayName,
                            },
                            Type = AttendeeType.Required,
                        });
                    }
                }
            }

            if (!string.IsNullOrEmpty(eventEntity.AutoRegisteredAttendees))
            {
                var autoRegisteredAttendeesList = eventEntity.AutoRegisteredAttendees.Trim().Split(";");
                if (autoRegisteredAttendeesList.Any())
                {
                    var userProfiles = await this.GetUserDetailsAsync(autoRegisteredAttendeesList);

                    foreach (var userProfile in userProfiles)
                    {
                        attendees.Add(new Attendee
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = userProfile.UserPrincipalName,
                                Name = userProfile.DisplayName,
                            },
                            Type = AttendeeType.Required,
                        });
                    }
                }
            }

            return attendees;
        }

        /// <summary>
        /// Get the event body content based on event type
        /// </summary>
        /// <param name="eventEntity">The event details</param>
        /// <returns>Returns </returns>
        private string GetEventBodyContent(EventEntity eventEntity)
        {
            switch ((EventType)eventEntity.Type)
            {
                case EventType.InPerson:
                    return $"{eventEntity.Description}<br/><br/>{this.localizer.GetString("CalendarEventLocationText", eventEntity.Venue)}";

                case EventType.LiveEvent:
                    return $"{eventEntity.Description}<br/><br/>{this.localizer.GetString("CalendarEventLiveEventURLText", $"<a href='{eventEntity.MeetingLink}'>{eventEntity.MeetingLink}</a>")}";

                default:
                    return eventEntity.Description;
            }
        }
    }
}
