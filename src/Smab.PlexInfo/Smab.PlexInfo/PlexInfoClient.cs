using System.Net.Http.Json;

namespace Smab.PlexInfo;

public class PlexInfoClient(HttpClient httpClient) : IPlexClient 
{
	public HttpClient Client { get; } = httpClient;

	public async Task<LibraryItem?> GetItem(int id) => await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/item/{id}");

	public async Task<LibraryItem?> GetItemChildren(int id) => await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/itemchildren/{id}");

	public async Task<LibraryItem?> GetRelatedItems(int id) => await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/related/{id}");

	public async Task<LibraryItem?> GetSimilarItems(int id) => await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/similar/{id}");

	public async Task<LibraryItem> GetLibraries() => (await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/librarysections")) ?? new();

	public async Task<List<MovieSummary>> GetMoviesList() => await Client.GetFromJsonAsync<List<MovieSummary>>($"PlexInfo/movieslist") ?? [];

	public async Task<List<TvShowSummary>> GetTvShowsList() => await Client.GetFromJsonAsync<List<TvShowSummary>>($"PlexInfo/tvshowslist") ?? [];

}
