namespace Smab.PlexInfo.Models;
public record ItemSummary
(
	int LibraryId = default,
	string LibraryTitle = "",
	int Id = default,
	string Title = "",
	int? Year = null,
	int Duration = default,
	string? Thumb = null,
	DateTime? AddedAt = null,
	double? AudienceRating = null,
	double? Rating = null,
	[property: JsonConverter(typeof(JsonDateOnlyConverter))] DateOnly? OriginallyAvailableAt = null
);
