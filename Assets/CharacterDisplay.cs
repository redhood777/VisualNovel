//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class CharacterDisplay : MonoBehaviour
//{
//    DialogueSystem dialogue;

//    public Character character;
//    public Text nameText;
//    public Text speechText;
//    public GameObject speechPanel;

//    public Image artworkImage;

//    int index = 0;
//    int speechCount = 0;

//    private void Start()
//    {
//        dialogue = DialogueSystem.instance;
//        nameText.text = character.name;
//        speechText.text = "";
//        artworkImage.sprite = character.artwork;
//    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
            
//            if (!dialogue.isSpeaking|| dialogue.isWaiting)
//            {
//                if (index >= character.speech.Length)
//                {
//                    return;
//                }
//                Say(character.speech[index]);
//                index++;
//                speechCount++;
//            }
//        }
//    }

//    void Say(string s)
//    {
//        string speech = character.speech[index];

//        dialogue.Say(speech);
//    }

   
//}
