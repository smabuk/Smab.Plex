namespace Smab.PlexInfo;

public interface IPlexClient
{
    Task<LibraryItem> GetLibraryRoot() => throw new NotImplementedException();
    Task<LibraryItem> GetLibrarySections() => throw new NotImplementedException();
    Task<List<LibraryItem>> GetAllMovies() => throw new NotImplementedException();
    Task<List<LibraryItem>> GetAllMovies(int start, int size) => throw new NotImplementedException();
	Task<List<MovieSummary>> GetMoviesList() => throw new NotImplementedException();
	Task<List<LibraryItem>> GetTvShows(int? start = null, int? size = null) => throw new NotImplementedException();
	Task<List<TvShowSummary>> GetTvShowsList(int? start = null, int? size = null) => throw new NotImplementedException();
    Task<LibraryItem> GetRelated(int id) => throw new NotImplementedException();
    Task<LibraryItem> GetSimilar(int id) => throw new NotImplementedException();
    Task<LibraryItem> GetMovieCollections() => throw new NotImplementedException();
    Task<LibraryItem?> GetItem(int id) => throw new NotImplementedException();
    Task<LibraryItem?> GetItemChildren(int id) => throw new NotImplementedException();
    Task<byte[]?> GetResource(string resource) => throw new NotImplementedException();
    Task<byte[]?> GetPhotoFromUrl(string url, int width, int height) => throw new NotImplementedException();
}
