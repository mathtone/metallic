using System.Text.Json;
using System.Text.Json.Serialization;

namespace Metallic.Serializer.Json.Converters;

public class TypeJsonConverter : JsonConverter<Type> {

	public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Type
		.GetType(reader.GetString()!)!;

	public override void Write(Utf8JsonWriter writer, Type typeValue, JsonSerializerOptions options) => writer
		.WriteStringValue(typeValue.AssemblyQualifiedName);
}