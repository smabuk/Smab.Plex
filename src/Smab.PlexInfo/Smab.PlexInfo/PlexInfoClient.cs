using System.Net.Http.Json;

namespace Smab.PlexInfo;

public class PlexInfoClient : IPlexClient 
{
	public HttpClient Client { get; }

	public PlexInfoClient(HttpClient httpClient)
	{
		Client = httpClient;
	}

	public async Task<LibraryItem?> GetItem(int id)
	{
		LibraryItem? item = await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/item/{id}");
		return item;
	}

	public async Task<LibraryItem?> GetItemChildren(int id)
	{
		LibraryItem? item = await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/itemchildren/{id}");
		return item;
	}

	public async Task<LibraryItem?> GetRelatedItems(int id)
	{
		LibraryItem? item = await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/related/{id}");
		return item;
	}

	public async Task<LibraryItem?> GetSimilarItems(int id)
	{
		LibraryItem? item = await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/similar/{id}");
		return item;
	}

	public async Task<LibraryItem> GetLibraries()
	{
		return (await Client.GetFromJsonAsync<LibraryItem>($"PlexInfo/librarysections")) ?? new();
	}

	public async Task<List<MovieSummary>> GetMoviesList()
	{
		List<MovieSummary>? items = await Client.GetFromJsonAsync<List<MovieSummary>>($"PlexInfo/movieslist");
		return items ?? new();
	}

	public async Task<List<TvShowSummary>> GetTvShowsList()
	{
		List<TvShowSummary>? items = await Client.GetFromJsonAsync<List<TvShowSummary>>($"PlexInfo/tvshowslist");
		return items ?? new();
	}

}
