using Microsoft.Extensions.DependencyInjection;

namespace Metallic.ModuleHost.Config;

public static class ServiceConfigBuilderExtensions {

	public static IServiceConfigBuilder Configure<SVC>(this IServiceConfigBuilder builder, ServiceLifetime lifetime, Action<IServiceConfig>? configAction = default) =>
		builder.Configure<SVC, SVC>(lifetime);

	public static IServiceConfigBuilder Configure<SVC, IMPL>(this IServiceConfigBuilder builder, ServiceLifetime lifetime, Action<IServiceConfig>? configAction = default) where IMPL : SVC =>
		builder.Configure(c => {
			c.Lifetime = lifetime;
			c.ServiceTypes.Add(typeof(SVC));
			c.ImplementationType = typeof(IMPL);
			configAction?.Invoke(c);
		});

	public static IServiceConfigBuilder Describe(this IServiceConfigBuilder builder, string name, string? description = default) =>
		builder.Configure(c => {
			c.Name = name;
			c.Description = description;
		});

	public static IServiceConfigBuilder AddServiceType<T>(this IServiceConfigBuilder builder) =>
		builder.Configure(c => c.ServiceTypes.Add(typeof(T)));

	public static IServiceConfigBuilder AddHosting(this IServiceConfigBuilder builder) =>
		builder.Configure(c => c.AddHosting = true);

	public static IServiceConfigBuilder Singleton<IMPL>(this IServiceConfigBuilder builder, Action<IServiceConfig>? configAction = default) =>
		builder.Singleton<IMPL, IMPL>(configAction);

	public static IServiceConfigBuilder Singleton<SVC, IMPL>(this IServiceConfigBuilder builder, Action<IServiceConfig>? configAction = default)
		where IMPL : SVC => builder.Configure<SVC, IMPL>(ServiceLifetime.Singleton, configAction);

	public static IServiceConfigBuilder Transient<IMPL>(this IServiceConfigBuilder builder, Action<IServiceConfig>? configAction = default) =>
		builder.Transient<IMPL, IMPL>(configAction);

	public static IServiceConfigBuilder Transient<SVC, IMPL>(this IServiceConfigBuilder builder, Action<IServiceConfig>? configAction = default)
		where IMPL : SVC => builder.Configure<SVC, IMPL>(ServiceLifetime.Transient, configAction);

	public static IServiceConfigBuilder Scoped<IMPL>(this IServiceConfigBuilder builder, Action<IServiceConfig>? configAction = default) =>
		builder.Scoped<IMPL, IMPL>(configAction);

	public static IServiceConfigBuilder Scoped<SVC, IMPL>(this IServiceConfigBuilder builder, Action<IServiceConfig>? configAction = default)
		where IMPL : SVC => builder.Configure<SVC, IMPL>(ServiceLifetime.Scoped, configAction);
}