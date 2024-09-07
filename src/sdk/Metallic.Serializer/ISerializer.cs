namespace Metallic.Serializer;

public interface ISerializer {
	string Serialize<T>(T value);
	T Deserialize<T>(string value);
}

public abstract class SerializerBase : ISerializer {
	public abstract string Serialize<T>(T value);
	public abstract T Deserialize<T>(string value);
}