using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;

namespace Metallic.Async {

	public static class AsyncEnumerableExtensions {

		public static IAsyncEnumerable<T> Interleave<T>(this IEnumerable<IAsyncEnumerable<T>> streams, int bufferSize = 16) =>
			streams.ToAsyncEnumerable().Interleave(bufferSize);

		public static async IAsyncEnumerable<T> Interleave<T>(this IAsyncEnumerable<IAsyncEnumerable<T>> streams, int bufferSize = 16) {
			var tasks = new List<Task>();
			var channel = Channel.CreateBounded<T>(
				new BoundedChannelOptions(bufferSize) { FullMode = BoundedChannelFullMode.Wait }
			);

			var inputTask = Task.Run(async () => {
				await foreach (var s in streams) {
					tasks.Add(s.ForEachAwaitAsync(async m => await channel.Writer.WriteAsync(m)));
				}
				await Task.WhenAll(tasks);
				channel.Writer.Complete();
			});

			await foreach (var i in channel.Reader.ReadAllAsync()) {
				yield return i;
			}

			await inputTask;
		}

		public static async IAsyncEnumerable<IAsyncEnumerable<T>> Batch<T>(this IAsyncEnumerable<T> stream, int batchSize) {
			var buffer = new List<T>(batchSize);
			await foreach (var i in stream) {
				buffer.Add(i);
				if (buffer.Count == batchSize) {
					yield return buffer.ToAsyncEnumerable();
					buffer = new List<T>(batchSize);
				}
			}
			if (buffer.Count != 0)
				yield return buffer.ToAsyncEnumerable();
		}

		public static async IAsyncEnumerable<T> ToEachAsync<T>(this IAsyncEnumerable<T> items, Func<T, Task> action) {
			await foreach (var item in items) {
				await action(item);
				yield return item;
			}
		}

		public static IAsyncEnumerable<T> StreamThrough<T>(this IAsyncEnumerable<T> items, Func<IAsyncEnumerable<T>, IAsyncEnumerable<T>> processor) =>
			processor(items);

		public static async IAsyncEnumerable<T> ReportEvery<T>(this IAsyncEnumerable<T> items, long interval, Func<long, Task> action) {
			var count = 0L;
			await foreach (var item in items) {
				if (++count % interval == 0)
					await action(count);
				yield return item;
			}
			if (count % interval != 0)
				await action(count);
		}

		public static async IAsyncEnumerable<T> ReportEvery<T>(this IAsyncEnumerable<T> items, long interval, Action<long> action) {
			var count = 0L;
			await foreach (var item in items) {
				if (++count % interval == 0)
					action(count);
				yield return item;
			}
			if (count % interval != 0)
				action(count);
		}

		public static async IAsyncEnumerable<T> Dispatch<T>(this IAsyncEnumerable<T> stream, Func<T, bool> selector, Func<T, Task> handler) {
			await foreach (var item in stream) {
				if (selector(item)) {
					await handler(item);
				}
				else {
					yield return item;
				}
			}
		}
	}
}