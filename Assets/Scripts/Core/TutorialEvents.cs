using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{

    bool pressed = false;
    public GameObject speechSystem;
    // Start is called before the first frame update

    public static TutorialEvents instance;
    private void Awake()
    {
        instance = this;  
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ButtonOn()
    {
        pressed = true;

    }

    public void DisableDialogue()
    {
        StopAllCoroutines();
        //DialogueSystem.instance.StopSpeaking();
        speechSystem.SetActive(false);
        //StopCoroutine(DialogueSystem.instance.StopSpeaking());
       
    }

    public void EnableDialogue()
    {
        
        if (pressed == true)
            speechSystem.SetActive(true);
        else
            speechSystem.SetActive(false);
        //if (pressed = true)
           // DialogueSystem.instance.Say();

        //pressed = false;
    }
}
