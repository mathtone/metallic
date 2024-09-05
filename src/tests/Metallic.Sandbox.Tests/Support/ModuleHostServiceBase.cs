using Metallic.ModularHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Metallic.Sandbox.Tests.Support;

public interface IHostConfigProvider {
	Task<ModuleHostConfiguration> GetConfigurationAsync();
}

public class ModuleHostService(ILogger<ModuleHostService> logger, IHostConfigProvider configProvider) :

	ModuleHostServiceBase<ModuleHost>(logger) {

	protected Task<ModuleHostConfiguration> GetConfiguration() =>
		configProvider.GetConfigurationAsync();

	protected override async Task<ModuleHost> CreateHost() =>
		new ModuleHost(await GetConfiguration());
}

public abstract class ModuleHostServiceBase<HOST>(ILogger<ModuleHostServiceBase<HOST>> logger) : IHostedService
	where HOST : IModuleHost {

	protected HOST? Host { get; private set; }
	protected readonly ILogger Logger = logger;

	public async Task StartAsync(CancellationToken cancellationToken) {
		Logger.StartingHost();
		if (Host != null)
			await StopAsync(cancellationToken);

		Host = await CreateHost();
		await Host.StartAsync(cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken) {
		Logger.StoppingHost();
		await (Host?.StopAsync(cancellationToken) ?? Task.CompletedTask);
	}

	protected abstract Task<HOST> CreateHost();
}	