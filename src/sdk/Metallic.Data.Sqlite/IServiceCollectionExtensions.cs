using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;

namespace Metallic.Data.Sqlite;

public static class IServiceCollectionExtensions {

	public static IServiceCollection AddSqliteDb(this IServiceCollection services, string connectionString) =>
		services.AddSqliteDb("", connectionString);

	public static IServiceCollection AddSqliteDb(this IServiceCollection services, string name, string connectionString) =>
		services.AddSqliteDb(new SqliteDbConfig() { Name = name, ConnectionString = connectionString });

	public static IServiceCollection AddSqliteDb(this IServiceCollection services, SqliteDbConfig config) {
		services.AddSingleton(config);
		if (!string.IsNullOrEmpty(config.Name)) {
			services
				.AddKeyedSingleton(config.Name, config)
				.AddKeyedTransient(config.Name, (svc, key) => new SqliteConnection(svc.GetKeyedService<SqliteDbConfig>(key)!.ConnectionString))
				.AddKeyedTransient<DbConnection>(config.Name, (svc, key) => new SqliteConnection(svc.GetKeyedService<SqliteDbConfig>(key)!.ConnectionString))
				.AddKeyedTransient<IDbConnection>(config.Name, (svc, key) => new SqliteConnection(svc.GetKeyedService<SqliteDbConfig>(key)!.ConnectionString));
		}
		return services;
	}

	public static IServiceCollection AddSqliteDbConnector(this IServiceCollection services) =>
		services.AddSingleton<ISqliteDbConnector, SqliteDbConnector>();
}