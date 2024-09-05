using Microsoft.Extensions.DependencyInjection;

namespace Metallic.ModularHost;

public static class ServiceConfigCollectionExtensions {

	public static IList<ServiceConfiguration> Singleton<SVC>(this IList<ServiceConfiguration> services, bool addHosting = false) => services
		.Singleton<ServiceConfiguration, SVC>(addHosting);

	public static IList<CFG> Singleton<CFG, SVC>(this IList<CFG> services, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Singleton<CFG, SVC, SVC>(addHosting);


	public static IList<ServiceConfiguration> Singleton<SVC, IMPL>(this IList<ServiceConfiguration> services, bool addHosting = false) => services
		.Singleton<ServiceConfiguration, SVC, IMPL>(addHosting);

	public static IList<CFG> Singleton<CFG, SVC, IMPL>(this IList<CFG> services, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Add<CFG, SVC, IMPL>(ServiceLifetime.Singleton, addHosting);

	public static IList<ServiceConfiguration> Singleton<IMPL>(this IList<ServiceConfiguration> services, IEnumerable<Type> types, bool addHosting = false) => services
		.Singleton<ServiceConfiguration, IMPL>(types, addHosting);

	public static IList<CFG> Singleton<CFG, IMPL>(this IList<CFG> services, IEnumerable<Type> types, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Add<CFG, IMPL>(ServiceLifetime.Singleton, types, addHosting);


	public static IList<ServiceConfiguration> Scoped<SVC>(this IList<ServiceConfiguration> services, bool addHosting = false) => services
		.Scoped<ServiceConfiguration, SVC>(addHosting);

	public static IList<CFG> Scoped<CFG, SVC>(this IList<CFG> services, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Scoped<CFG, SVC, SVC>(addHosting);


	public static IList<ServiceConfiguration> Scoped<SVC, IMPL>(this IList<ServiceConfiguration> services, bool addHosting = false) => services
		.Scoped<ServiceConfiguration, SVC, IMPL>(addHosting);

	public static IList<CFG> Scoped<CFG, SVC, IMPL>(this IList<CFG> services, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Add<CFG, SVC, IMPL>(ServiceLifetime.Scoped, addHosting);

	public static IList<ServiceConfiguration> Scoped<IMPL>(this IList<ServiceConfiguration> services, IEnumerable<Type> types, bool addHosting = false) => services
		.Scoped<ServiceConfiguration, IMPL>(types, addHosting);

	public static IList<CFG> Scoped<CFG, IMPL>(this IList<CFG> services, IEnumerable<Type> types, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Add<CFG, IMPL>(ServiceLifetime.Scoped, types, addHosting);


	public static IList<ServiceConfiguration> Transient<SVC>(this IList<ServiceConfiguration> services, bool addHosting = false) => services
		.Transient<ServiceConfiguration, SVC>(addHosting);

	public static IList<CFG> Transient<CFG, SVC>(this IList<CFG> services, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Transient<CFG, SVC, SVC>(addHosting);

	public static IList<ServiceConfiguration> Transient<SVC, IMPL>(this IList<ServiceConfiguration> services, bool addHosting = false) => services
		.Transient<ServiceConfiguration, SVC, IMPL>(addHosting);

	public static IList<CFG> Transient<CFG, SVC, IMPL>(this IList<CFG> services, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Add<CFG, SVC, IMPL>(ServiceLifetime.Transient, addHosting);


	public static IList<ServiceConfiguration> Transient<IMPL>(this IList<ServiceConfiguration> services, IEnumerable<Type> types, bool addHosting = false) => services
		.Transient<ServiceConfiguration, IMPL>(types, addHosting);

	public static IList<CFG> Transient<CFG, IMPL>(this IList<CFG> services, IEnumerable<Type> types, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Add<CFG, IMPL>(ServiceLifetime.Transient, types, addHosting);


	public static IList<ServiceConfiguration> Activate<IMPL>(this IList<ServiceConfiguration> services, ServiceLifetime? lifetime, params object[] parameters) => services
		.Activate<ServiceConfiguration, IMPL>(lifetime, false, parameters);

	public static IList<CFG> Activate<CFG, IMPL>(this IList<CFG> services, ServiceLifetime? lifetime, params object[] parameters)
		where CFG : ServiceConfiguration, new() =>
			services.Activate<CFG, IMPL, IMPL>(lifetime, false, parameters);


	public static IList<ServiceConfiguration> Activate<SVC, IMPL>(this IList<ServiceConfiguration> services, ServiceLifetime? lifetime, params object[] parameters) => services
		.Activate<ServiceConfiguration, SVC, IMPL>(lifetime, false, parameters);

	public static IList<CFG> Activate<CFG, SVC, IMPL>(this IList<CFG> services, ServiceLifetime? lifetime, params object[] parameters)
		where CFG : ServiceConfiguration, new() => services
			.Activate<CFG, IMPL>(lifetime, [typeof(SVC)], false, parameters);


	public static IList<ServiceConfiguration> Activate<IMPL>(this IList<ServiceConfiguration> services, ServiceLifetime? lifetime, IEnumerable<Type> types, bool addHosting = false, params object[] parameters) => services
		.Activate<ServiceConfiguration, IMPL>(lifetime, types, addHosting, parameters);

	public static IList<CFG> Activate<CFG, IMPL>(this IList<CFG> services, ServiceLifetime? lifetime, IEnumerable<Type> types, bool addHosting = false, params object[] parameters)
		where CFG : ServiceConfiguration, new() {
		services.Add(new() {
			Lifetime = lifetime,
			ServiceTypes = types.ToList(),
			ImplementationType = typeof(IMPL),
			AddHosting = addHosting,
			ActivatorParams = parameters
		});
		return services;
	}


	public static IList<ServiceConfiguration> Host<IMPL>(this IList<ServiceConfiguration> services) =>
		services.Host<ServiceConfiguration, IMPL>();

	public static IList<CFG> Host<CFG, IMPL>(this IList<CFG> services)
		where CFG : ServiceConfiguration, new() {
		services.Add(new() {
			ImplementationType = typeof(IMPL),
			AddHosting = true
		});
		return services;
	}


	public static IList<ServiceConfiguration> Add<SVC>(this IList<ServiceConfiguration> services, ServiceLifetime lifetime, bool addHosting = false) => services
		.Add<ServiceConfiguration, SVC>(lifetime, addHosting);

	public static IList<CFG> Add<CFG, SVC>(this IList<CFG> services, ServiceLifetime lifetime, bool addHosting = false)
		where CFG : ServiceConfiguration, new() => services
			.Add<CFG, SVC, SVC>(lifetime, addHosting);


	public static IList<ServiceConfiguration> Add<SVC, IMPL>(this IList<ServiceConfiguration> services, ServiceLifetime lifetime, bool addHosting = false) => services
		.Add<ServiceConfiguration, SVC, IMPL>(lifetime, addHosting);

	public static IList<CFG> Add<CFG, SVC, IMPL>(this IList<CFG> services, ServiceLifetime lifetime, bool addHosting = false)
		where CFG : ServiceConfiguration, new() {

		services.Add(new() {
			Lifetime = lifetime,
			ServiceTypes = { typeof(SVC) },
			ImplementationType = typeof(IMPL),
			AddHosting = addHosting
		});
		return services;
	}


	public static IList<ServiceConfiguration> Add<IMPL>(this IList<ServiceConfiguration> services, ServiceLifetime lifetime, IEnumerable<Type> types, bool addHosting = false) =>
		services.Add<ServiceConfiguration, IMPL>(lifetime, types, addHosting);

	public static IList<CFG> Add<CFG, IMPL>(this IList<CFG> services, ServiceLifetime lifetime, IEnumerable<Type> types, bool addHosting = false)
		where CFG : ServiceConfiguration, new() {
		services.Add(new() {
			Lifetime = lifetime,
			ServiceTypes = types.ToList(),
			ImplementationType = typeof(IMPL),
			AddHosting = addHosting
		});

		return services;
	}
}