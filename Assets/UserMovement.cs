using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float force = 100f;
    private PlayerInput playerInput;
    private Vector2 input;

    private bool canMove = true;

    [SerializeField] private float moveSpeed = 10f;

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>(); 
    }


    private void FixedUpdate() {
        if (canMove) {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
            // rb.AddForce(moveDirection * force);
            rb.MovePosition(transform.position + moveDirection * moveSpeed*Time.deltaTime);
        }
    }

    public void Move(InputAction.CallbackContext callbackContext){
        if (callbackContext.performed) {
            
        }
    }
}
