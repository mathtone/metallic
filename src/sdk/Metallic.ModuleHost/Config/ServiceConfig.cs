using Microsoft.Extensions.DependencyInjection;

namespace Metallic.ModuleHost.Config;

public class ServiceConfig : IServiceConfig {
	public ServiceLifetime? Lifetime { get; set; }
	public IList<Type> ServiceTypes { get; set; } = [];
	public Type? ImplementationType { get; set; }
	public bool AddHosting { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public IList<ParamConfig> ActivatorParameters { get; set; } = [];
}

public interface IServiceConfig {
	ServiceLifetime? Lifetime { get; set; }
	IList<Type> ServiceTypes { get; set; }
	IList<ParamConfig> ActivatorParameters { get; set; }
	Type? ImplementationType { get; set; }
	bool AddHosting { get; set; }
	string? Name { get; set; }
	string? Description { get; set; }
}

