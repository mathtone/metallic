
using Avalonia;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Metallic.Avalonia.Host;

using Host = Microsoft.Extensions.Hosting.Host;

public partial class AvaloniaHostApp(string[] args) : Application {

	IHost? host;

	public override void Initialize() {
		base.Initialize();
		AvaloniaXamlLoader.Load(this);
	}

	public override async void OnFrameworkInitializationCompleted() {
		base.OnFrameworkInitializationCompleted();
		host = CreateHost(args);
		
		await host.StartAsync();
	}

	protected virtual IHostBuilder ConfigureHostBuilder(IHostBuilder builder) => builder
		.ConfigureServices(svc => svc.AddSingleton(this.ApplicationLifetime!));

	protected virtual IHostBuilder CreateHostBuilder(string[] args) => Host
		.CreateDefaultBuilder(args);

	protected virtual IHost CreateHost(string[] args) => CreateHostBuilder(args)
		.Build();
}