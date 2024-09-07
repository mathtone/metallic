using Metallic.Serializer.Json.Converters;
using System.Text.Json;

namespace Metallic.Serializer.Json.Tests;

public class UnitTest1 {

	[Fact]
	public void Test1() {
		var serializer = new SystemJsonSerializer(Options);
		Assert.Equal(42, serializer.Deserialize<int>("42"));
		Assert.Equal(typeof(string), serializer.Deserialize<Type>(serializer.Serialize(typeof(string))));
		Assert.Equal(typeof(int), serializer.Deserialize<Type>(serializer.Serialize(typeof(int))));
	}

	static readonly JsonSerializerOptions Options = new() {
		Converters = {
			new TypeJsonConverter()
		},
		WriteIndented = true,
		IncludeFields = true
	};
}