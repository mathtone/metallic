using System.Data;

namespace Metallic.Data.Ado;

public static class IDbCommandExtensions {

	public static CMD WithTemplate<CMD>(this CMD command, string tag, string value, TemplateType type = TemplateType.Identifier)
	 where CMD : IDbCommand {

		string safeValue = type switch {
			TemplateType.Identifier => EscapeIdentifier(value),
			TemplateType.Raw => value,
			_ => throw new ArgumentOutOfRangeException(nameof(type))
		};

		command.CommandText = command.CommandText.Replace(tag, safeValue);
		return command;
	}

	private static string EscapeIdentifier(string identifier) {
		if (string.IsNullOrWhiteSpace(identifier))
			throw new ArgumentException("Identifier cannot be empty");

		return "[" + identifier.Replace("]", "]]") + "]";
	}
	public static CMD WithInput<CMD, T>(this CMD command, string name, T value, int size = default)
		where CMD : IDbCommand =>
		command.WithParameter(name, value, ParameterDirection.Input, size);

	public static CMD WithInputOutput<CMD, T>(this CMD command, string name, T value, int size = default)
		where CMD : IDbCommand =>
		command.WithParameter(name, value, ParameterDirection.InputOutput, size);

	public static CMD WithParameter<CMD, T>(this CMD command, string name, T value, ParameterDirection direction, int size = default)
		where CMD : IDbCommand {

		var p = command.CreateParameter();
		p.ParameterName = name;
		p.Value = value;
		p.Direction = direction;
		p.Size = size;
		return command.WithParameter(p);
	}

	public static CMD WithParameter<CMD, P>(this CMD command, P parameter)
		where CMD : IDbCommand
		where P : IDbDataParameter {
		command.Parameters.Add(parameter);
		return command;
	}
}
