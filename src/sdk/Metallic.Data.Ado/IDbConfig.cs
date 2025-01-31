namespace Metallic.Data.Ado;

public interface IDbConfig {
	string Name { get; set; }
	string ConnectionString { get; set; }
}

public abstract class DbConfig : IDbConfig{
	public string Name { get; set; } = string.Empty;
	public string ConnectionString { get; set; } = string.Empty;
}