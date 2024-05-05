namespace Smab.PlexInfo.Models;

public record LibraryDirectory
(
	bool? AllowSync,
	string? Art,
	string? Composite,
	bool? Filters,
	bool? Refreshing,
	string? Thumb,
	string Key,
	string? Type,
	string Title,
	string? Agent,
	string? Scanner,
	string? Language,
	string? Uuid,
	bool? Content,
	bool? Directory,
	int? Hidden,
	List<Location>? Location,
	bool? EnableAutoPhotoTags,
	[property: JsonConverter(typeof(JsonUnixDateConverterWithNulls))] DateTime? UpdatedAt,
	[property: JsonConverter(typeof(JsonUnixDateConverterWithNulls))] DateTime? CreatedAt,
	[property: JsonConverter(typeof(JsonUnixDateConverterWithNulls))] DateTime? ScannedAt,
	[property: JsonConverter(typeof(JsonUnixDateConverterWithNulls))] DateTime? ContentChangedAt
);
