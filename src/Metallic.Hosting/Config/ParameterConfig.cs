using Metallic.Hosting.Support;

namespace Metallic.Hosting.Config;

public class ParameterConfig : ConfigBase, IParameterConfig {

	public string? Name { get; set; }
	public object? Value { get; set; }

	public ParameterConfig() { }
	public ParameterConfig(object value) {
		this.Value = value;
	}
	public ParameterConfig(string name, object value) {
		this.Name = name;
		this.Value = value;
	}
}

public interface IParameterConfig : IConfig {
	string? Name { get; set; }
	object? Value { get; set; }
}