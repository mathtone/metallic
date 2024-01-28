using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Tests.Metallic.Data.Npgsql.Support {
	public abstract class ServiceComponentTest : IDisposable, ILogEventSink {

		protected readonly ITestOutputHelper Output;
		protected readonly IServiceProvider ServiceProvider;
		protected readonly ILogger Log;

		protected ServiceComponentTest(ITestOutputHelper output) {
			Output = output;
			ServiceProvider = CreateServiceProvider();
			Log = ServiceProvider.GetRequiredService<ILogger<ServiceComponentTest>>();
		}

		public void Emit(LogEvent logEvent) =>
			Output.WriteLine(logEvent.RenderMessage());

		protected virtual SVC? GetService<SVC>() where SVC : notnull =>
			ServiceProvider.GetService<SVC>();

		protected virtual SVC? Activate<SVC>(params object[] parameters) where SVC : notnull =>
			ActivatorUtilities.CreateInstance<SVC>(ServiceProvider, parameters);
			
		protected virtual IServiceProvider CreateServiceProvider() =>
			ConfigureServices(CreateServices()).BuildServiceProvider();

		protected virtual IServiceCollection CreateServices() =>
			new ServiceCollection();

		protected virtual IServiceCollection ConfigureServices(IServiceCollection services) => services
			.AddLogging()
			.AddSerilog(cfg => cfg.WriteTo.Sink(this));

		public virtual void Dispose() {
			;
		}
	}
}