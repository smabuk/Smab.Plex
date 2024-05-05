namespace Smab.PlexInfo.Models;

public record Subitem
(
	int? Id,
	string? Filter,
	string Tag,
	int? Count
)
{
	public override string ToString() => Tag;
};
