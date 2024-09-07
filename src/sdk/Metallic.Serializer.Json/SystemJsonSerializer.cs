using System.Text.Json;

namespace Metallic.Serializer.Json;

public class SystemJsonSerializer(JsonSerializerOptions? options = default) : ISerializer {
	public T Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, options)!;
	public string Serialize<T>(T value) => JsonSerializer.Serialize(value, options);
}