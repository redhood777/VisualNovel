using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEvents : MonoBehaviour
{

    bool pressed = false;
    public GameObject photo;
    
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

    public void ShowImage()
    {
        photo.SetActive(true);
    }

}
