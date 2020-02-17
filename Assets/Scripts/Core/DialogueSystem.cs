﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour 
{
	public static DialogueSystem instance;

	public ELEMENTS elements;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	/// <summary>
	/// Say something and show it on the speech box.
	/// </summary>
	public void Say(string speech, string speaker = "", bool additive = false)
	{
		StopSpeaking();

        if(additive)
            speechText.text = targetSpeech;

        speaking = StartCoroutine(Speaking(speech, additive, speaker));
	}

	public void StopSpeaking()
	{
		if (isSpeaking)
		{
			StopCoroutine(speaking);
		}
        if (textArchitect != null && textArchitect.isConstructing)
        {
            textArchitect.Stop();
        }
		speaking = null;
	}
		
	public bool isSpeaking {get{return speaking != null;}}
	[HideInInspector] public bool isWaitingForUserInput = false;

	public string targetSpeech = "";
	Coroutine speaking = null;
    TextArchitect textArchitect = null;
	IEnumerator Speaking(string speech, bool additive, string speaker = "")
	{
		speechPanel.SetActive(true);

        string additiveSpeech = additive ? speechText.text : "";
		targetSpeech = additiveSpeech + speech;

        textArchitect = new TextArchitect(speech, additiveSpeech);

		speakerNameText.text = DetermineSpeaker(speaker);//temporary
        speakerNamePanel.SetActive(speakerNameText.text != "");

		isWaitingForUserInput = false;

		while(textArchitect.isConstructing)
		{
            if (Input.GetKey(KeyCode.Space))
            {
                textArchitect.skip = true;
            }
            speechText.text = textArchitect.currentText;

			yield return new WaitForEndOfFrame();
		}
        speechText.text = textArchitect.currentText;

        //text finished
        isWaitingForUserInput = true;
		while(isWaitingForUserInput)
			yield return new WaitForEndOfFrame();

		StopSpeaking();
	}

	string DetermineSpeaker(string s)
	{
		string retVal = speakerNameText.text;//default return is the current name
		if (s != speakerNameText.text && s != "")
			retVal = (s.ToLower().Contains("narrator")) ? "" : s;

		return retVal;
	}

	/// <summary>
	/// Close the entire speech panel. Stop all dialogue.
	/// </summary>
	public void Close()
	{
		StopSpeaking ();
		speechPanel.SetActive (false);
	}

	[System.Serializable]
	public class ELEMENTS
	{
		/// <summary>
		/// The main panel containing all dialogue related elements on the UI
		/// </summary>
		public GameObject speechPanel;
        public GameObject speakerNamePanel;
		public TextMeshProUGUI speakerNameText;
		public TextMeshProUGUI speechText;
	}
	public GameObject speechPanel {get{return elements.speechPanel;}}
    public GameObject speakerNamePanel { get { return elements.speakerNamePanel; } }
	public TextMeshProUGUI speakerNameText {get{return elements.speakerNameText;}}
	public TextMeshProUGUI speechText {get{return elements.speechText;}}
}