using System.Text.Json;
using System.Text.Json.Serialization;

namespace Metallic.Sandbox.Tests.Support;

public interface ISerializerConverter<TYPE> {}

public abstract class JsonSerializerConverter<TYPE> : JsonConverter<TYPE>, ISerializerConverter<TYPE> {}

public class TypeJsonConverter : JsonSerializerConverter<Type> ,ISerializerConverter<Type>{

	public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		Type.GetType(reader.GetString()!)!;

	public override void Write(Utf8JsonWriter writer, Type typeValue, JsonSerializerOptions options) =>
		writer.WriteStringValue(typeValue.AssemblyQualifiedName);
}