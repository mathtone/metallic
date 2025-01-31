using System.Data;
using System.Data.Common;

namespace Metallic.Data.Ado;

public static class DbCommandExtensions {
	public static CMD WithParameter<CMD, T>(this CMD command, string name, T value, ParameterDirection direction, DbType type, int size = default)
		where CMD : DbCommand {
		var p = command.CreateParameter();
		p.ParameterName = name;
		p.Value = value;
		p.Direction = direction;
		p.Size = size;
		p.DbType = type;
		return command.WithParameter(p);
	}

	public static CMD WithInput<CMD, T>(this CMD command, string name, T value, DbType type, int size = default)
		where CMD : DbCommand =>
		command.WithParameter(name, value, ParameterDirection.Input, type, size);

	public static async Task<T> ExecuteScalarAsync<CMD, T>(this CMD cmd, Func<object?, T> selector) where CMD : DbCommand =>
		selector(await cmd.ExecuteScalarAsync());
}
