using UnityEngine;
using UnityEditor;




[CustomEditor(typeof(FieldOfView))]

public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360f, fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + fow.AngleToVector(-fow.viewAngle/2) * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + fow.AngleToVector(fow.viewAngle/2) * fow.viewRadius);

        Handles.color = Color.red;
        foreach(Transform target in fow.targetsInView)
        {
            Handles.DrawLine(fow.transform.position, target.position);
        }
    }
}
