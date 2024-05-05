namespace Smab.PlexInfo.Models;

public record Setting
(
	string Id,
	string Label,
	string Summary,
	string Type,
	object Default,
	object Value,
	bool Hidden,
	bool Advanced,
	string Group
);
