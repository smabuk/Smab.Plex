namespace Smab.PlexInfo.Models;

public record MyPlex
(
	string AuthToken,
	string Username,
	string MappingState,
	string MappingError,
	string SignInState,
	string PublicAddress,
	int PublicPort,
	string PrivateAddress,
	int PrivatePort,
	string SubscriptionFeatures,
	bool SubscriptionActive,
	string SubscriptionState
);
