using Metallic.Async;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Tests.Metallic.Async{
	public class StreamWeaverTests(ITestOutputHelper output) {

		[Fact]
		public async Task Aggregate_Streams_Interleaved() {

			var rslt = await new StreamWeaver<string>(64,
				CreateInterleavedTestData("A", 10, 50, 200),  // Stream A starts at 50ms, then every 200ms
				CreateInterleavedTestData("B", 10, 100, 200), // Stream B starts at 100ms, then every 200ms
				CreateInterleavedTestData("C", 10, 150, 200)  // Stream C starts at 150ms, then every 200ms
			).ToArrayAsync();

			var lastSeen = new Dictionary<string, int>();
			foreach (var item in rslt) {
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
		public async Task Aggregate_Streams_Interleaved_2() {

			var rslt = new[] {
				CreateInterleavedTestData("A", 10, 50, 200),  // Stream A starts at 50ms, then every 200ms
				CreateInterleavedTestData("B", 10, 100, 200), // Stream B starts at 100ms, then every 200ms
				CreateInterleavedTestData("C", 10, 150, 200)  // Stream C starts at 150ms, then every 200ms
			}.Weave();

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

		protected static async IAsyncEnumerable<string> CreateInterleavedTestData(string name, int count, int initialDelay, int subsequentDelay, [EnumeratorCancellation] CancellationToken cancellationToken = default) {
			await Task.Delay(initialDelay, cancellationToken);
			for (int i = 0; i < count; i++) {
				yield return $"{name}: {i}";
				await Task.Delay(subsequentDelay, cancellationToken);
			}
		}
	}
}