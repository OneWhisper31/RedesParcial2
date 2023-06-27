using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVAgent : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public float viewRadius;
    public LayerMask wallMask;
    public float viewAngle;

    // Update is called once per frame
    void Update()
    {
        foreach (var item in enemies)
        {
            if (InFieldOfView(item.transform.position))
            {
                item.GetComponent<Renderer>().material.color = Color.red;
                Debug.DrawLine(transform.position, item.transform.position, Color.red);
            }
            else
                item.GetComponent<Renderer>().material.color = Color.white;
        }
    }


    public bool InFieldOfView(Vector3 targetPos)
    {
        Vector3 dir = targetPos - transform.position;
        if (dir.magnitude > viewRadius) return false;
        if (!InLineOfSight(transform.position, targetPos)) return false;
        return Vector3.Angle(transform.forward, dir) <= viewAngle / 2;
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        //origen, direccion, distancia maxima, layer mask
        Vector3 dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, wallMask);
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
