using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit.Abstractions;

namespace Tests.Support {
	public abstract class ServiceComponentTest {

		readonly Logger serilogger;
		protected readonly IServiceProvider Services;
		protected readonly Microsoft.Extensions.Logging.ILogger Logger;

		protected ServiceComponentTest(ITestOutputHelper output) {
			var loggerConfiguration = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Sink(new TestOutputSink(output));

			serilogger = loggerConfiguration.CreateLogger();
			Services = CreateServices().BuildServiceProvider();
			Logger = Services.GetRequiredService<ILogger<ServiceComponentTest>>();
		}

		protected virtual IServiceCollection CreateServices() {
			return new ServiceCollection()
				.AddLogging(builder => builder.AddSerilog(serilogger));
		}

		protected virtual T Service<T>() where T : notnull => Services!.GetRequiredService<T>()!;
	}

	public class TestOutputSink(ITestOutputHelper testOutputHelper) : ILogEventSink {
		public void Emit(LogEvent logEvent) {
			if (logEvent == null) return;
			var message = logEvent.RenderMessage();
			testOutputHelper.WriteLine(message);
		}
	}
}