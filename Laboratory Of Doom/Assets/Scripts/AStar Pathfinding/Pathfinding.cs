using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	[Header("DEBUG-ONLY")]
	public Transform seeker;
	public Transform target;

	[Header("Grid of Nodes"), Space]
	[SerializeField] private NodeGrid grid;

	// Private fields.
	private Heap<Node> _open;
	private HashSet<Node> _closed;

	private void Start()
	{
		_open = new Heap<Node>(grid.MaxSize);
		_closed = new HashSet<Node>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			FindPath(seeker.position, target.position);
	}

	void FindPath(Vector3 startPos,  Vector3 endPos)
	{
		Stopwatch sw = new Stopwatch();
		sw.Start();

		Node startNode = grid.FromWorldPosition(startPos);
		Node endNode = grid.FromWorldPosition(endPos);

		_open.Clear();
		_closed.Clear();

		_open.AddLast(startNode);

		while (_open.Count > 0)
		{
			Node current = _open.RemoveFirst();
			_closed.Add(current);

			if (current == endNode)
			{
				sw.Stop();
				UnityEngine.Debug.Log($"Path found in: {sw.ElapsedMilliseconds} ms, or {sw.ElapsedTicks} ticks.");

				ConstructPath(startNode, endNode);
				return;
			}

			foreach (Node neighbor in grid.GetNeighbors(current))
			{
				if (!neighbor.walkable || _closed.Contains(neighbor))
					continue;

				int newCostToNeighbor = current.gCost + GetDistanceBetween(current, neighbor);

				if (newCostToNeighbor < neighbor.gCost || !_open.Contains(neighbor))
				{
					// Set the new F cost.
					neighbor.gCost = newCostToNeighbor;
					neighbor.hCost = GetDistanceBetween(neighbor, endNode);
					neighbor.Parent = current;

					if (!_open.Contains(neighbor))
						_open.AddLast(neighbor);
				}
			}
		}
	}

	private void ConstructPath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node current = endNode;

		while (current != startNode)
		{
			path.Add(current);
			current = current.Parent;
		}

		path.Reverse();
		grid.path = path;
	}

	private int GetDistanceBetween(Node a, Node b)
	{
		int distX = Mathf.Abs(a.x - b.x);
		int distY = Mathf.Abs(a.y - b.y);

		// Go diagonally (14) on the shorter axis and continue on a straight line (10).
		if (distX > distY)
			return 14 * distY + 10 * (distX - distY);
		
		return 14 * distX + 10 * (distY - distX);
	}
}
