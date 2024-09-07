namespace Metallic.ModuleHost.Config;

public class InitializerConfig : IInitializerConfig {
	public Type? Type { get; set; }
	public string MethodName { get; set; } = string.Empty;
}

public interface IInitializerConfig {
	Type? Type { get; set; }
	string MethodName { get; set; }
}
