
using Metallic.Async;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Tests.Support;
using Xunit.Abstractions;

namespace Sandbox {
	public class SandboxTests : ServiceComponentTest {

		public SandboxTests(ITestOutputHelper output) : base(output) {

		}

		[Fact]
		public async Task TrySomethingAsync() {
			var data = await Enumerable.Range(0, 5)
				.ToAsyncEnumerable()
				.Select(i => new StreamProcessResponse<int>(i, i % 2 == 0))
				.Dispatch(i => !i.IsSuccessful, async i => {
					this.Logger.Information("Error: {item}", i.Item);
					await Task.CompletedTask;
				})
				.Dispatch(i => i.IsSuccessful, async i => {
					this.Logger.Information("Success: {item}", i.Item);
					await Task.CompletedTask;
				})
				.AnyAsync();

			Assert.False(data);
		}
	}

	public interface IItemResult {
		bool IsSuccessful { get; }
		Exception? Exception { get; }
	}

	public interface IItemResult<out I> : IItemResult {
		public I Item { get; }
	}

	public readonly struct StreamProcessResponse<T> : IItemResult<T> {

		public T Item { get; }
		public bool IsSuccessful { get; }
		public Exception? Exception { get; }

		public StreamProcessResponse(T item, bool isSuccessful = true, Exception? exception = default) {
			Item = item;
			IsSuccessful = isSuccessful;
			Exception = exception;
		}
	}
}