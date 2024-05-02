using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Serilog;
using Metallic.Async;

namespace Tests.Metallic.Telemetry {
	public class TestTelemetry : ITelemetry {

		private readonly Microsoft.Extensions.Logging.ILogger logger;

		public TestTelemetry(Microsoft.Extensions.Logging.ILogger logger) {
			this.logger = logger;
		}

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull => logger.BeginScope(state);

		public IAsyncEnumerable<I> Collect<I>(string name, IAsyncEnumerable<I> stream) =>
			stream.ReportEvery(33, i => this.LogInformation("Processed {count} items.", i));

		public bool IsEnabled(LogLevel logLevel) => logger.IsEnabled(logLevel);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) =>
			logger.Log(logLevel, eventId, state, exception, formatter);


		public R Summarize<T, R>(string name, Func<T> resultFunction) where R : IProcessSummary<T>, new() {
			var duration = Stopwatch.StartNew();
			var rslt = resultFunction();
			duration.Stop();
			return new R() {
				Name = name,
				Result = rslt,
				Duration = duration.Elapsed
			};
		}

		public IProcessSummary<T> Summarize<T>(string name, Func<T> resultFunction) =>
			Summarize<T, ProcessSummary<T>>(name, resultFunction);

		public async Task<IProcessSummary<T>> SummarizeAsync<T>(string name, Func<Task<T>> resultFunction) {
			var duration = Stopwatch.StartNew();
			var rslt = await resultFunction();
			duration.Stop();
			return new ProcessSummary<T>() {
				Name = name,
				Result = rslt,
				Duration = duration.Elapsed
			};
		}
	}

	public class ProcessResult<I> {
		public I Item { get; set; }
	}

	public class TestTelemetry<T> : TestTelemetry, ITelemetry<T> {
		public TestTelemetry(ILogger<T> logger) :
			base(logger) { }
	}

	public interface ITelemetry<T> : ITelemetry, ILogger<T> {

	}

	public interface ITelemetry : Microsoft.Extensions.Logging.ILogger {
		R Summarize<T, R>(string name, Func<T> resultFunction) where R : IProcessSummary<T>, new();
		Task<IProcessSummary<T>> SummarizeAsync<T>(string name, Func<Task<T>> resultFunction);
		IAsyncEnumerable<I> Collect<I>(string name, IAsyncEnumerable<I> stream);
	}


	public interface IProcessSummary {
		string Name { get; set; }
		TimeSpan Duration { get; set; }
	}

	public interface IProcessSummary<T> : IProcessSummary {
		T? Result { get; set; }
	}

	public class ProcessSummary {
		public string Name { get; set; }
		public TimeSpan Duration { get; set; }
	}

	public class ProcessSummary<R> : ProcessSummary, IProcessSummary<R> {
		public R? Result { get; set; }
	}
}