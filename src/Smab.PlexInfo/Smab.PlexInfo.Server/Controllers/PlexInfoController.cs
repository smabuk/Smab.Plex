using Microsoft.AspNetCore.Mvc;

namespace Smab.PlexInfo.Server.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class PlexInfoController : ControllerBase {

	private readonly IPlexClient _plexClient;

	public PlexInfoController(IPlexClient plexClient) {
		_plexClient = plexClient;
	}

	[HttpGet(Name = nameof(LibraryRoot))]
	public async Task<IActionResult> LibraryRoot() {
		var item = await _plexClient.GetLibraryRoot();
		return Ok(item);
	}

	[HttpGet(Name = nameof(LibrarySections))]
	public async Task<IActionResult> LibrarySections() {
		var items = await _plexClient.GetLibrarySections();
		return Ok(items);
	}

	[HttpGet(Name = nameof(AllMovies))]
	public async Task<IActionResult> AllMovies() {
		var items = await _plexClient.GetAllMovies();
		return Ok(items);
	}

	[HttpGet(Name = nameof(MoviesList))]
	public async Task<IActionResult> MoviesList() {
		var items = await _plexClient.GetMoviesList();
		return Ok(items);
	}

	[HttpGet(Name = nameof(TvShowsList))]
	public async Task<IActionResult> TvShowsList() {
		var items = await _plexClient.GetTvShowsList();
		return Ok(items);
	}

	[HttpGet("{id}", Name = nameof(Related))]
	public async Task<IActionResult> Related(int id) {
		var items = await _plexClient.GetRelated(id);
		return Ok(items);
	}

	[HttpGet("{id}", Name = nameof(Similar))]
	public async Task<IActionResult> Similar(int id) {
		var items = await _plexClient.GetSimilar(id);
		return Ok(items);
	}

	[HttpGet(Name = nameof(MovieCollections))]
	public async Task<IActionResult> MovieCollections() {
		var items = await _plexClient.GetMovieCollections();
		return Ok(items);
	}

	[HttpGet("{id}", Name = nameof(Item))]
	public async Task<IActionResult> Item(int id) {
		var item = await _plexClient.GetItem(id);
		if ((item is null) || (item.MediaContainer?.Size == 0)) {
			return NotFound(null);
		}
		return Ok(item);
	}

	[HttpGet("{id}", Name = nameof(ItemChildren))]
	public async Task<IActionResult> ItemChildren(int id) {
		var item = await _plexClient.GetItemChildren(id);
		if ((item is null) || (item.MediaContainer?.Size == 0)) {
			return NotFound(null);
		}
		return Ok(item);
	}

	[HttpGet(Name = nameof(Photo))]
	// Cache photos/thumbnails for 1 hour (3600 seconds)
	[ResponseCache(VaryByQueryKeys = new[] { "url", "width", "height" }, CacheProfileName = "PlexInfoThumbnails")]
	public async Task<IActionResult> Photo([FromQuery] string url, [FromQuery] int width = 180, [FromQuery] int height = 270) {
		byte[]? item = await _plexClient.GetPhotoFromUrl(url, width, height);
		if (item is null) {
			return NotFound(null);
		}
		return new FileContentResult(item, "image/jpeg");
	}

	[HttpGet("{resource}", Name = nameof(Resource))]
	public async Task<IActionResult> Resource(string resource) {
		byte[]? item = await _plexClient.GetResource(resource);
		if (item is null) {
			return NotFound(null);
		}
		return new FileContentResult(item, "image/png");
	}

}
