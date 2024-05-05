namespace Smab.PlexInfo.Models;

public record Server
(
	string Name,
	string Host,
	string Address,
	int Port,
	string MachineIdentifier,
	string Version
);
