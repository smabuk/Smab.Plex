namespace Smab.PlexInfo.Models;

public record Metadata
(
	string? HistoryKey,
	string RatingKey,
	string? ParentRatingKey,
	string? GrandparentRatingKey,
	string Key,
	string? ParentKey,
	string? GrandparentKey,
	string? ParentGuid,
	string? GrandparentGuid,
	string? Studio,
	string Type,
	string? SubType,
	string Title,
	string? ParentTitle,
	string? GrandparentTitle,
	string? LibrarySectionTitle,
	int? LibrarySectionId,
	string? LibrarySectionKey,
	string? ContentRating,
	string? Summary,
	int? Index,
	int? ParentIndex,
	double? AudienceRating,
	int? ViewCount,
	int? Year,
	int? MaxYear,
	int? MinYear,
	string? Tagline,
	string? Thumb,
	string? ParentThumb,
	string? GrandparentThumb,
	string? Art,
	string? ParentArt,
	string? GrandparentArt,
	string? Banner,
	string? Theme,
	string? ParentTheme,
	int? Duration,
	string? OriginallyAvailableAt,
	int? LeafCount,
	int? ViewedLeafCount,
	int? ChildCount,
	string? AudienceRatingImage,
	string? ChapterSource,
	string? PrimaryExtraKey,
	string? RatingImage,
	int? AccountId,
	List<Medium>? Media,
	List<Subitem>? Genre,
	List<Subitem>? Director,
	List<Subitem>? Writer,
	List<Subitem>? Producer,
	List<Subitem>? Country,
	List<RoleSubitem>? Role,
	List<Subitem>? Collection,
	List<Subitem>? Similar,
	List<Location>? Location,
	// There are now 2 values "rating" and "Rating" the latter of which is an array
	[property: JsonPropertyName("ratingfix")] double? Rating,
	[property: JsonPropertyName("Rating")] List<RatingSubitem>? Ratings,
	// There are now 2 values "guid" and "Guid" the latter of which is an array
	[property: JsonPropertyName("guidfix")] string? Guid,
	[property: JsonPropertyName("Guid")] List<GuidSubitem>? Guids,
	[property: JsonPropertyName("addedAt")] [property: JsonConverter(typeof(JsonUnixDateConverter))] DateTime AddedAt,
	[property: JsonPropertyName("updatedAt")] [property: JsonConverter(typeof(JsonUnixDateConverter))] DateTime UpdatedAt,
	[property: JsonPropertyName("lastViewedAt")] [property: JsonConverter(typeof(JsonUnixDateConverterWithNulls))] DateTime? LastViewedAt
)
{
	public bool HasMedia => (Media is not null);
	public bool HasGenres => (Genre is not null);
	public bool HasDirectors => (Director is not null);
	public bool HasWriters => (Writer is not null);
	public bool HasProducers => (Producer is not null);
	public bool HasCountries => (Country is not null);
	public bool HasRoles => (Role is not null);
	public bool HasCollections => (Collection is not null);
	public bool HasSimilar => (Similar is not null);
	public bool HasLocations => (Location is not null);
};
