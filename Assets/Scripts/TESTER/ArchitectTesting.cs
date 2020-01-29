using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArchitectTesting : MonoBehaviour
{
    public Text text;
    public TextMeshProUGUI tmproText;
    TextArchitect architect;

    [TextArea(5,10)]
    public string say;
    public int characterPerFrame = 1;
    public float speed = 1f;
    public bool useEncap = true;
    public bool useTmpro = true;

    // Start is called before the first frame update
    void Start()
    {
        architect = new TextArchitect(say, "", characterPerFrame, speed, useEncap, useTmpro);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            architect = new TextArchitect(say, "", characterPerFrame, speed, useEncap, useTmpro);
        }

        if (useTmpro)
            tmproText.text = architect.currentText;
        else 
            text.text = architect.currentText;
    }
}
