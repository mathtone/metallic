using Metallic.Hosting.Builders;

namespace Tests.Metallic.Hosting;

public class UnitTest1 {

	[Fact]
	public async Task Test1Async() {
		var rslt = await new ServiceConfigBuilder()
			.AddParameter("value1", 1)
			.BuildAsync();

		;
	}
}