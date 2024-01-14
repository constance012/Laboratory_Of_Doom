using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathRequester : Singleton<PathRequester>
{
	[Header("References"), Space]
	[SerializeField] private AStarBrain brain;
	[SerializeField] private NodeGrid grid;

	// Private fields.
	private Queue<PathRequestData> _queue = new Queue<PathRequestData>();
	private PathRequestData _currentRequest;

	private bool _isProcessingPath;

	public static void Request(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
	{
		PathRequestData newRequest = new PathRequestData(start, end, callback);
		Instance._queue.Enqueue(newRequest);
		Instance.TryProcessNext();
	}
	
	public void InvokeCallback(Vector3[] path, bool success)
	{
		_currentRequest.callback(path, success);
		_isProcessingPath = false;

		TryProcessNext();
	}

	public void ChangeGridCellState(Vector3 worldPos, bool walkable)
	{
		grid.SetCellWalkableState(worldPos, walkable);
	}

	private bool TryProcessNext()
	{
		if (!_isProcessingPath && _queue.Count > 0)
		{
			_currentRequest = _queue.Dequeue();
			_isProcessingPath = true;

			brain.StartFindingPath(_currentRequest.pathStart, _currentRequest.pathEnd);

			return true;
		}

		return false;
	}

	struct PathRequestData
	{
		public Vector3 pathStart;
		public Vector3 pathEnd;
		public Action<Vector3[], bool> callback;

		public PathRequestData(Vector3 start, Vector3 end, Action<Vector3[], bool> callback)
		{
			this.pathStart = start;
			this.pathEnd = end;
			this.callback = callback;
		}
	}
}
