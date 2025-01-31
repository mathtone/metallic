using Metallic.Data.Ado;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;

namespace Metallic.Data.Sql;

public static class IServiceCollectionExtensions {

	public static IServiceCollection AddSqlDb(this IServiceCollection services, string connectionString) =>
		services.AddSqlDb("", connectionString);

	public static IServiceCollection AddSqlDb(this IServiceCollection services, string name, string connectionString) =>
		services.AddSqlDb(new SqlDbConfig() { Name = name, ConnectionString = connectionString });

	public static IServiceCollection AddSqlDb(this IServiceCollection services, SqlDbConfig config) {
		services.AddSingleton(config);
		if (!string.IsNullOrEmpty(config.Name)) {
			services
				.AddKeyedSingleton(config.Name, config)
				.AddKeyedTransient(config.Name, (svc, key) => new SqlConnection(svc.GetKeyedService<SqlDbConfig>(key)!.ConnectionString))
				.AddKeyedTransient<DbConnection>(config.Name, (svc, key) => new SqlConnection(svc.GetKeyedService<SqlDbConfig>(key)!.ConnectionString))
				.AddKeyedTransient<IDbConnection>(config.Name, (svc, key) => new SqlConnection(svc.GetKeyedService<SqlDbConfig>(key)!.ConnectionString));
		}
		return services;
	}

	public static IServiceCollection AddSqlDbConnector(this IServiceCollection services) =>
		services.AddSingleton<ISqlDbConnector, SqlDbConnector>();
}

public static class SqlConnectionExtensions {
	public static SqlCommand CreateCommand<CN>(this CN cn, string commandText, CommandType type) where CN : IDbConnection => cn.CreateCommand<SqlCommand>(commandText, type);
	public static SqlCommand TextCommand<CN>(this CN cn, string commandText) where CN : IDbConnection => cn.CreateCommand(commandText, CommandType.Text);
	public static SqlCommand ProcCommand<CN>(this CN cn, string commandText) where CN : IDbConnection => cn.CreateCommand(commandText, CommandType.StoredProcedure);
	public static SqlCommand TableCommand<CN>(this CN cn, string commandText) where CN : IDbConnection => cn.CreateCommand(commandText, CommandType.TableDirect);
}