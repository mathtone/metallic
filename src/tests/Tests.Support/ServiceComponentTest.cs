using Microsoft.Extensions.Logging;
using Serilog;
using Xunit.Abstractions;

namespace Tests.Support {
	public class ServiceComponentTest {

		protected readonly Serilog.ILogger Logger;

		public ServiceComponentTest(ITestOutputHelper output) {
			var loggerConfiguration = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.Sink(new TestOutputSink(output));

			Logger = loggerConfiguration.CreateLogger();
		}
	}
}
