using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;

namespace Metallic.Studies.DataAccess;

public class UnitTest1 {

	const string testPwd = "Strong!Passw0rd";
	readonly string connectionString = $"Server=localhost,6433;Database=master;User Id=sa;Password={testPwd};TrustServerCertificate=True;";

	[Fact]
	public async Task Test1() {

		await using var cn = new SqlConnection(connectionString);

		await cn.OpenAsync();

	}
}