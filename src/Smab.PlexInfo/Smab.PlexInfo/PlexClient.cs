using System.Text.Json;

using Microsoft.Extensions.Options;

namespace Smab.PlexInfo;

public class PlexClient : IPlexClient
{
	private readonly HttpClient _httpClient;
	private readonly string token = "";
	private readonly int TV_LIBRARY_ID = 1;
	private readonly int MOVIE_LIBRARY_ID = 3;
	private readonly int OTHER_MOVIE_LIBRARY_ID = 7;
	private readonly int SHORT_MOVIE_LIBRARY_ID = 8;

	public PlexClient(HttpClient httpClient, IOptions<PlexSettings> plexSettings)
	{
		ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
		ArgumentNullException.ThrowIfNull(plexSettings.Value.Server, nameof(plexSettings.Value.Server));
		ArgumentNullException.ThrowIfNull(plexSettings.Value.Token, nameof(plexSettings.Value.Token));
		token = plexSettings.Value.Token;
		if (String.IsNullOrWhiteSpace(token) || token == "TOKEN") {
			throw new ArgumentOutOfRangeException(nameof(plexSettings), "Plex token must have a value");
		}

		httpClient.BaseAddress = new Uri(plexSettings.Value.Server);
		httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
		httpClient.Timeout = new(0, 3, 0);
		_httpClient = httpClient;
	}

	public async Task<LibraryItem> GetLibraryRoot()
	{
		return await CallPlexApi<LibraryItem>($"library")
			?? throw new ApplicationException("Library root not found");
	}

	public async Task<LibraryItem> GetLibrarySections()
	{
		return await CallPlexApi<LibraryItem>($"library/sections")
			?? throw new ApplicationException("No library sections found");
	}

	public async Task<List<LibraryItem>> GetAllMovies()
	{
		List<PlexOption> options =
		[
			new("X-Plex-Features", "external-media,indirect-media"),
			new("X-Plex-Model", "bundled"),
			new("type", 1),
			new("includeCollections", false),
			new("includeExternalMedia", true),
			new("includeAdvanced", true),
			new("includeMeta", true),
			new("sort", "titleSort")
		];

		Task<LibraryItem?> task1 = CallPlexApi<LibraryItem>($"library/sections/{MOVIE_LIBRARY_ID}/all", options);
		Task<LibraryItem?> task2 = CallPlexApi<LibraryItem>($"library/sections/{OTHER_MOVIE_LIBRARY_ID}/all", options);
		Task<LibraryItem?> task3 = CallPlexApi<LibraryItem>($"library/sections/{SHORT_MOVIE_LIBRARY_ID}/all", options);

		LibraryItem? result1 = await task1;
		LibraryItem? result2 = await task2;
		LibraryItem? result3 = await task3;

		return [result1, result2, result3];
	}

	public async Task<List<LibraryItem>> GetAllMovies(int start, int size)
	{
		List<PlexOption> options =
		[
			new("X-Plex-Container-Start", start),
			new("X-Plex-Container-Size", size),
			new("X-Plex-Features", "external-media,indirect-media"),
			new("X-Plex-Model", "bundled"),
			new("type", 1),
			new("includeCollections", false),
			new("includeExternalMedia", true),
			new("includeAdvanced", true),
			new("includeMeta", true),
			new("sort", "titleSort")
		];

		LibraryItem result = await CallPlexApi<LibraryItem>($"library/sections/{MOVIE_LIBRARY_ID}/all", options)
			?? throw new NullReferenceException("No movies found");

		return [result];
	}

	public async Task<LibraryItem> GetMovieCollections()
	{
		List<PlexOption> options =
		[
			new("X-Plex-Features", "external-media,indirect-media"),
			new("X-Plex-Model", "bundled"),
			new("includeCollections", true),
			new("includeExternalMedia", true),
			new("includeAdvanced", true),
			new("includeMeta", true),
			new("sort", "titleSort")
		];

		LibraryItem result = await CallPlexApi<LibraryItem>($"library/sections/{MOVIE_LIBRARY_ID}/collections", options)
			?? throw new ApplicationException("No collections found");

		return result;
	}

	public async Task<List<MovieSummary>> GetMoviesList()
	{
		List<LibraryItem> items = await GetAllMovies();
		// SelectMany flattens the returned structure to IEnumerable<MovieSummary>
		List<MovieSummary> result = items.SelectMany(i => i.MediaContainer?.Metadata?
			.Select(m =>
				new MovieSummary()
				{
					LibraryId = i.MediaContainer.LibrarySectionID ?? 0,
					LibraryTitle = i.MediaContainer.LibrarySectionTitle ?? "",
					Id = int.Parse(m.Key.Replace(@"/library/metadata/", "").Replace(@"/children", "")),
					Title = m.Title,
					Year = m.Year,
					Duration = m.Duration ?? 0,
					Thumb = m.Thumb,
					AddedAt = m.AddedAt,
					AudienceRating = m.AudienceRating,
					Rating = m.Rating,
					OriginallyAvailableAt = (m.OriginallyAvailableAt is not null) ? DateOnly.Parse(m.OriginallyAvailableAt) : ((m.Year is not null) ? new(m.Year ?? 1, 1, 1) : null),
				})
			?? [])
			.ToList();

		return result;
	}

