namespace Smab.PlexInfo.Models;

public record Medium
(
	int Id,
	int Duration,
	int Bitrate,
	int Width,
	int Height,
	float AspectRatio,
	int AudioChannels,
	string AudioCodec,
	string VideoCodec,
	string VideoResolution,
	string Container,
	string VideoFrameRate,
	string VideoProfile,
	List<Part>? Part,
	int? OptimizedForStreaming,
	string? AudioProfile,
	bool? Has64bitOffsets
);
