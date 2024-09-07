using Microsoft.Extensions.DependencyInjection;

namespace Metallic.Sandbox.Tests;

public class ServiceConfig : IServiceConfig {
	public ServiceLifetime? Lifetime { get; set; }
	public IList<Type> ServiceTypes { get; set; } = [];
	public Type ImplementationType { get; set; } = typeof(object);
	public bool AddHosting { get; set; }
	public Type PrimaryServiceType => ServiceTypes.FirstOrDefault() ?? ImplementationType;
	public IEnumerable<Type> SecondaryServiceTypes => ServiceTypes.Skip(1);
	public IEnumerable<ParamConfig> ActivatorParams { get; set; } = [];
}

public class ParamConfig {
	public string? Name { get; }
	public Type? Type { get; }
	public object? Value { get; }
}

public interface IServiceConfig {
	IEnumerable<ParamConfig> ActivatorParams { get; set; }
	bool AddHosting { get; set; }
	Type ImplementationType { get; set; }
	ServiceLifetime? Lifetime { get; set; }
	Type PrimaryServiceType { get; }
	IEnumerable<Type> SecondaryServiceTypes { get; }
	IList<Type> ServiceTypes { get; set; }
}