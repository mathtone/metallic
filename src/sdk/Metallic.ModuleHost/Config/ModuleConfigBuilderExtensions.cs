using Microsoft.Extensions.DependencyInjection;

namespace Metallic.ModuleHost.Config;

public static class IModuleConfigBuilderExtensions {

	public static IModuleConfigBuilder AddService<IMPL>(this IModuleConfigBuilder builder, ServiceLifetime lifetime) =>
		builder.AddService<IMPL, IMPL>(lifetime);

	public static IModuleConfigBuilder AddService<SVC, IMPL>(this IModuleConfigBuilder builder, ServiceLifetime lifetime)
		where IMPL : SVC => builder.AddService(c => c.Configure<SVC, IMPL>(lifetime));

	public static IModuleConfigBuilder AddSingleton<IMPL>(this IModuleConfigBuilder builder) =>
		builder.AddSingleton<IMPL, IMPL>();

	public static IModuleConfigBuilder AddSingleton<SVC, IMPL>(this IModuleConfigBuilder builder) where IMPL : SVC =>
		builder.AddService<SVC, IMPL>(ServiceLifetime.Singleton);

	public static IModuleConfigBuilder AddTransient<IMPL>(this IModuleConfigBuilder builder) =>
		builder.AddTransient<IMPL, IMPL>();

	public static IModuleConfigBuilder AddTransient<SVC, IMPL>(this IModuleConfigBuilder builder) where IMPL : SVC =>
		builder.AddService<SVC, IMPL>(ServiceLifetime.Transient);

	public static IModuleConfigBuilder AddScoped<IMPL>(this IModuleConfigBuilder builder) =>
		builder.AddScoped<IMPL, IMPL>();

	public static IModuleConfigBuilder AddScoped<SVC, IMPL>(this IModuleConfigBuilder builder) where IMPL : SVC =>
		builder.AddService<SVC, IMPL>(ServiceLifetime.Scoped);
}