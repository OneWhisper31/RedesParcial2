using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGrid : MonoBehaviour
{

    [SerializeField] Node[] _nodes;//en orden horizontal

    Node[,] _grid;

    public int width;
    public int height;

    public GameObject nodePrefab;
    public float nodeOffset;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        _grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(_nodes[x * width + y] != null)
                {
                    _grid[x,y] = _nodes[x*width + y];
                    _grid[x, y].Initialize(this, new Vector2Int(x, y));
                }
            }
        }
    }

    public List<Node> GetNeighborsBasedOnPosition(int x, int y)
    {

        List<Node> neighbors = new List<Node>();

        Vector3 mainPos = Vector3.zero;

        if (x < width || y < height)
        {
            mainPos = _grid[x, y].transform.position;
        }


        LayerMask wallMask = GameManager.instance.wallLayer;

        if(x + 1 < width)
        {
            if (_grid[x + 1, y] != null)
            {
                Vector3 endPos1 = _grid[x + 1, y].transform.position;

                if (!Physics.Raycast(mainPos, endPos1 - mainPos, (endPos1 - mainPos).magnitude, wallMask))
                    neighbors.Add(_grid[x + 1, y]);
            }
        }
        
        if (x - 1 >= 0)
        {
            if (_grid[x - 1, y] != null)
            {
                Vector3 endPos2 = _grid[x - 1, y].transform.position;

                if (!Physics.Raycast(mainPos, endPos2 - mainPos, (endPos2 - mainPos).magnitude, wallMask))
                    neighbors.Add(_grid[x - 1, y]);
            }
        }
            
        if (y + 1 < height)
        {
            if (_grid[x, y + 1] != null)
            {
                Vector3 endPos3 = _grid[x, y + 1].transform.position;

                if (!Physics.Raycast(mainPos, endPos3 - mainPos, (endPos3 - mainPos).magnitude, wallMask))
                    neighbors.Add(_grid[x, y + 1]);
            }
        }
            
        if (y - 1 >= 0)
        {
            if (_grid[x, y - 1] != null)
            {
                Vector3 endPos4 = _grid[x, y - 1].transform.position;
                Debug.Log("ARRIBA " + _grid[x, y - 1]);

                if (!Physics.Raycast(mainPos, endPos4 - mainPos, (endPos4 - mainPos).magnitude, wallMask))
                    neighbors.Add(_grid[x, y - 1]);
            }
        }

        return neighbors;
    }
    public Node GetStartingNode(Vector3 pos)//devuelve el nodo mas cercano segun la pos
    {
        float closestMagnitude=Mathf.Infinity;
        Node closestNode=null;

        foreach (Node node in _nodes)
        {
            if (node == null) continue;

            float magnitude=Vector3.Distance(pos, node.transform.position);
            if (magnitude < closestMagnitude)
            {
                closestMagnitude = magnitude;
                closestNode = node;

            }
        }

        return closestNode;
    }
} 