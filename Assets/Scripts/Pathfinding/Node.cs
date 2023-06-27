using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Node : MonoBehaviour
{
	public int cost = 1;

    List<Node> _neighbors = new List<Node>();

    private NodeGrid _grid;
    private Vector2Int _posInGrid;	

    public void Initialize(NodeGrid grid, Vector2Int pos)
    {
        _grid = grid;
        _posInGrid = pos;
		SetCost(1);
    }

    public List<Node> GetNeighbors()
    {
        if (_neighbors.Count == 0)
            _neighbors = _grid.GetNeighborsBasedOnPosition(_posInGrid.x, _posInGrid.y);

        return _neighbors;
    }
    public Node GetStartingNode(Vector3 pos)
    {//devuelve el nodo mas cercano segun la pos
        return _grid.GetStartingNode(pos);
    }
    void SetCost(int c)
	{
		cost = Mathf.Clamp(c, 1, 99);
	}


}
