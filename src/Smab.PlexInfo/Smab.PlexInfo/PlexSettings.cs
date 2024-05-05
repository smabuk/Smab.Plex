namespace Smab.PlexInfo;

public record PlexSettings {
	public string? Server { get; set; }
	public string? Token { get; set; }
	public string? RootPath { get; set; }
	/// <summary>
	/// Response Cache time for photo thumbnails (specified in minutes)
	/// </summary>
	public int? ThumbnailCacheDuration { get; set; }
}
