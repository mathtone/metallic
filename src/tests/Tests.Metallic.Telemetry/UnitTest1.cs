using Metallic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Metallic.Async;
using Xunit.Abstractions;
using Serilog.Core;
using Serilog.Events;
using Serilog;
using Tests.Support;

namespace Tests.Metallic.Telemetry {

	

	public class UnitTest1(ITestOutputHelper output) : ServiceComponentTest(output) {

		[Fact]
		public async Task Test1Async() {

			var svc = new ServiceCollection()
				.AddLogging(builder => builder.AddSerilog(Logger))
				.AddSingleton(typeof(ITelemetry<>), typeof(TestTelemetry<>))
				.BuildServiceProvider();

			var data = Enumerable.Range(0, 100)
				.ToAsyncEnumerable();

			var telemetry = svc.GetRequiredService<ITelemetry<UnitTest1>>();
			var summary = await telemetry.SummarizeAsync(
				"Test",
				() => data
					.Process(i=>telemetry.Collect("Test", i))
					.Process(async data => await data.CountAsync())
			);
			Assert.Equal(100, summary.Result);
		}
	}	
}