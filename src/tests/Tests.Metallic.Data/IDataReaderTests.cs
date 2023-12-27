using System.Data;
using Metallic.Data;
using Tests.Metallic.Data.Support;
namespace Tests.Metallic.Data {

	public class IDataReaderTests {

		[Fact]
		public async Task DbReader_ReadAll() {

			var r = await new TestDbDataReader()
				.ReadAll()
				.Select(r => (int)r[0])
				.ToArrayAsync();

			Assert.Equal(new[] { 1, 2, 3 }, r);
		}

		[Fact]
		public async Task DbReader_Disposes() {
			var disposed = false;
			var r = await new TestDbDataReader(() => disposed = true)
				.ReadAll()
				.Take(2)
				.Select(r => (int)r[0])
				.ToArrayAsync();

			Assert.Equal(new[] { 1, 2, }, r);
			Assert.True(disposed);
		}

		[Fact]
		public async Task IDataReader_Disposes() {
			var disposed = false;
			var r = await new TestIDataReader(() => disposed = true)
				.ReadAll()
				.Take(2)
				.Select(r => (int)r[0])
				.ToArrayAsync();

			Assert.Equal(new[] { 1, 2, }, r);
			Assert.True(disposed);
		}

		[Fact]
		public async Task DbReader_Foreach() {

			var c = 0;
			await new TestDbDataReader()
			   .ReadAll()
			   .Select(r => (int)r[0])
			   .ForEachAsync(r => ++c);

			Assert.Equal(3, c);
		}

		[Fact]
		public async Task DbReader_as_IReader_ReadAll() {

			var rdr = (IDataReader)new TestDbDataReader();
			var rslt = await rdr
				.ReadAll()
				.Select(r => (int)r[0])
				.ToArrayAsync();

			Assert.Equal(new[] { 1, 2, 3 }, rslt);
		}

		[Fact]
		public async Task IDataReaderReader_ReadAll() {

			var r = await new TestIDataReader()
				.ReadAll()
				.Select(r => (int)r[0]).ToArrayAsync();

			Assert.Equal(new[] { 1, 2, 3 }, r);
		}
	}
}