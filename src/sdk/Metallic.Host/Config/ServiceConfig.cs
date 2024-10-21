using Microsoft.Extensions.DependencyInjection;

namespace Metallic.Host.Config;

public class ServiceConfig {
	public string? Name { get; set; }
	public Type? ServiceType { get; set; }
	public Type? ImplementationType { get; set; }
	public Type? AliasForType { get; set; }
	public object? Instance { get; set; }
	public List<ArgumentConfig> Arguments { get; set; } = [];
	public ServiceLifetime Lifetime { get; set; }
}


