using Metallic.Data;
using Metallic.Data.Npgsql;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using Tests.Metallic.Data.Npgsql.Support;

namespace Tests.Metallic.Data.Npgsql {

	public class NpgsqlConnectionTests : IClassFixture<NpgsqlTestFixture> {

		private readonly NpgsqlTestFixture fixture;

		public NpgsqlConnectionTests(NpgsqlTestFixture fixture) =>
			this.fixture = fixture;

		[Fact]
		public async Task Execute_TextCommand() {
			await using var cn = await fixture.Connections.OpenConnection();
			await using var cmd = cn.TextCommand("SELECT 1");
			var rslt = await cmd.Execute(r => (int)r[0]).ToArrayAsync();
			Assert.Equal(new[] { 1 }, rslt);
		}

		[Fact]
		public async Task Create_TextCommand() {
			await using var cn = fixture.Connections.CreateConnection();
			await using var cmd = cn.TextCommand("SELECT 1");
			Assert.Equal("SELECT 1", cmd.CommandText);
			Assert.Equal(CommandType.Text, cmd.CommandType);
		}

		[Fact]
		public async Task Create_ProcCommand() {
			await using var cn = fixture.Connections.CreateConnection();
			await using var cmd = cn.ProcCommand("sp_someproc");
			Assert.Equal("sp_someproc", cmd.CommandText);
			Assert.Equal(CommandType.StoredProcedure, cmd.CommandType);
		}

		[Fact]
		public async Task Create_TableCommand() {
			await using var cn = fixture.Connections.CreateConnection();
			await using var cmd = cn.TableCommand("test_table");
			Assert.Equal("test_table", cmd.CommandText);
			Assert.Equal(CommandType.TableDirect, cmd.CommandType);
		}

		[Fact]
		public async Task WithInput() {
			await using var cn = await fixture.Connections.OpenConnection();
			await using var cmd = cn
				.TextCommand("SELECT 1 where @param > 0")
				.WithInput("@param", 1);
			var rslt = await cmd.Execute(r => (int)r[0]).ToArrayAsync();
			Assert.Equal(new[] { 1 }, rslt);
		}

		[Fact]
		public async Task WithOutput() {
			
			await using var cmd = new NpgsqlCommand()
				.WithOutput("@param",NpgsqlDbType.Integer);
			
			Assert.Equal(ParameterDirection.Output, cmd.Parameters[0].Direction);
		}

		[Fact]
		public async Task WithInputOutput() {

			await using var cmd = new NpgsqlCommand()
				.WithInputOutput("@param", 1);

			Assert.Equal(ParameterDirection.InputOutput, cmd.Parameters[0].Direction);
		}

		[Theory]
		[InlineData(true, "THIs tes''t PARAM")]
		[InlineData(false, "THIs tes't PARAM")]
		public async Task WithTemplate(bool sanitize, string expected) {

			await using var cmd = new NpgsqlCommand("THIs <@param> PARAM", default)
				.WithTemplate("<@param>", "tes't",sanitize);
			Assert.Equal(expected, cmd.CommandText);
		}
	}
}