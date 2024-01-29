using System.Data;
using System.Data.Common;

namespace Tests.Support {
	public class TestDbTransaction : DbTransaction {
		public override IsolationLevel IsolationLevel { get; }
		protected override DbConnection? DbConnection { get; }

		public override void Commit() {

		}

		public override void Rollback() {

		}
	}
}