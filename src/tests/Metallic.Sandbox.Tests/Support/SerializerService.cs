namespace Metallic.Sandbox.Tests.Support;


public interface ISerializer {
	string Serialize<T>(T value);
	T Deserialize<T>(string value);
}