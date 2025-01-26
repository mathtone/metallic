using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Metallic.Testing.Xunit;

public class TestOutputLoggerProvider(ITestOutputHelper outputHelper) : ILoggerProvider {
	private bool disposedValue;

	public ILogger CreateLogger(string categoryName) => new TestOutputLogger(outputHelper);

	protected virtual void Dispose(bool disposing) {
		if (!disposedValue) {
			if (disposing) {
				OnDispose();
			}
			disposedValue = true;
		}
	}

	protected virtual void OnDispose() {
	}

	public void Dispose() {
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
