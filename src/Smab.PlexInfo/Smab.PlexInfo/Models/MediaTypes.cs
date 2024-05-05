namespace Smab.PlexInfo.Models;

public static class MediaTypes
{
	public static List<MediaType> Collection => new() {
		new(Id:  1, TypeString: "movie",        Title: "Movie",       Element: "video",     Related: null),
		new(Id:  2, TypeString: "show",         Title: "Show",        Element: "directory", Related: new() {3, 4}  ),
		new(Id:  3, TypeString: "season",       Title: "Season",      Element: "directory", Related: new() {2, 4}),
		new(Id:  4, TypeString: "episode",      Title: "Episode",     Element: "video",     Related: new() {2, 3}),
		new(Id:  5, TypeString: "trailer",      Title: "Trailer",     Element: "video",     Related: null),
		new(Id:  6, TypeString: "comic",        Title: "Comic",       Element: "photo",     Related: null),
		new(Id:  7, TypeString: "person",       Title: "Person",      Element: "directory", Related: null),
		new(Id:  8, TypeString: "artist",       Title: "Artist",      Element: "directory", Related: new() {9, 10}),
		new(Id:  9, TypeString: "album",        Title: "Album",       Element: "directory", Related: new() { 8, 10 }),
		new(Id: 10, TypeString: "track",        Title: "Track",       Element: "audio",     Related: new() { 8, 9 }),
		new(Id: 11, TypeString: "photoAlbum",   Title: "Photo Album", Element: "directory", Related: new() { 12, 13 }),
		new(Id: 12, TypeString: "picture",      Title: "Picture",     Element: "photo",     Related: new() {11}),
		new(Id: 13, TypeString: "photo",        Title: "Photo",       Element: "photo",     Related: new() {11}),
		new(Id: 14, TypeString: "clip",         Title: "Clip",        Element: "video",     Related: null),
		new(Id: 15, TypeString: "playlistItem", Title: "Clip",        Element: "video",     Related: null)
	};
}

public record MediaType
(
	int Id,
	string TypeString,
	string Title,
	string Element,
	List<int>? Related
);
