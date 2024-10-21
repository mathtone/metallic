namespace Metallic.Host.Config;

public class ArgumentConfig {
	public string? Name { get; set; }
	public Type? Type { get; set; }
	public object? Value { get; set; }
}

public class ArgumentConfig<T> : ArgumentConfig {
	public ArgumentConfig(string? name = default, T? value = default) {
		Name = name;
		Type = typeof(T);
		Value = value;
	}
}