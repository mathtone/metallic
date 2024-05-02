using Serilog.Core;
using Serilog.Events;
using Xunit.Abstractions;

namespace Tests.Support {
	public class TestOutputSink : ILogEventSink {

		private readonly ITestOutputHelper testOutputHelper;

		public TestOutputSink(ITestOutputHelper testOutputHelper) {
			this.testOutputHelper = testOutputHelper;
		}

		public void Emit(LogEvent logEvent) {
			if (logEvent == null) return;

			var message = logEvent.RenderMessage();
			testOutputHelper.WriteLine(message);
		}
	}
}
