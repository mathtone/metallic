using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data.Common;
using System.Data;

namespace Metallic.Data.Npgsql;

public static class IServiceCollectionExtensions {

	public static IServiceCollection AddNpgsqlDb(this IServiceCollection services, string connectionString) =>
		services.AddNpgsqlDb("", connectionString);

	public static IServiceCollection AddNpgsqlDb(this IServiceCollection services, string name, string connectionString) =>
		services.AddNpgsqlDb(new NpgsqlDbConfig() { Name = name, ConnectionString = connectionString });

	public static IServiceCollection AddNpgsqlDb(this IServiceCollection services, NpgsqlDbConfig config) {
		services.AddSingleton(config);
		if (!string.IsNullOrEmpty(config.Name)) {
			services
				.AddKeyedSingleton(config.Name, config)
				.AddKeyedTransient(config.Name, (svc, key) => new NpgsqlConnection(svc.GetKeyedService<NpgsqlDbConfig>(key)!.ConnectionString))
				.AddKeyedTransient<DbConnection>(config.Name, (svc, key) => new NpgsqlConnection(svc.GetKeyedService<NpgsqlDbConfig>(key)!.ConnectionString))
				.AddKeyedTransient<IDbConnection>(config.Name, (svc, key) => new NpgsqlConnection(svc.GetKeyedService<NpgsqlDbConfig>(key)!.ConnectionString));
		}
		return services;
	}

	public static IServiceCollection AddNpgsqlDbConnector(this IServiceCollection services) =>
		services.AddSingleton<INpgsqlDbConnector, NpgsqlDbConnector>();
}