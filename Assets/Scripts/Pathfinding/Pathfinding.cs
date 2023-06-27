using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public List<Node> AStar(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return new List<Node>();

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();
                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Add(startingNode);
                path.Reverse();

                return path;
            }

            foreach (Node next in current.GetNeighbors())
            {
                int newCost = costSoFar[current] + next.cost;
                float priority = newCost + Vector3.Distance(next.transform.position, goalNode.transform.position);
                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }
        return new List<Node>();
    }

    public bool InFieldOfView(Vector3 _targetPos,Transform _transform, float viewRadius, float viewAngle)
    {
        Vector3 dir = _targetPos - _transform.position;
        if (dir.magnitude > viewRadius) return false;
        if (!InLineOfSight(_transform.position, _targetPos)) return false;
        return Vector3.Angle(_transform.forward, dir) <= viewAngle / 2;
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        //origen, direccion, distancia maxima, layer mask
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, GameManager.instance.wallLayer);
    }
}
