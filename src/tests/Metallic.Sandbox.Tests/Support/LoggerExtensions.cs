using Microsoft.Extensions.Logging;

namespace Metallic.Sandbox.Tests.Support;

public static partial class LoggerExtensions {

	[LoggerMessage(0, LogLevel.Information, "Starting host")]
	public static partial void StartingHost(this ILogger logger);

	[LoggerMessage(1, LogLevel.Information, "Stopping host")]
	public static partial void StoppingHost(this ILogger logger);
};