using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    DialogueSystem dialogue;

    private void Start()
    {
        dialogue = DialogueSystem.instance;
    }

    public string[] s = new string[]
    {
        "I.......:Avira",
        "I have to tell u smthing..."
        
    };

    int index = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (!dialogue.isSpeaking || dialogue.isWaitingForUserInput)
            {
                if (index >= s.Length)
                {
                    return;
                }
                Say(s[index]);
                index++;
            }
        }
    }


    void Say(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        dialogue.Say(speech,speaker);
    }
}
