using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewAngle;
    public float viewRadius;

    public LayerMask targetMask;
    public LayerMask objectMask;
    //[HideInInspector]
    public List<Transform> targetsInView = new List<Transform>();


    private void Update()
    {
        FindVisibleTargets();
    }

    void FindVisibleTargets()
    {
        targetsInView.Clear();
        Collider[] visibleTargets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        for (int i = 0; i < visibleTargets.Length; i++)
        {
            Transform target = visibleTargets[i].transform;
            Vector3 targetRelativePos = target.position - transform.position;
            if (Vector3.Angle(transform.forward, targetRelativePos) < viewAngle)
            {
                float distanceOfTarget = Vector3.Distance(target.position, transform.position);
                if (!Physics.Raycast(transform.position, targetRelativePos, distanceOfTarget, objectMask))
                {
                    targetsInView.Add(target);
                }
            }
        }
    }
    public Vector3 AngleToVector(float angle)
    {
        angle += transform.localEulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
