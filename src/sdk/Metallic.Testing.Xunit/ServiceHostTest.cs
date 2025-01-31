using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Metallic.Testing.Xunit;

public abstract class ServiceHostTest {

	protected readonly ILoggerProvider LoggerProvider;
	protected ILogger Logger;
	IHost? testHost;

	protected virtual IHost TestHost => testHost ??= CreateHost();
	protected virtual IServiceProvider Services => TestHost.Services;

	protected virtual SVC GetKeyedService<SVC>(string key) where SVC : notnull => Services
		.GetRequiredKeyedService<SVC>(key);

	protected virtual SVC GetService<SVC>() where SVC : notnull => Services
		.GetRequiredService<SVC>();

	protected virtual SVC GetOrCreateService<SVC>() where SVC : notnull =>
		GetOrCreateService<SVC, SVC>();

	protected virtual SVC GetOrCreateService<SVC, IMPL>() where SVC : notnull where IMPL : SVC => Services
		.GetRequiredService<SVC>() ?? Activator.CreateInstance<IMPL>();

	protected ServiceHostTest(ITestOutputHelper output) {
		LoggerProvider = new TestOutputLoggerProvider(output);
		Logger = LoggerProvider.CreateLogger(GetType().FullName!);
	}

	protected virtual IHost CreateHost() => CreateHostBuilder()
		.ConfigureLogging(lb => this.ConfigureLogging(lb))
		.ConfigureServices(svc => ConfigureServices(svc))
		.Build();

	protected virtual IHostBuilder CreateHostBuilder() => Host
		.CreateDefaultBuilder();

	protected virtual IServiceCollection ConfigureServices(IServiceCollection services) =>
		services;

	protected virtual ILoggingBuilder ConfigureLogging(ILoggingBuilder loggingBuilder) => loggingBuilder
		.ClearProviders()
		.AddProvider(LoggerProvider);
}