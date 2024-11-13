using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUsableElement  
{
    void Use(GameObject triggerObject);
}

public class ButtonController : MonoBehaviour, IUsableElement
{
    public bool isPressed = false;
    [SerializeField] private Animator buttonAnimator;
    [SerializeField] private bool useTrigger = false;
    [SerializeField] private float waitTimer = 1.3f;
    [SerializeField] private bool pauseInteraction = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PauseInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void Use(GameObject triggerObject)
    {
        if (pauseInteraction) return;

        isPressed = true;
        buttonAnimator.Play("LeverUp", 0, 0.0f);
        StartCoroutine(PauseInteraction());

    }

}
