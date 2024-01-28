using Serilog.Core;
using Serilog.Events;
using Xunit.Abstractions;

namespace Tests.Support {
	public class TestOutputSink(ITestOutputHelper testOutputHelper) : ILogEventSink {


		public void Emit(LogEvent logEvent) {
			if (logEvent == null) return;

			var message = logEvent.RenderMessage();
			testOutputHelper.WriteLine(message);
		}
	}
}
