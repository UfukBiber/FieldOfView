using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewAngle;
    public float viewRadius;
    public float meshResolution;

    public LayerMask targetMask;
    public LayerMask objectMask;

    public MeshFilter meshFilter;
    Mesh viewMesh;
    //[HideInInspector]
    public List<Transform> targetsInView = new List<Transform>();

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 hitPoint;
        public float distance;
        public float angle;
        public ViewCastInfo(bool _hit, Vector3 _hitPoint, float _distance, float _angle)
        {
            hit = _hit;
            hitPoint = _hitPoint;
            distance = _distance;
            angle = _angle;
        }
    }

    private void Start()
    {
        viewMesh = new Mesh();
        meshFilter.mesh = viewMesh;
    }


    private void LateUpdate()
    {
        FindVisibleTargets();
        DrawFieldOfView();
    }

    void FindVisibleTargets()
    {
        targetsInView.Clear();
        
        Collider[] visibleTargets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < visibleTargets.Length; i++)
        {
            Transform target = visibleTargets[i].transform;
            Vector3 targetRelativePos = target.position - transform.position;
            if (Vector3.Angle(transform.forward, targetRelativePos) < viewAngle/2)
            {
                float distanceOfTarget = Vector3.Distance(target.position, transform.position);
                if (!Physics.Raycast(transform.position, targetRelativePos, distanceOfTarget, objectMask))
                {
                    targetsInView.Add(target);
                }
            }
        }
    }

    public ViewCastInfo GetViewCastInfo(float globalAngle)
    {
        Vector3 dir = AngleToVector(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, objectMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else if(Physics.Raycast(transform.position, dir, out hit, viewRadius, targetMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, hit.distance, globalAngle);
        }
    }
    void DrawFieldOfView()
    {
        int steps = Mathf.FloorToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / steps;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i < steps; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + AngleToVector(angle, true)*viewRadius);
            ViewCastInfo viewCastInfo = GetViewCastInfo(angle);
            viewPoints.Add(viewCastInfo.hitPoint);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }
    }
    public Vector3 AngleToVector(float angle, bool isGlobal = false)
    {
        if (!isGlobal)
        {
            angle += transform.localEulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
