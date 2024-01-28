using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Tests.Support;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Metallic {
	public class LoggingServiceTest(ITestOutputHelper output) : ServiceComponentTest(output) {

		[Fact]
		public void Log_To_Output() {
			//Logger.LogInformation("Hello World!");
			Assert.True(true);
		}

		//protected override ServiceProvider CreateServiceProvider() => new ServiceCollection()
		//	.AddLogging()
		//	.AddSerilog(cfg => cfg.WriteTo.Sink(this))
		//	.BuildServiceProvider();
	}
}