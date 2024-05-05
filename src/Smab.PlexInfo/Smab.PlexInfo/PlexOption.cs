namespace Smab.PlexInfo;

public record PlexOption {
	public string Key { get; init; }
	public string Value { get; init; }

	public PlexOption(string key, string value) {
		Key = key;
		Value = value;
	}

	public PlexOption(string key, bool value) {
		Key = key;
		Value = (value == true) ? "1" : "0";
	}

	public PlexOption(string key, int value) {
		Key = key;
		Value = value.ToString();
	}

	public override string ToString() => $"{Key}={Value}";
}
