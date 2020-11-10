import { EventAudience } from "../models/event-audience";
import { EventType } from "../models/event-type";
import { IPostType } from "../models/IPostType";
import { SortBy } from "../models/sort-by";

export interface IConstantDropdownItem {
	name: string;
	id: number;
}

export interface ITimeZonesItem {
	displayName: string;
	id: string;
}

export default class Resources {
	public static readonly dark: string = "dark";
	public static readonly contrast: string = "contrast";
	public static readonly eventNameMaxLength: number = 100;
	public static readonly eventDescriptionMaxLength: number = 1000;
	public static readonly eventVenueMaxLength: number = 200;
	public static readonly userEventsMobileFilteredCategoriesLocalStorageKey: string = "user-events-filtered-categories";
	public static readonly userEventsMobileFilteredUsersLocalStorageKey: string = "user-events-filtered-users";
	public static readonly userEventsMobileSortByFilterLocalStorageKey: string = "user-events-sortby";
	public static readonly validUrlRegExp: RegExp = /^http(s)?:\/\/(www\.)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/;

	public static readonly colorCells = [
		{ id: 'a', label: 'Wild blue yonder', color: '#A4A8CB' },
		{ id: 'b', label: 'Jasmine', color: '#FFDE85' },
		{ id: 'c', label: 'Sky blue', color: '#A0EAF8' },
		{ id: 'd', label: 'Nadeshiko pink', color: '#F1A7B9' },
		{ id: 'e', label: 'Lavender blue', color: '#E3D7FF' },
	];

	public static readonly audienceType: Array<IConstantDropdownItem> = [
		{ name: "Public", id: EventAudience.Public },
		{ name: "Private", id: EventAudience.Private },
	];

	public static readonly sortBy: Array<IPostType> = [
		{ name: "Newest", id: SortBy.Recent, color: "" },
		{ name: "Popularity", id: SortBy.Popularity, color: "" }
	];

	public static readonly eventType: Array<IConstantDropdownItem> = [
		{ name: "In person", id: EventType.InPerson },
		{ name: "Teams", id: EventType.Teams },
		{ name: "Live event", id: EventType.LiveEvent },
	];

