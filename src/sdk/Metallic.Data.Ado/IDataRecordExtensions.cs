using System.Data;

namespace Metallic.Data.Ado;

public static class IDataRecordExtensions {
	public static T Field<T>(this IDataRecord data, string name, Func<object?, T> selector) => selector(ToNull(data[name]));
	public static T Field<T>(this IDataRecord data, int index, Func<object?, T> selector) => selector(ToNull(data[index]));
	public static T? Field<T>(this IDataRecord data, string name) => (T?)Convert.ChangeType(ToNull(data[name]), typeof(T));
	public static T? Field<T>(this IDataRecord data, int index) => (T?)Convert.ChangeType(ToNull(data[index]), typeof(T));
	public static object? ToNull(object value) => value == Convert.DBNull ? null : value;
}
