using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float speed;

    [Header("FOV")]
    public float viewRadius;
    public float viewAngle;

    public List<Node> patrolWaypoints;

    Pathfinding _pathfinding = new Pathfinding();

    int _waypointsIndex;

    public bool onPlayerNotice;

    Node _target;

    Vector3 dir;

    List<Node> _path = new List<Node>();
    List<Vector3> _posPath = new List<Vector3>();

    private void Start()
    {
        _target = patrolWaypoints[_waypointsIndex];
        dir = _target.transform.position - transform.position;
    }

    private void Update()
    {//que cada vez que se acerque a su objetivo, que pregunte si tiene el siguiente si puede verlo

        dir = _target.transform.position - transform.position;


        if (_pathfinding.InFieldOfView(GameManager.instance.player.transform.position, transform, viewRadius, viewAngle))
        {
            dir = GameManager.instance.player.transform.position - transform.position;

            GameManager.instance.OnPlayerDetected(this);

            if (dir.magnitude < 0.3f)//si llego a su target que elija un nuevo objetivo, ya sea el a*, player o waypoints
            {
                Debug.Log("Llegue a Player");
                return;
            }
        }
        else if (!_pathfinding.InLineOfSight(transform.position, _target.transform.position))//si no ve su target que use a*
        {
            TrackNewPath();
            dir= TrackNewTarget();
        }
        else if (_posPath.Count>0)
            PathAStar();
        else
            Patrol();

        transform.position += dir.normalized * speed * Time.deltaTime;

        transform.forward = dir.normalized;
    }
    void Patrol()
    {
        if (dir.magnitude < 0.01f)//si llego a su target que elija un nuevo objetivo, ya sea el a*, player o waypoints
        {
            if (_waypointsIndex < patrolWaypoints.Count - 1)
                _waypointsIndex++;
            else
                _waypointsIndex = 0;

            onPlayerNotice = false;//significa que ya llego

            _target = patrolWaypoints[_waypointsIndex];
            dir = _target.transform.position - transform.position;

        }
        dir = _target.transform.position - transform.position;
    }
    void PathAStar()
    {
        if (dir.magnitude < 0.01f)//si llego a su target que elija un nuevo objetivo, ya sea el a*, player o waypoints
        {
            if (_posPath.Count > 0)
            {
                dir = TrackNewTarget();
            }
        }
        dir = _target.transform.position - transform.position;
    }

    public void TrackNewPathPlayer(Transform playerPos)
    {
        Node playerClostestNode = patrolWaypoints[_waypointsIndex].GetStartingNode(playerPos.position);


        if (onPlayerNotice)//if alrealdy set, return;
            return;

        onPlayerNotice = true;
        TrackNewPath(playerClostestNode);
        _posPath.Add(playerPos.position);
        _path.Add(playerClostestNode);//duplicate to avoid errors

        dir = TrackNewTarget();
    }
    public void TrackNewPath(Node _personalTarget= null)
    {
        _posPath = new List<Vector3>();

        _path = _pathfinding.AStar(patrolWaypoints[_waypointsIndex].GetStartingNode(transform.position),
            _personalTarget == null ? _target.GetComponent<Node>() : _personalTarget);
        //== null usa el nodo del waypoint, sino usa el player

        foreach (Node node in _path)
        {
            _posPath.Add(node.transform.position);
        }
    }
    public Vector3 TrackNewTarget()
    {
        _posPath.RemoveAt(0);

        _target = _path[0];
        _path.RemoveAt(0);

        return _target.transform.position - transform.position;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 lineA = GetDirFromAngle(-viewAngle / 2 + transform.eulerAngles.y);
        Vector3 lineB = GetDirFromAngle(viewAngle / 2 + transform.eulerAngles.y);

        Gizmos.DrawLine(transform.position, transform.position + lineA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + lineB * viewRadius);

    }

    Vector3 GetDirFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
