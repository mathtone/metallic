using System.Data;
using Metallic.Data;
using Tests.Metallic.Data.Support;
namespace Tests.Metallic.Data {
	public class DbConnectionTests {
		[Fact]
		public async Task CreateCommand_CreatesCommandAsync() {
			await using var cn = new TestDbConnection();
			await using var cmd = cn.CreateCommand<TestDbConnection, TestDbCommand>("TEST COMMAND",CommandType.Text,60);
			Assert.Equal("TEST COMMAND", cmd.CommandText);
			Assert.Equal(CommandType.Text,cmd.CommandType);
			Assert.Equal(60, cmd.CommandTimeout);
		}
	}
}