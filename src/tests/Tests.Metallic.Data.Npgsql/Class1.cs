using Metallic.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Tests.Metallic.Data.Npgsql.Support;
using Xunit.Abstractions;

namespace Tests.Metallic.Data.Npgsql {
	public class NpgsqlConnectionTests(ITestOutputHelper output) : NpgsqlTest(output) {

		[Fact]
		public async Task Open_Connection() {
			await using var cn = GetService<IAsyncDbConnector<NpgsqlConnection>>()!
				.CreateConnection();

			await cn.OpenAsync();
			Assert.Equal(ConnectionState.Open, cn.State);
			await cn.CloseAsync();
			Assert.Equal(ConnectionState.Closed, cn.State);
		}
	}

	

	
}