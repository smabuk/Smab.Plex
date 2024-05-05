namespace Smab.PlexInfo.Models;

public record RoleSubitem
(
	string? Role,
	string? Thumb,
	int? Id,
	string? Filter,
	string Tag,
	int? Count

) : Subitem(Id, Filter, Tag, Count);
