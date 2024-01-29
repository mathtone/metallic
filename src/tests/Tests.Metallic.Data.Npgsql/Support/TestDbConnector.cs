using Metallic.Data.Npgsql;
using Npgsql;

namespace Tests.Metallic.Data.Npgsql.Support
{
    public class TestDbConnector : ITestDbConnector
    {
        public NpgsqlConnection CreateConnection() => new($"Server=localhost;User Id=postgres;Password=postgres");
    }
    public interface ITestDbConnector : INpgsqlConnector
    {
    }
}