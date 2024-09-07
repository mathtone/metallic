namespace Metallic.ModuleHost.Config;

public class ParamConfig {
	public string? Name { get; set; }
	public Type? Type { get; set; }
	public object? Value { get; set; }
}

public class ParamConfig<T> : ParamConfig {
	public ParamConfig() {
		Type = typeof(T);
	}
}

public interface IParamConfig<out T> : IParamConfig {
	new Type? Type => typeof(T);
}

public interface IParamConfig {
	string? Name { get; set; }
	Type? Type { get; set; }
	object? Value { get; set; }
}