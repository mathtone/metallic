using System.Data;
using System.Diagnostics.CodeAnalysis;
namespace Tests.Metallic.Data.Support {

	class TestIDataReader : IDataReader {
		int rowNum = 0;
		private Action? disposeAction;

		public TestIDataReader(Action? disposeAction = default) => this.disposeAction = disposeAction;

		public object this[int ordinal] { get => rowNum; }
		public object this[string name] { get => rowNum; }

		public int Depth { get; }
		public bool IsClosed { get; }
		public int RecordsAffected { get; }
		public int FieldCount { get; }

		public void Close() {
			throw new NotImplementedException();
		}

		public void Dispose() {
			if (disposeAction != null) {
				disposeAction();
				disposeAction = null;
			}
		}

		public bool GetBoolean(int i) {
			throw new NotImplementedException();
		}

		public byte GetByte(int i) {
			throw new NotImplementedException();
		}

		public long GetBytes(int i, long fieldOffset, byte[]? buffer, int bufferoffset, int length) {
			throw new NotImplementedException();
		}

		public char GetChar(int i) {
			throw new NotImplementedException();
		}

		public long GetChars(int i, long fieldoffset, char[]? buffer, int bufferoffset, int length) {
			throw new NotImplementedException();
		}

		public IDataReader GetData(int i) {
			throw new NotImplementedException();
		}

		public string GetDataTypeName(int i) {
			throw new NotImplementedException();
		}

		public DateTime GetDateTime(int i) {
			throw new NotImplementedException();
		}

		public decimal GetDecimal(int i) {
			throw new NotImplementedException();
		}

		public double GetDouble(int i) {
			throw new NotImplementedException();
		}

		[return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)]
		public Type GetFieldType(int i) {
			throw new NotImplementedException();
		}

		public float GetFloat(int i) {
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i) {
			throw new NotImplementedException();
		}

		public short GetInt16(int i) {
			throw new NotImplementedException();
		}

		public int GetInt32(int i) {
			throw new NotImplementedException();
		}

		public long GetInt64(int i) {
			throw new NotImplementedException();
		}

		public string GetName(int i) {
			throw new NotImplementedException();
		}

		public int GetOrdinal(string name) {
			throw new NotImplementedException();
		}

		public DataTable? GetSchemaTable() {
			throw new NotImplementedException();
		}

		public string GetString(int i) {
			throw new NotImplementedException();
		}

		public object GetValue(int i) {
			throw new NotImplementedException();
		}

		public int GetValues(object[] values) {
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i) {
			throw new NotImplementedException();
		}

		public bool NextResult() {
			throw new NotImplementedException();
		}

		public bool Read() => ++rowNum <= 3;
	}
}