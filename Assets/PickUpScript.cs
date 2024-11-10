using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour
{

    public GameObject player;
    public Transform holdPos;
    // Start is called before the first frame update
    //if you copy from below this point, you are legally required to like the video
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index
    private GameObject highlightedObject;

    private PlayerInput playerInput;

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        Debug.Log("PickUpScript is working!");
        playerInput = player.GetComponent<PlayerInput>();
        LayerNumber = LayerMask.NameToLayer("HoldLayer"); //if your holdLayer is named differently make sure to change this ""
        
        //mouseLookScript = player.GetComponent<MouseLookScript>();
    }
    void Update(){
        HighlightObject();
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            RotateObject();
        }
    }

    public void ThrowHolding(InputAction.CallbackContext callbackContext){
        Debug.Log("Throwing!");
        HighlightObject();
        if (callbackContext.started && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
        {
            StopClipping();
            ThrowObject();
        }
    }

    private void HighlightObject()
    {
        if (highlightedObject != null)
        {
            Debug.Log("No longer looking at object");
            highlightedObject.GetComponent<Outline>().enabled = false;
            highlightedObject = null;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
        {
            Debug.Log("Looking at object");
            //make sure pickup tag is attached
            if (hit.transform.gameObject.tag == "canPickUp")
            {
                highlightedObject = hit.transform.gameObject;
                Debug.Log("Found object to pick up");
                //pass in object hit into the PickUpObject function
                if (highlightedObject.GetComponent<Outline>() == null)
                {
                    highlightedObject.AddComponent<Outline>();
                    highlightedObject.GetComponent<Outline>().OutlineMode = Outline.Mode.OutlineAll;
                    highlightedObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlightedObject.GetComponent<Outline>().OutlineWidth = 7f;
                }
                highlightedObject.GetComponent<Outline>().enabled = true;
            }
        }
    }

    public void Interact(InputAction.CallbackContext callbackContext) {
        Debug.Log("Interacting!");
        Debug.Log(callbackContext.phase);
        if (callbackContext.started) //change E to whichever key you want to press to pick up
        {
            Debug.Log("Step1");
            if (heldObj == null) //if currently not holding anything
            {
                Debug.Log("Step2");
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    Debug.Log("Step3");
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        Debug.Log("Step4");
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                Debug.Log("Step2.1");
                if(canDrop == true)
                {
                    Debug.Log("Step3.1");
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        Debug.LogWarning("Step5");
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            Debug.LogWarning("Step6");
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    void RotateObject()
    {
        if (playerInput == null)
        {
            Debug.LogError("playerInput is null");
            return;
        }

        var rotateAction = playerInput.actions["Rotate"];
        if (rotateAction == null)
        {
            Debug.LogError("Rotate action is null");
            return;
        }

        bool isRotating = playerInput.actions["Rotate"].IsPressed();
        if (isRotating)//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating
            player.GetComponent<UserMovement>().CanMove = false;

            Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>(); 
            float XaxisRotation = input.x * rotationSensitivity;
            float YaxisRotation = input.y * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis from the camera perspective
            heldObj.transform.Rotate(Camera.main.transform.up, -XaxisRotation, Space.World);
            heldObj.transform.Rotate(Camera.main.transform.right, YaxisRotation, Space.World);
        }
        else
        {
            //re-enable player being able to look around
            //mouseLookScript.verticalSensitivity = originalvalue;
            //mouseLookScript.lateralSensitivity = originalvalue;
            canDrop = true;
            player.GetComponent<UserMovement>().CanMove = true;
        }
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
