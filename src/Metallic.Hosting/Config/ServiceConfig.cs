using Metallic.Hosting.Support;
using Microsoft.Extensions.DependencyInjection;

namespace Metallic.Hosting.Config;

public class ServiceConfig : ConfigBase, IServiceConfig {
	public ServiceLifetime? Lifetime { get; set; }
	public Type? ServiceType { get; set; }
	public Type? ImplementationType { get; set; }
	public IList<Type> AliasTypes { get; set; } = [];
	public IList<IParameterConfig> ActivationParameters { get; set; } = [];
}

public interface IServiceConfig : IConfig {
	ServiceLifetime? Lifetime { get; set; }
	Type? ServiceType { get; set; }
	Type? ImplementationType { get; set; }
	IList<Type> AliasTypes { get; set; }
	IList<IParameterConfig> ActivationParameters { get; set; }
}