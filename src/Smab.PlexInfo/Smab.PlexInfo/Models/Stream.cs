namespace Smab.PlexInfo.Models;

public record Stream
(
	int Id,
	int StreamType,
	string Codec,
	int Index,
	int Bitrate,
	int BitDepth,
	string ChromaLocation,
	string ChromaSubsampling,
	int CodedHeight,
	int CodedWidth,
	double FrameRate,
	int Height,
	int Level,
	string Profile,
	int RefFrames,
	int Width,
	string DisplayTitle,
	bool? Selected,
	int? Channels,
	string AudioChannelLayout,
	int? SamplingRate,
	string StreamIdentifier
);
