using Microsoft.AspNetCore.Mvc;

namespace Smab.PlexInfo.Server.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PlexInfoController(IPlexClient plexClient) : ControllerBase {

	[HttpGet(Name = nameof(LibraryRoot))]
	public async Task<IActionResult> LibraryRoot() => Ok(await plexClient.GetLibraryRoot());

	[HttpGet(Name = nameof(LibrarySections))]
	public async Task<IActionResult> LibrarySections() => Ok(await plexClient.GetLibrarySections());

	[HttpGet(Name = nameof(AllMovies))]
	public async Task<IActionResult> AllMovies() => Ok(await plexClient.GetAllMovies());

	[HttpGet(Name = nameof(MoviesList))]
	public async Task<IActionResult> MoviesList() => Ok(await plexClient.GetMoviesList());

	[HttpGet(Name = nameof(TvShowsList))]
	public async Task<IActionResult> TvShowsList() => Ok(await plexClient.GetTvShowsList());

	[HttpGet("{id}", Name = nameof(Related))]
	public async Task<IActionResult> Related(int id) => Ok(await plexClient.GetRelated(id));

	[HttpGet("{id}", Name = nameof(Similar))]
	public async Task<IActionResult> Similar(int id) => Ok(await plexClient.GetSimilar(id));

	[HttpGet(Name = nameof(MovieCollections))]
	public async Task<IActionResult> MovieCollections() => Ok(await plexClient.GetMovieCollections());

	[HttpGet("{id}", Name = nameof(Item))]
	public async Task<IActionResult> Item(int id) {
		var item = await plexClient.GetItem(id);
		return (item is null) || (item.MediaContainer?.Size == 0)
			? NotFound(null)
			: Ok(item);
	}

	[HttpGet("{id}", Name = nameof(ItemChildren))]
	public async Task<IActionResult> ItemChildren(int id) {
		var item = await plexClient.GetItemChildren(id);
		return (item is null) || (item.MediaContainer?.Size == 0)
			? NotFound(null)
			: Ok(item);
	}

	[HttpGet(Name = nameof(Photo))]
	// Cache photos/thumbnails for 1 hour (3600 seconds)
	[ResponseCache(VaryByQueryKeys = new[] { "url", "width", "height" }, CacheProfileName = "PlexInfoThumbnails")]
	public async Task<IActionResult> Photo([FromQuery] string url, [FromQuery] int width = 180, [FromQuery] int height = 270) {
		byte[]? item = await plexClient.GetPhotoFromUrl(url, width, height);
		return item switch
		{
			null => NotFound(null),
			_ => new FileContentResult(item, "image/jpeg")
		};
	}

	[HttpGet("{resource}", Name = nameof(Resource))]
	public async Task<IActionResult> Resource(string resource) {
		byte[]? item = await plexClient.GetResource(resource);
		return item switch
		{
			null => NotFound(null),
			_ => new FileContentResult(item, "image/png")
		};
	}

}
