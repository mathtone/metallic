using System.Collections;

namespace Metallic.Graphs;

public class Tree<T>(T? value = default) : IEnumerable<Tree<T>> {
	private readonly List<Tree<T>> children = [];
	private Tree<T>? parent;

	public T? Value { get; set; } = value;

	public Tree<T>? Parent {
		get => parent;
		set => SetParent(value);
	}

	public Tree<T> this[int index] => children[index];

	public Tree<T> Root => Parent == null ? this : Parent.Root;
	public int Level => Parent == null ? 0 : Parent.Level + 1;

	public void SetParent(Tree<T>? newParent) {
		if (newParent == this)
			throw new InvalidOperationException("A node cannot be its own parent.");

		if (parent == newParent) return;
		parent?.children.Remove(this);
		parent = newParent;
		parent?.children.Add(this);
	}

	public void Add(Tree<T> child) => child.SetParent(this);

	public IEnumerable<Tree<T>> WithAncestors() => Ancestors().Prepend(this);
	public IEnumerable<Tree<T>> Ancestors() {
		var current = Parent;
		while (current != null) {
			yield return current;
			current = current.Parent;
		}
	}

	public IEnumerable<Tree<T>> WithDescendants() => Descendants().Prepend(this);
	public IEnumerable<Tree<T>> Descendants() {
		foreach (var child in children) {
			yield return child;
			foreach (var descendant in child.Descendants()) {
				yield return descendant;
			}
		}
	}

	public override string ToString() => Value?.ToString() ?? "null";

	public IEnumerator<Tree<T>> GetEnumerator() => children.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
