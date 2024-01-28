using System.Data;

namespace Metallic.Data {
	public static class DbCommandExtensions {
		public static CMD WithInput<CMD, T>(this CMD command, string name, T value, Func<T?, object>? converter = default)
			where CMD : IDbCommand =>
			command.WithParameter(name, ParameterDirection.Input, value, default, converter);

		public static CMD WithInputOutput<CMD, T>(this CMD command, string name, T value, Func<T?, object>? converter = default)
			where CMD : IDbCommand =>
			command.WithParameter(name, ParameterDirection.InputOutput, value, default, converter);

		public static CMD WithOutput<CMD>(this CMD command, string name, SqlDbType type)
			where CMD : IDbCommand =>
			command.WithParameter<CMD, object>(name, ParameterDirection.Output, default, type);


		public static CMD WithTemplate<CMD>(this CMD command, string name, string value)
			where CMD : IDbCommand {
			command.CommandText = command.CommandText.Replace(name, value);
			return command;
		}

		public static CMD WithParameter<CMD, T>(this CMD command, string name, ParameterDirection direction, T? value = default, SqlDbType? type = default, Func<T?, object>? converter = default) where CMD : IDbCommand {
			var parameter = command.CreateParameter();
			parameter.Direction = direction;
			parameter.ParameterName = name;
			parameter.Value = converter == null ? value : converter(value);
			command.Parameters.Add(parameter);
			return command;
		}
	}
}