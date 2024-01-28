using Metallic.Async;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Tests.Metallic.Async {

	public class AsyncEnumerableExtensionTests(ITestOutputHelper output) {

		[Theory]
		[InlineData(10, 10, 1, 10)]
		[InlineData(100, 10, 10, 10)]
		[InlineData(109, 10, 11, 9)]
		public async Task Streams_Batched(int items, int batchSize, int expectedBatches, int expectedLastBatchSize) {
			var data = await Enumerable.Range(0, items)
				.ToAsyncEnumerable()
				.Batch(batchSize)
				.ToArrayAsync();

			Assert.Equal(expectedBatches, data.Length);
			Assert.Equal(expectedLastBatchSize, await data[^1]!.CountAsync());
		}


		[Fact]
		public async Task Aggregate_Streams_Interleaved() {

			var rslt = new[] {
				CreateInterleavedTestData("A", 10, 50, 200),
				CreateInterleavedTestData("B", 10, 100, 200),
				CreateInterleavedTestData("C", 10, 150, 200)
			}.Interleave();

			var lastSeen = new Dictionary<string, int>();

			await foreach (var item in rslt) {

				var parts = item.Split(": ");
				var streamName = parts[0];
				var number = int.Parse(parts[1]);

				if (lastSeen.TryGetValue(streamName, out var lastNumber)) {
					Assert.True(number > lastNumber, $"Stream {streamName} is not interleaved properly.");
				}
				lastSeen[streamName] = number;
				output.WriteLine(item);
			}
		}

		[Fact]
		public async Task Stream_ReportEvery() {
			var reports = new List<long>();
			var data = await Enumerable.Range(0, 35)
				.ToAsyncEnumerable()
				.ReportEvery(10, reports.Add)
				.ToArrayAsync();

			Assert.Equal([10L, 20L, 30L, 35L], reports);
		}

		[Fact]
		public async Task Stream_ReportEvery_Async() {
			var reports = new List<long>();
			var data = await Enumerable.Range(0, 35)
				.ToAsyncEnumerable()
				.ReportEvery(10, c => Task.Run(() => reports.Add(c)))
				.ToArrayAsync();

			Assert.Equal([10L, 20L, 30L, 35L], reports);
		}

		[Fact]
		public async Task Stream_ToEach() {
			var reports = new List<long>();
			var data = await Enumerable.Range(0, 35)
				.ToAsyncEnumerable()
				.ToEachAsync(c => {
					reports.Add(c);
					return Task.CompletedTask;
				})
				.ToArrayAsync();

			Assert.Equal(35, reports.Count);
		}

		[Fact]
		public async Task Through() {
			var data = await Enumerable.Range(0, 35)
				.ToAsyncEnumerable()
				.StreamThrough(i => i)
				.ToArrayAsync();

			Assert.Equal(35, data.Length);
		}

		[Fact]
		public async Task Dispatch() {
			var data = Enumerable.Range(0, 35)
				.ToAsyncEnumerable()
				.Dispatch(i => i % 2 == 0, i => Task.CompletedTask);

			Assert.Equal(17, await data.CountAsync());
		}

		protected static async IAsyncEnumerable<string> CreateInterleavedTestData(string name, int count, int initialDelay, int subsequentDelay, [EnumeratorCancellation] CancellationToken cancellationToken = default) {
			await Task.Delay(initialDelay, cancellationToken);
			for (int i = 0; i < count; i++) {
				yield return $"{name}: {i}";
				await Task.Delay(subsequentDelay, cancellationToken);
			}
		}
	}
}