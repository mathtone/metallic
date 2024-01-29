using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tests.Support;
using Xunit.Abstractions;

namespace Sandbox {

	public class UnitTest1(ITestOutputHelper output) : ServiceComponentTest(output) {

		[Fact]
		public void Test1() {

		}

		protected override IServiceCollection CreateServices() => base.CreateServices();
	}
}