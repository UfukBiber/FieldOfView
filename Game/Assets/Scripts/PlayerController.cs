using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector2 inputVector;
    private Vector3 frameMovementVector;

    public float speed;
    public float runningSpeed;
    public float rotationSpeed;


    void Start()
    {
        _characterController = GetComponent<CharacterController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Rotate();
    }


    void Move()
    {
        frameMovementVector = Vector3.zero;
        frameMovementVector = transform.forward * Time.deltaTime * inputVector.y;
        if (Input.GetKey(KeyCode.LeftShift)) { frameMovementVector *= runningSpeed; }
        else { frameMovementVector *= speed; }
        _characterController.Move(frameMovementVector);
    }


    void Rotate()
    {
        frameMovementVector = Vector3.zero;
        frameMovementVector.y = inputVector.x;
        frameMovementVector *= rotationSpeed * Time.deltaTime;
        transform.Rotate(frameMovementVector);
    }

    void GetInput()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
    }
}
