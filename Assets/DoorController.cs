using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private bool openTrigger = false;
    [SerializeField] private GameObject firstButton;
    [SerializeField] private GameObject firstButtonLight;
    [SerializeField] private GameObject secondButton;
    [SerializeField] private GameObject secondButtonLight;
    [SerializeField] private GameObject thirdButton;
    [SerializeField] private GameObject thirdButtonLight;
    [SerializeField] private GameObject fourthButton;
    [SerializeField] private GameObject fourthButtonLight;
    private bool isFirstButtonPressed = false;
    private bool isSecondButtonPressed = false;
    private bool isThirdButtonPressed = false;
    private bool isFourthButtonPressed = false;
    [SerializeField] private float waitTimer = 1.0f;
    [SerializeField] private bool pauseInteraction = false;
    [SerializeField ]private bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isFirstButtonPressed = firstButton.GetComponent<ButtonController>().isPressed;
        isSecondButtonPressed = secondButton.GetComponent<ButtonController>().isPressed;
        isThirdButtonPressed = thirdButton.GetComponent<ButtonController>().isPressed;
        isFourthButtonPressed = fourthButton.GetComponent<ButtonController>().isPressed;
        // Debug.Log("First: " + isFirstButtonPressed + " Second: " + isSecondButtonPressed + " Third: " + isThirdButtonPressed + " Fourth: " + isFourthButtonPressed);
        if (isFourthButtonPressed)
        {
            FourthButtonPressed();
        }
        else if (isThirdButtonPressed)
        {
            ThirdButtonPressed();
        }
        else if (isSecondButtonPressed)
        {
            SecondButtonPressed();
        }
        firstButton.GetComponent<ButtonController>().isPressed = isFirstButtonPressed;
        firstButtonLight.GetComponent<Light>().intensity = isFirstButtonPressed ? 0.6f : 0;
        secondButton.GetComponent<ButtonController>().isPressed = isSecondButtonPressed;
        secondButtonLight.GetComponent<Light>().intensity = isSecondButtonPressed ? 0.6f : 0;
        thirdButton.GetComponent<ButtonController>().isPressed = isThirdButtonPressed;
        thirdButtonLight.GetComponent<Light>().intensity = isThirdButtonPressed ? 0.6f : 0;
        fourthButton.GetComponent<ButtonController>().isPressed = isFourthButtonPressed;
        fourthButtonLight.GetComponent<Light>().intensity = isFourthButtonPressed ? 0.6f : 0;

    }

    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
        // door.GetComponent<MeshCollider>().enabled = true;
    }

    void TriggerDoor()
    {
        if (pauseInteraction) return;
        if (doorOpen) return;
        doorOpen = true;
        door.GetComponent<MeshCollider>().enabled = false;
        doorAnimator.Play("DoorOpen", 0, 0.0f);
        StartCoroutine(PauseInteraction());
    }

    public void SecondButtonPressed()
    {
        if (!isFirstButtonPressed)
        {
            isFirstButtonPressed = false;
            isSecondButtonPressed = false;
        }
    }

    public void ThirdButtonPressed()
    {
        if (!(isFirstButtonPressed && isSecondButtonPressed))
        {
            isFirstButtonPressed = false;
            isSecondButtonPressed = false;
            isThirdButtonPressed = false;
        }
    }


    public void FourthButtonPressed()
    {
        if (!(isFirstButtonPressed && isSecondButtonPressed && isThirdButtonPressed))
        {
            isFirstButtonPressed = false;
            isSecondButtonPressed = false;
            isThirdButtonPressed = false;
            isFourthButtonPressed = false;
        }
        else TriggerDoor();
    }
}
