using Metallic.ModuleHost.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;
namespace ModuleHostConfigStudy;

public class UnitTest1(ITestOutputHelper output) {

	[Fact]
	public async Task TestAsync() {
		var cfg = await new ModuleHostConfigBuilder()
			.AddModule(b => b
				.AddSingleton<ITestService, TestService>()
				.AddSingleton<ITestService2, TestService2>()
			)
			.BuildAsync();
		;
	}

	[Fact]
	public async Task TestAsync2() {
		using var host = new ModuleHost<IModuleHostConfig>(ConfigLocator);
		await host.StartAsync();
		var svc1 = host.Services.GetRequiredService<ITestService>();
		var svc2 = host.Services.GetRequiredService<ITestService2>();
		;
	}

	ValueTask<IModuleHostConfig> ConfigLocator() => new ModuleHostConfigBuilder()
		.AddModule(b => b
			.AddInitializer(builder => {

			})
			.AddSingleton<ITestService, TestService>()
			.AddSingleton<ITestService2, TestService2>()
		)
		.BuildAsync();

	class TestService : ITestService { }
	interface ITestService { }

	class TestService2(ITestService testService) : ITestService2 { }
	interface ITestService2 { }
}

public class ModuleHost<CFG>(Func<ValueTask<CFG>> configLocator) : IHost where CFG : IModuleHostConfig {

	IHost? app;

	public IServiceProvider Services => app!.Services;

	public async Task StartAsync(CancellationToken cancel = default) {
		var cfg = await configLocator();
		var host = Host.CreateDefaultBuilder();
		var desc = cfg.Modules.SelectMany(m => m.Services).SelectMany(c => c.GetServiceDescriptors()).ToArray();

		app = host.ConfigureServices(s => {
			foreach (var d in desc) {
				s.Add(d);
			}
		}).Build();

		await app.StartAsync(cancel);
	}

	public async Task StopAsync(CancellationToken cancel = default) {
		if (app != null) {
			await app.StopAsync(cancel);
			app.Dispose();
			app = null;
		}
	}

	public async Task RestartAsync(CancellationToken cancel = default) {
		await StopAsync(cancel);
		await StartAsync(cancel);
	}

	public void Dispose() {
		app?.Dispose();
		app = null;
		GC.SuppressFinalize(this);
	}
}

public static class IServiceConfigExtensions {
	public static IEnumerable<ServiceDescriptor> GetServiceDescriptors(this IServiceConfig cfg) {
		if (cfg.Lifetime.HasValue) {
			var primaryServiceType = cfg.ServiceTypes.First();
			if (cfg.ActivatorParameters?.Any() ?? false) {
				yield return new(primaryServiceType, svc => ActivatorUtilities.CreateInstance(svc, cfg.ImplementationType!, cfg.ActivatorParameters.ToArray()), cfg.Lifetime.Value);
			}
			else {
				yield return new ServiceDescriptor(primaryServiceType, cfg.ImplementationType!, cfg.Lifetime.Value);
			}
		}
		else {
			if (cfg.AddHosting) {
				if (cfg.ActivatorParameters?.Any() ?? false) {
					;
				}
				else {
					;
				}
			}
		}
	}
}