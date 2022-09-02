
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    Camera cam;
    public Transform plane;
    public Transform player;

    public float rotationSpeed;

    private Vector3 frameRotation;
    private Vector3 rotation;
    private Vector3 offset;


    void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = plane.localScale.x * 4;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            frameRotation.x = Input.GetAxis("Mouse Y");
            frameRotation.y = Input.GetAxis("Mouse X");
            frameRotation *= Time.deltaTime * rotationSpeed;
            rotation += frameRotation;
            rotation.x = Mathf.Clamp(rotation.x, -90f, 0f);
            player.localRotation = Quaternion.Euler(rotation);
        }
    }
}
