using System.Data;
using System.Data.Common;

namespace Tests.Support {
	public class TestDbConnection : DbConnection {

		public override string ConnectionString { get; set; }
		public override string Database { get; }
		public override string DataSource { get; }
		public override string ServerVersion { get; }
		public override ConnectionState State { get; }

		public override void ChangeDatabase(string databaseName) {
			throw new NotImplementedException();
		}

		public override void Close() {
			//throw new NotImplementedException();
		}

		public override void Open() {
			//throw new NotImplementedException();
		}

		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) => new TestDbTransaction();

		protected override DbCommand CreateDbCommand() => new TestDbCommand();
	}
}