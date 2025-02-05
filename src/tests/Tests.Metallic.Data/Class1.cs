using Metallic.Data;

namespace Tests.Metallic.Data;

public class RepositoryTests {

	[Fact]
	public void Test() {
		var repo = new DictionaryRepo<int, string>();
		
	}
}

public class DictionaryRepo<KEY, VALUE> : IRepository<KEY, VALUE> where VALUE : class {
	public VALUE Create(VALUE item) {
		throw new NotImplementedException();
	}

	public bool Delete(KEY id) {
		throw new NotImplementedException();
	}

	public VALUE Read(KEY id) {
		throw new NotImplementedException();
	}

	public void Update(VALUE item) {
		throw new NotImplementedException();
	}
}