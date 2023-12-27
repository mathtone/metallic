using System.Data;
using System.Data.Common;
namespace Tests.Metallic.Data.Support {
	class TestDbConnection : DbConnection {
		public override string ConnectionString { get; set; }
		public override string Database { get; }
		public override string DataSource { get; }
		public override string ServerVersion { get; }
		public override ConnectionState State { get; }

		public override void ChangeDatabase(string databaseName) {
			throw new NotImplementedException();
		}

		public override void Close() {
			throw new NotImplementedException();
		}

		public override void Open() {
			throw new NotImplementedException();
		}

		protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel) {
			throw new NotImplementedException();
		}

		protected override DbCommand CreateDbCommand() =>
			new TestDbCommand();
	}

	public class TestDbCommand : DbCommand {
		public override string CommandText { get; set; }
		public override int CommandTimeout { get; set; }
		public override CommandType CommandType { get; set; }
		public override bool DesignTimeVisible { get; set; }
		public override UpdateRowSource UpdatedRowSource { get; set; }
		protected override DbConnection? DbConnection { get; set; }
		protected override DbParameterCollection DbParameterCollection { get; }
		protected override DbTransaction? DbTransaction { get; set; }

		public override void Cancel() {
			throw new NotImplementedException();
		}

		public override int ExecuteNonQuery() {
			throw new NotImplementedException();
		}

		public override object? ExecuteScalar() {
			throw new NotImplementedException();
		}

		public override void Prepare() {
			throw new NotImplementedException();
		}

		protected override DbParameter CreateDbParameter() {
			throw new NotImplementedException();
		}

		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) {
			throw new NotImplementedException();
		}
	}
}