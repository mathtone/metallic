using Microsoft.Extensions.Hosting;

namespace Metallic.ModularHost;
public class ModuleHost : ModuleHost<ModuleHostConfiguration> {
	public ModuleHost(Func<Task<ModuleHostConfiguration>> asyncConfigLocator) :
		base(asyncConfigLocator) { }
	public ModuleHost(ModuleHostConfiguration config) :
		base(config) { }
	public ModuleHost(Func<ModuleHostConfiguration> configLocator) :
		base(configLocator) { }
}

public interface IModuleHost : IHost,IAsyncDisposable {
	Task StartAsync(CancellationToken cancellationToken = default);
	Task StopAsync(CancellationToken cancellationToken = default);
}

public class ModuleHost<CFG> : IModuleHost where CFG : IModuleHostConfig<IModuleConfiguration<IServiceConfiguration>> {

	IHost? app;
	readonly Func<Task<CFG>> asyncConfigLocator;

	public IServiceProvider Services => app!.Services;

	public ModuleHost(CFG config) =>
		this.asyncConfigLocator = () => Task.FromResult(config);
	public ModuleHost(Func<CFG> configLocator) =>
		this.asyncConfigLocator = () => Task.FromResult(configLocator());
	public ModuleHost(Func<Task<CFG>> asyncConfigLocator) =>
		this.asyncConfigLocator = asyncConfigLocator;

	public async Task StartAsync(CancellationToken cancellationToken = default) {
		if (app != null) await OnStop(cancellationToken);
		await OnStart(cancellationToken);
	}

	public Task StopAsync(CancellationToken cancellationToken = default) =>
		OnStop(cancellationToken);

	protected virtual void OnDispose() {
		app?.Dispose();
	}

	protected virtual async Task OnDisposeAsync() {
		await OnStop();
	}

	public void Dispose() {
		OnDispose();
		GC.SuppressFinalize(this);
	}

	protected virtual async Task OnStart(CancellationToken cancellationToken) {
		var cfg = await asyncConfigLocator();
		OnLoadConfig(cfg);
		await app!.StartAsync(cancellationToken);
	}

	protected virtual async Task OnStop(CancellationToken cancellationToken = default) {
		if (app != null)
			await app.StopAsync(cancellationToken);
	}

	protected virtual void OnLoadConfig(CFG configuration) {
		var bldr = Host.CreateDefaultBuilder()
			.ConfigureServices(svc => {
				foreach (var module in configuration.Modules) {
					OnLoadModule(module);
					foreach (var service in module.Services) {
						foreach (var desc in service.GetServiceDescriptors()) {
							svc.Add(desc);
						}
					}
					foreach (var initializer in module.Initializers) {
						var i = Activator.CreateInstance(initializer) as IServiceCollectionInitializer;
						OnInitializeModule(i);
						i!.Initialize(svc);
					}
				}
			});
		app = bldr.Build();
		//await app.StartAsync();
	}

	protected virtual void OnLoadModule(IModuleConfiguration<IServiceConfiguration> configuration) { }

	protected virtual void OnInitializeModule(IServiceCollectionInitializer initializer) { }

	public async ValueTask DisposeAsync() {
		await OnDisposeAsync();
		GC.SuppressFinalize(this);
	}
}