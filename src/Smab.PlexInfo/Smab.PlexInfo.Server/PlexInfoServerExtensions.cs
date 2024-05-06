using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Smab.PlexInfo.Server;
public static class PlexInfoServerExtensions
{
	public static readonly int DEFAULT_DURATION = 60 * 24; // 24 hours
	private static PlexSettings _plexSettings = new();

	public static WebApplicationBuilder? AddPlexInfoServer(this WebApplicationBuilder? builder)
	{
		ArgumentNullException.ThrowIfNull(builder, nameof(builder));

		_plexSettings = builder.Configuration.GetSection(nameof(PlexSettings)).Get<PlexSettings>() ?? new();

		_ = builder.Services.Configure<PlexSettings>(builder.Configuration.GetSection(nameof(PlexSettings)));

		// Register the HttpClient for use in the controllers and services
		_ = builder.Services.AddHttpClient<IPlexClient, PlexClient>()
		// The local Plex Server will not have a proper certificate so we have to ignore this
		.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
		{
			ClientCertificateOptions = ClientCertificateOption.Manual,
			ServerCertificateCustomValidationCallback =
			(httpRequestMessage, cert, certChain, policyErrors) => true
		});

		return builder;
	}

	public static WebApplicationBuilder? AddPlexInfoServer(this WebApplicationBuilder? builder, Action<PlexSettings> options)
	{
		ArgumentNullException.ThrowIfNull(builder, nameof(builder));

		_ = builder.AddPlexInfoServer();

		_ = builder.Services.PostConfigure(options);
			
		PlexSettings plexSettings = new();
		options.Invoke(plexSettings);

		_plexSettings.Server = plexSettings.Server ?? _plexSettings.Server;
		_plexSettings.Token = plexSettings.Token ?? _plexSettings.Token;
		_plexSettings.ThumbnailCacheDuration = plexSettings.ThumbnailCacheDuration ?? _plexSettings.ThumbnailCacheDuration;

		return builder;
	}

	public static IMvcBuilder ConfigurePlexInfoApis(this IMvcBuilder builder, Action<PlexSettings>? options = null)
	{
		PlexSettings plexSettings = new();
		options?.Invoke(plexSettings);

		_plexSettings.ThumbnailCacheDuration = plexSettings.ThumbnailCacheDuration ?? _plexSettings.ThumbnailCacheDuration;

		_ = builder.AddMvcOptions(opt =>
			opt.CacheProfiles.Add("PlexInfoThumbnails",
			new()
			{
				// Multiply by 60 to convert from duration in minutes to seconds
				Duration = (_plexSettings.ThumbnailCacheDuration ?? DEFAULT_DURATION) * 60
			})
		);

		return builder;
	}
}
