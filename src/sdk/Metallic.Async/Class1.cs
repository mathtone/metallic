using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;

namespace Metallic.Async {

	public class StreamWeaver<T> : IAsyncEnumerable<T> {

		private readonly IAsyncEnumerable<T>[] input;
		private readonly Channel<T> channel;

		public StreamWeaver(params IAsyncEnumerable<T>[] input) :
			this(256, input) { }

		public StreamWeaver(int bufferSize, params IAsyncEnumerable<T>[] input) {
			this.input = input;
			channel = Channel.CreateBounded<T>(
				new BoundedChannelOptions(bufferSize) { FullMode = BoundedChannelFullMode.Wait }
			);
		}

		public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) {
			var t = WriteToChannel(cancellationToken);
			await foreach (var i in channel.Reader.ReadAllAsync(cancellationToken))
				yield return i;

			await t;
		}

		async Task WriteToChannel(CancellationToken cancellationToken) {
			await Task.WhenAll(input.Select(i => i.ForEachAwaitAsync(async m => await channel.Writer.WriteAsync(m, cancellationToken))));
			channel.Writer.Complete();
		}
	}

	public static class AsyncEnumerableExtensions {

		public static IAsyncEnumerable<T> Weave<T>(this IEnumerable<IAsyncEnumerable<T>> streams) =>
			new StreamWeaver<T>(streams.ToArray());

		public static IAsyncEnumerable<T> Weave<T>(this IAsyncEnumerable<T>[] streams) =>
			new StreamWeaver<T>(streams);

	}


	//public class StreamWeaver<T> : IAsyncEnumerable<T> {

	//	private readonly IAsyncEnumerable<T>[] input;
	//	private readonly Channel<T> channel;

	//	public StreamWeaver(int bufferSize, params IAsyncEnumerable<T>[] input) {
	//		this.input = input;
	//		channel = Channel.CreateBounded<T>(new BoundedChannelOptions(bufferSize) { FullMode = BoundedChannelFullMode.Wait });
	//	}

	//	public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) {
	//		var t = WriteToChannel(cancellationToken);
	//		await foreach (var i in channel.Reader.ReadAllAsync(cancellationToken))
	//			yield return i;

	//		await t;
	//	}

	//	async Task WriteToChannel(CancellationToken cancellationToken) {
	//		await Task.WhenAll(input.Select(i => i.ForEachAwaitAsync(async m => await channel.Writer.WriteAsync(m, cancellationToken))));
	//		channel.Writer.Complete();
	//	}

	//	//public static IAsyncEnumerable<OUT> Create<OUT>(int bufferSize, params IAsyncEnumerable<OUT>[] streams) =>
	//	//	new StreamWeaver<OUT>(bufferSize,streams);
	//}

	//public static class AsyncEnumerableExtensions {
		
	//	public static IAsyncEnumerable<T> Weave<T>(params IAsyncEnumerable<T>[] streams) =>
	//		new StreamWeaver<T>(64, streams.ToArray());

	//	//public static IAsyncEnumerable<T> Weave<T>(this IAsyncEnumerable<T> input, params IAsyncEnumerable<T>[] streams) =>
	//	//	StreamWeaver<T>.Create(streams.Prepend(input).ToArray());
	//}
}