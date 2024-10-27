using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float force = 10f;
    private PlayerInput playerInput;
    private Vector2 input;

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
        rb.AddForce(new Vector3(input.x, 0f, input.y)*force);
    }

    public void Move(InputAction.CallbackContext callbackContext){
        if (callbackContext.performed) {
            
        }
    }
}
