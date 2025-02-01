using System.Collections;
using System.Collections.Generic;

namespace Metallic.Graphs;

/// <summary>
/// A single node in the Graph, holding its value and all connections.
/// </summary>
public class Node<T>(T value) {
	public T Value { get; set; } = value;
	private readonly HashSet<Edge<T>> edges = [];

	public IEnumerable<Edge<T>> Edges => edges;

	public void AddEdge(Edge<T> edge) => edges.Add(edge);
}

/// <summary>
/// A connection between two nodes, potentially weighted.
/// </summary>
public class Edge<T>(Node<T> from, Node<T> to, double weight = 1, bool isDirected = false) {
	public Node<T> From { get; } = from;
	public Node<T> To { get; } = to;
	public double Weight { get; } = weight;
	public bool IsDirected { get; } = isDirected;
}

/// <summary>
/// The master of the web. The overseer of all nodes and connections.
/// </summary>
public class Graph<T> : IEnumerable<Node<T>> {
	private readonly HashSet<Node<T>> nodes = [];

	public void AddNode(Node<T> node) => nodes.Add(node);

	public Edge<T> Connect(Node<T> from, Node<T> to, double weight = 1, bool isDirected = false) {
		var edge = new Edge<T>(from, to, weight, isDirected);
		from.AddEdge(edge);
		if (!isDirected) to.AddEdge(new Edge<T>(to, from, weight, isDirected));
		return edge;
	}

	public IEnumerable<Edge<T>> Edges => nodes.SelectMany(n => n.Edges).Distinct();
	public IEnumerator<Node<T>> GetEnumerator() => nodes.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}