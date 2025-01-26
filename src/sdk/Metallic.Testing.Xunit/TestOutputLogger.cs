using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Metallic.Testing.Xunit;

public class TestOutputLogger(ITestOutputHelper outputHelper) : ILogger {

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

	public bool IsEnabled(LogLevel logLevel) => true;

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
		ArgumentNullException.ThrowIfNull(formatter);
		outputHelper.WriteLine(formatter(state, exception));
	}
}
