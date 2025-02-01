using Metallic.Graphs;

namespace Tests.Metallic.Graphs;
public class TreeTests {
	[Fact]
	public void Test1() {
		var tree = new Tree<string>("0") {
				new("0.0") {
					new("0.0.0"),
					new("0.0.1"),
				},
				new("0.1") {
					new("0.1.0"),
					new("0.1.1")
				}
			};

		Assert.Equal(7, tree.WithDescendants().Count());
	}
}


public class Edge<NODE> {
	public readonly NODE[] Nodes = new NODE[2];
}
public interface IEdge<NODE> {
	public NODE[] Nodes { get; }
}

public class Graph {

}