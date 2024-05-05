namespace Smab.PlexInfo.Models;

public record TvShowSummary
(
	int Seasons = default,
	int Episodes = default,
	int ViewedEpisodes = default
) : ItemSummary ;
