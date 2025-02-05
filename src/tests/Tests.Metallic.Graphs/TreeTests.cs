using Metallic.Graphs;
using System.Collections;

namespace Tests.Metallic.Graphs;
public class TreeTests {

	readonly Tree<string> tree = new("0") {
		new ("0.0") {
			new ("0.0.0"),
			new ("0.0.1"),
		},
		new ("0.1") {
			new ("0.1.0"),
			new ("0.1.1")
		}
	};

	[Fact]
	public void Root() =>
		Assert.Equal("0", tree[0][0].Root.Value);

	[Fact]
	public void As_Enumerable() {
		var i = 0;
		foreach(var node in (IEnumerable)tree) {
			i++;
		}
		Assert.Equal(2, i);
	}

	[Fact]
	public void Create_Node() {
		var node = new Tree<int?>(1);
		Assert.Equal("1", node.ToString());
		node.Value = null;
		Assert.Null(node.ToString());
	}

	[Fact]
	public void Set_Parent() {
		var child = new Tree<string>("child") {
			Parent = tree[0]
		};
		Assert.Equal("child", tree[0][2].Value);
		Assert.Equal(tree[0], child.Parent);
	}

	[Fact]
	public void Set_New_Parent() {
		var child = new Tree<string>("child") {
			Parent = tree[0]
		};
		child.Parent = tree[1];
		Assert.Equal("child", tree[1][2].Value);
		Assert.Equal(tree[1], child.Parent);
	}

	[Fact]
	public void Set_Null_Parent() {
		var child = new Tree<string>("child") {
			Parent = tree[0]
		};
		Assert.Equal(3, tree[0].Count());
		child.Parent = null;
		Assert.Equal(2, tree[0].Count());
	}

	[Fact]
	public void Set_Parent_To_Self() {
		var child = new Tree<string>("child");
		Assert.Throws<InvalidOperationException>(() => child.Parent = child);
	}

	[Fact]
	public void Level() =>
		Assert.Equal(2, tree[0][0].Level);

	[Fact]
	public void With_Descendants() =>
		Assert.Equal(7, tree.WithDescendants().Count());

	[Fact]
	public void Ancestors() {
		Assert.Equal(3, tree[0][0].WithAncestors().Count());
		Assert.Equal(3, tree[0][1].WithAncestors().Count());
	}
}