	public async Task<List<LibraryItem>> GetTvShows(int? start = null, int? size = null)
	{
		List<PlexOption> options =
		[
			new("X-Plex-Features", "external-media,indirect-media"),
			new("X-Plex-Model", "bundled"),
			new("includeCollections", true),
			new("includeExternalMedia", true),
			new("includeAdvanced", true),
			new("includeMeta", true),
			//new("unmatched", true),
			//new("unwatchedLeaves", true),
			new("sort", "originallyAvailableAt:desc")
		];

		if ((start is not null) && (size is not null)) {
			options.Add(new PlexOption("X-Plex-Container-Start", (int)start));
			options.Add(new PlexOption("X-Plex-Container-Size", (int)size));
		}

		LibraryItem result = await CallPlexApi<LibraryItem>($"library/sections/{TV_LIBRARY_ID}/all", options)
			?? throw new NullReferenceException("No TV series found");

		return [result];
	}

	public async Task<List<TvShowSummary>> GetTvShowsList(int? start = null, int? size = null)
	{
		List<LibraryItem> items = await GetTvShows();

		// SelectMany flattens the returned structure to IEnumerable<TvShowSummary>
		List<TvShowSummary> result = items
			.SelectMany(i => i.MediaContainer?.Metadata?
				.Select(m =>
					new TvShowSummary()
					{
						LibraryId = i.MediaContainer.LibrarySectionID ?? 0,
						LibraryTitle = i.MediaContainer.LibrarySectionTitle ?? "",
						Id = int.Parse(m.Key.Replace(@"/library/metadata/", "").Replace(@"/children", "")),
						Title = m.Title,
						Year = m.Year,
						Duration = m.Duration ?? 0,
						Thumb = m.Thumb,
						AddedAt = m.AddedAt,
						AudienceRating = m.AudienceRating,
						Rating = m.Rating,
						OriginallyAvailableAt = (m.OriginallyAvailableAt is not null) ? DateOnly.Parse(m.OriginallyAvailableAt) : ((m.Year is not null) ? new(m.Year ?? 1, 1, 1) : null),
						Seasons = m.ChildCount ?? 0,
						Episodes = m.LeafCount ?? 0,
						ViewedEpisodes = m.ViewedLeafCount ?? 0,
					})
				?? [])
			.ToList();

		return result;
	}

	public async Task<LibraryItem?> GetItem(int id) => await CallPlexApi<LibraryItem>($"library/metadata/{id}");

	public async Task<LibraryItem?> GetItemChildren(int id) => await CallPlexApi<LibraryItem>($"library/metadata/{id}/children");

	public async Task<byte[]?> GetPhotoFromUrl(string url, int width = 180, int height = 270)
	{
		List<PlexOption> options =
		[
			new("width", width),
			new("height", height),
			new("minsize", true),
			new("upscale", true),
			new("url", url)
		];

		return await CallPlexApiAndReturnImage($"photo/:/transcode", options);
	}

	public async Task<byte[]?> GetResource(string resource) => await CallPlexApiAndReturnImage($@"/:/resources/{resource}", []);

	public async Task<LibraryItem> GetRelated(int id)
	{
		List<PlexOption> options =
		[
			new("X-Plex-Features", "external-media,indirect-media"),
			new("X-Plex-Model", "bundled"),
			new("excludeFields", "summary"),
			new("includeExternalMedia", true),
			new("includeExternalMetadata", true),
			new("asyncAugmentMetadata", true)
		];

		return await CallPlexApi<LibraryItem>($"hubs/metadata/{id}/related", options)
			?? throw new NullReferenceException("No related found");
	}

	public async Task<LibraryItem> GetSimilar(int id)
	{
		List<PlexOption> options =
		[
			new("X-Plex-Features", "external-media,indirect-media"),
			new("X-Plex-Model", "bundled"),
			new("excludeFields", "summary"),
			new("includeExternalMedia", true),
			new("includeExternalMetadata", true),
			new("asyncAugmentMetadata", true)
		];

		return await CallPlexApi<LibraryItem>($"library/metadata/{id}/similar", options)
			?? throw new NullReferenceException("No similar found");
	}


	private async Task<T?> CallPlexApi<T>(string query, IEnumerable<PlexOption>? options = null) where T : class
	{
		string optionsString = "";
		foreach (PlexOption option in options ?? []) {
			optionsString += $"&{option}";
		}

		var response = await _httpClient.GetAsync($"{query}?X-Plex-Token={token}{optionsString}");

		if (response.IsSuccessStatusCode && response.Content.Headers.ContentType?.ToString() == "application/json") {
			string json = await response.Content.ReadAsStringAsync();
			json = json
				.Replace("\"guid\"", "\"guidfix\"")
				.Replace("\"rating\"", "\"ratingfix\"");

			return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions.Web);
		}

		return null;
	}

	private async Task<byte[]?> CallPlexApiAndReturnImage(string query, IEnumerable<PlexOption>? options = null)
	{
		string optionsString = "";
		foreach (PlexOption option in options ?? []) {
			optionsString += $"&{option}";
		}

		var response = await _httpClient.GetAsync($"{query}?X-Plex-Token={token}{optionsString}");

		return response.IsSuccessStatusCode switch
		{
			false => null,
			true when response.Content.Headers.ContentType?.ToString() == "image/jpeg"
				=> await response.Content.ReadAsByteArrayAsync(),
			true when response.Content.Headers.ContentType?.ToString() == "image/png"
				=> await response.Content.ReadAsByteArrayAsync(),
			_ => null
		};
	}
}
