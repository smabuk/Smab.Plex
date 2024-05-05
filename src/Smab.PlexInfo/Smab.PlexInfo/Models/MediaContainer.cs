namespace Smab.PlexInfo.Models;

public record MediaContainer
(
	int Size,
	bool? Claimed,
	string? MachineIdentifier,
	string? Version,
	bool AllowSync,
	string? AugmentationKey,
	string? Art,
	string? Content,
	string Identifier,
	int? LibrarySectionID,
	string? LibrarySectionTitle,
	string? LibrarySectionUUID,
	string? MediaTagPrefix,
	int? MediaTagVersion,
	string? Thumb,
	string? Title1,
	string? Title2,
	string? ViewGroup,
	int? ViewMode,
	List<LibraryDirectory>? Directory,
	List<Metadata>? Metadata,
	List<Hub>? Hub,
	List<Setting>? Setting,
	List<Server>? Server
);
