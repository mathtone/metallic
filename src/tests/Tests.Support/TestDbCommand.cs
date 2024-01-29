﻿using System.Data;
using System.Data.Common;

namespace Tests.Support{
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

		}

		public override int ExecuteNonQuery() => 0;

		public override object? ExecuteScalar() {
			throw new NotImplementedException();
		}

		public override void Prepare() {

		}

		protected override DbParameter CreateDbParameter() =>
			new TestParameter();

		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior) {
			throw new NotImplementedException();
		}
	}
}