	public static readonly windowsTimeZones: Array<ITimeZonesItem> = [
		{ id: "Dateline Standard Time", displayName: "(GMT-12:00) International Date Line West" },
		{ id: "Samoa Standard Time", displayName: "(GMT-11:00) Midway Island, Samoa" },
		{ id: "Hawaiian Standard Time", displayName: "(GMT-10:00) Hawaii" },
		{ id: "Alaskan Standard Time", displayName: "(GMT-09:00) Alaska" },
		{ id: "Pacific Standard Time", displayName: "(GMT-08:00) Pacific Time (US and Canada); Tijuana" },
		{ id: "Mountain Standard Time", displayName: "(GMT-07:00) Mountain Time (US and Canada)" },
		{ id: "Mexico Standard Time 2", displayName: "(GMT-07:00) Chihuahua, La Paz, Mazatlan" },
		{ id: "U.S. Mountain Standard Time", displayName: "(GMT-07:00) Arizona" },
		{ id: "Central Standard Time", displayName: "(GMT-06:00) Central Time (US and Canada" },
		{ id: "Canada Central Standard Time", displayName: "(GMT-06:00) Saskatchewan" },
		{ id: "Mexico Standard Time", displayName: "(GMT-06:00) Guadalajara, Mexico City, Monterrey" },
		{ id: "Central America Standard Time", displayName: "(GMT-06:00) Central America" },
		{ id: "Eastern Standard Time", displayName: "(GMT-05:00) Eastern Time (US and Canada)" },
		{ id: "U.S. Eastern Standard Time", displayName: "(GMT-05:00) Indiana (East)" },
		{ id: "S.A. Pacific Standard Time", displayName: "(GMT-05:00) Bogota, Lima, Quito" },
		{ id: "Atlantic Standard Time", displayName: "(GMT-04:00) Atlantic Time (Canada)" },
		{ id: "S.A. Western Standard Time", displayName: "(GMT-04:00) Caracas, La Paz" },
		{ id: "Pacific S.A. Standard Time", displayName: "(GMT-04:00) Santiago" },
		{ id: "Newfoundland and Labrador Standard Time", displayName: "(GMT-03:30) Newfoundland and Labrador" },
		{ id: "E. South America Standard Time", displayName: "(GMT-03:00) Brasilia" },
		{ id: "S.A. Eastern Standard Time", displayName: "(GMT-03:00) Buenos Aires, Georgetown" },
		{ id: "Greenland Standard Time", displayName: "(GMT-03:00) Greenland" },
		{ id: "Mid-Atlantic Standard Time", displayName: "(GMT-02:00) Mid-Atlantic" },
		{ id: "Azores Standard Time", displayName: "(GMT-01:00) Azores" },
		{ id: "Cape Verde Standard Time", displayName: "(GMT-01:00) Cape Verde Islands" },
		{ id: "GMT Standard Time", displayName: "(GMT) Greenwich Mean Time: Dublin, Edinburgh, Lisbon, London" },
		{ id: "Greenwich Standard Time", displayName: "(GMT) Casablanca, Monrovia" },
		{ id: "Central Europe Standard Time", displayName: "(GMT+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague" },
		{ id: "Central European Standard Time", displayName: "(GMT+01:00) Sarajevo, Skopje, Warsaw, Zagreb" },
		{ id: "Romance Standard Time", displayName: "(GMT+01:00) Brussels, Copenhagen, Madrid, Paris" },
		{ id: "W. Europe Standard Time", displayName: "(GMT+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna" },
		{ id: "W. Central Africa Standard Time", displayName: "(GMT+01:00) West Central Africa" },
		{ id: "E. Europe Standard Time", displayName: "(GMT+02:00) Bucharest" },
		{ id: "Egypt Standard Time", displayName: "(GMT+02:00) Cairo" },
		{ id: "FLE Standard Time", displayName: "(GMT+02:00) Helsinki, Kiev, Riga, Sofia, Tallinn, Vilnius" },
		{ id: "GTB Standard Time", displayName: "(GMT+02:00) Athens, Istanbul, Minsk" },
		{ id: "Israel Standard Time", displayName: "(GMT+02:00) Jerusalem" },
		{ id: "South Africa Standard Time", displayName: "(GMT+02:00) Harare, Pretoria" },
		{ id: "Russian Standard Time", displayName: "(GMT+03:00) Moscow, St. Petersburg, Volgograd" },
		{ id: "Arab Standard Time", displayName: "(GMT+03:00) Kuwait, Riyadh" },
		{ id: "E. Africa Standard Time", displayName: "(GMT+03:00) Nairobi" },
		{ id: "Arabic Standard Time", displayName: "(GMT+03:00) Baghdad" },
		{ id: "Iran Standard Time", displayName: "(GMT+03:30) Tehran" },
		{ id: "Arabian Standard Time", displayName: "(GMT+04:00) Abu Dhabi, Muscat" },
		{ id: "Caucasus Standard Time", displayName: "(GMT+04:00) Baku, Tbilisi, Yerevan" },
		{ id: "Transitional Islamic State of Afghanistan Standard Time", displayName: "(GMT+04:30) Kabul" },
		{ id: "Ekaterinburg Standard Time", displayName: "(GMT+05:00) Ekaterinburg" },
		{ id: "West Asia Standard Time", displayName: "(GMT+05:00) Islamabad, Karachi, Tashkent" },
		{ id: "India Standard Time", displayName: "(GMT+05:30) Chennai, Kolkata, Mumbai, New Delhi" },
		{ id: "Nepal Standard Time", displayName: "(GMT+05:45) Kathmandu" },
		{ id: "Central Asia Standard Time", displayName: "(GMT+06:00) Astana, Dhaka" },
		{ id: "Sri Lanka Standard Time", displayName: "(GMT+06:00) Sri Jayawardenepura" },
		{ id: "N. Central Asia Standard Time", displayName: "(GMT+06:00) Almaty, Novosibirsk" },
		{ id: "Myanmar Standard Time", displayName: "(GMT+06:30) Yangon Rangoon" },
		{ id: "S.E. Asia Standard Time", displayName: "(GMT+07:00) Bangkok, Hanoi, Jakarta" },
		{ id: "North Asia Standard Time", displayName: "(GMT+07:00) Krasnoyarsk" },
		{ id: "China Standard Time", displayName: "(GMT+08:00) Beijing, Chongqing, Hong Kong SAR, Urumqi" },
		{ id: "Singapore Standard Time", displayName: "(GMT+08:00) Kuala Lumpur, Singapore" },
		{ id: "Taipei Standard Time", displayName: "(GMT+08:00) Taipei" },
		{ id: "W. Australia Standard Time", displayName: "(GMT+08:00) Perth" },
		{ id: "North Asia East Standard Time", displayName: "(GMT+08:00) Irkutsk, Ulaanbaatar" },
		{ id: "Korea Standard Time", displayName: "(GMT+09:00) Seoul" },
		{ id: "Tokyo Standard Time", displayName: "(GMT+09:00) Osaka, Sapporo, Tokyo" },
		{ id: "Yakutsk Standard Time", displayName: "(GMT+09:00) Yakutsk" },
		{ id: "A.U.S. Central Standard Time", displayName: "(GMT+09:30) Darwin" },
		{ id: "Cen. Australia Standard Time", displayName: "(GMT+09:30) Adelaide" },
		{ id: "A.U.S. Eastern Standard Time", displayName: "(GMT+10:00) Canberra, Melbourne, Sydney" },
		{ id: "E. Australia Standard Time", displayName: "(GMT+10:00) Brisbane" },
		{ id: "Tasmania Standard Time", displayName: "(GMT+10:00) Hobart" },
		{ id: "Vladivostok Standard Time", displayName: "(GMT+10:00) Vladivostok" },
		{ id: "West Pacific Standard Time", displayName: "(GMT+10:00) Guam, Port Moresby" },
		{ id: "Central Pacific Standard Time", displayName: "(GMT+11:00) Magadan, Solomon Islands, New Caledonia" },
		{ id: "Fiji Islands Standard Time", displayName: "(GMT+12:00) Fiji Islands, Kamchatka, Marshall Islands" },
		{ id: "New Zealand Standard Time", displayName: "(GMT+12:00) Auckland, Wellington" },
		{ id: "Tonga Standard Time", displayName: "(GMT+13:00) Nuku'alofa" }
	];
}