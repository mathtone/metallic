namespace Metallic.ModularHost;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static Microsoft.Extensions.DependencyInjection.ActivatorUtilities;

//using CFGLIST< = List<ServiceConfiguration>;

public class ServiceConfiguration : IServiceConfiguration {

	public ServiceLifetime? Lifetime { get; set; }
	public IList<Type> ServiceTypes { get; set; } = [];
	public Type ImplementationType { get; set; } = typeof(object);
	public bool AddHosting { get; set; }
	public Type PrimaryServiceType => ServiceTypes.FirstOrDefault() ?? ImplementationType;
	public IEnumerable<Type> SecondaryServiceTypes => ServiceTypes.Skip(1);
	public IEnumerable<object> ActivatorParams { get; set; } = [];

	public IEnumerable<ServiceDescriptor> GetServiceDescriptors() {

		if (Lifetime.HasValue) {
			yield return ActivatorParams.Any() ?
				  new(PrimaryServiceType, svc => CreateInstance(svc, ImplementationType, ActivatorParams.ToArray()), Lifetime.Value) :
				  new(PrimaryServiceType, ImplementationType, Lifetime.Value);

			foreach (var secondary in ServiceTypes.Skip(1)) {
				yield return new(secondary, svc => svc.GetRequiredService(PrimaryServiceType), Lifetime.Value);
			}

			if (AddHosting) {
				yield return new(typeof(IHostedService), svc => svc.GetRequiredService(PrimaryServiceType), ServiceLifetime.Transient);
			}
		}
		else {
			if (AddHosting) {
				yield return ActivatorParams.Any() ?
					new(typeof(IHostedService), svc => CreateInstance(svc, ImplementationType, ActivatorParams.ToArray()), ServiceLifetime.Transient) :
					new(typeof(IHostedService), ImplementationType, ServiceLifetime.Transient);
			}
		}
	}

	public static List<ServiceConfiguration> CreateDefault() => CreateDefault<ServiceConfiguration>();
	public static List<CFG> CreateDefault<CFG>() => [];
}

public interface IServiceConfiguration {
	IEnumerable<object> ActivatorParams { get; set; }
	bool AddHosting { get; set; }
	Type ImplementationType { get; set; }
	ServiceLifetime? Lifetime { get; set; }
	Type PrimaryServiceType { get; }
	IEnumerable<Type> SecondaryServiceTypes { get; }
	IList<Type> ServiceTypes { get; set; }

	IEnumerable<ServiceDescriptor> GetServiceDescriptors();
}