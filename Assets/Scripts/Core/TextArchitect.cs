using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextArchitect
{
    public string currentText { get { return _currentText; } }
    private string _currentText = "";

    private string preText;
    private string targetText;

    private int charactersPerFrame = 1;

    [Range(1f,60f)]
    private float speed = 1f;
    private bool useEncapsulation = true;

    public bool skip = false;

    public bool isConstructing { get { return buildProcess != null; } } 
    Coroutine buildProcess = null;

    private bool isTMPro = false;

    public TextArchitect(string targetText,string preText = "",int charactersPerFrame = 1,float speed = 1f,bool useEncapsulation = true,bool isTMPro = true)
    {
        this.targetText = targetText;
        this.preText = preText;
        this.charactersPerFrame = charactersPerFrame;
        this.speed = speed;
        this.useEncapsulation = useEncapsulation;
        this.isTMPro = isTMPro;

        buildProcess = DialogueSystem.instance.StartCoroutine(Construction());
    }

    public void Stop()
    {
        if (isConstructing)
        {
            DialogueSystem.instance.StopCoroutine(buildProcess);
        }
        buildProcess = null;
    }

    IEnumerator Construction()
    {
        int runsThisFrame = 0;
        string[] speechAndTags = useEncapsulation ? TagManager.SplitByTags(targetText) : new string[1] { targetText };

        //if this is aditive,make sure to include aditive text
        _currentText = preText;

        //make a storage variable so we dont chnage whats been made already
        string curTxt = "";

        //build text by moving through each part
        for(int a = 0; a < speechAndTags.Length; a++)
        {
            string section = speechAndTags[a];
            //tags will always be odd indexed
            bool isATag = (a & 1) != 0;

            if (isATag && useEncapsulation)
            {
                if (!isTMPro)
                {
                    curTxt = _currentText;
                    ENCAPSULATED_TEXT encapsulation = new ENCAPSULATED_TEXT(string.Format("<{0}>", section), speechAndTags, a);
                    while (!encapsulation.isDone)
                    {
                        bool stepped = encapsulation.Step();
                        _currentText = curTxt + encapsulation.displayText;
                        //only yield if a step was taken in building the string
                        if (stepped)
                        {
                            runsThisFrame++;
                            int maxRunsPerFrame = skip ? 5 : charactersPerFrame;
                            if (runsThisFrame == maxRunsPerFrame)
                            {
                                runsThisFrame = 0;
                                yield return new WaitForSeconds(skip ? 0.01f : 0.01f * speed);
                            }
                        }
                    }
                    a = encapsulation.speechAndTagsArrayProgress + 1;
                }
                else
                {
                    string tag = string.Format("<{0}>", section);
                    _currentText += tag;
                    yield return new WaitForEndOfFrame();
                }
               
            }
            else
            {
                for(int i = 0; i < section.Length; i++)
                {
                    _currentText += section[i];

                    runsThisFrame++;
                    int maxRunsPerFrame = skip ? 5 : charactersPerFrame;
                    if (runsThisFrame == maxRunsPerFrame)
                    {
                        runsThisFrame = 0;
                        yield return new WaitForSeconds(skip ? 0.01f : 0.01f * speed);
                    }
                }
            }
        }

        buildProcess = null;
    }

    private class ENCAPSULATED_TEXT
    {
        private string tag = "";
        private string endingTag="";

        private string currentText = ""; 
        private string targetText = "";

        public string displayText { get { return _displayText;}}
        private string _displayText = "";

        private string[] allSpeechAndTagArray;
        public int speechAndTagsArrayProgress { get { return arrayProgress; } }
        private int arrayProgress;

        public bool isDone { get { return _isDone; } }
        private bool _isDone = false;

        public ENCAPSULATED_TEXT encapsulator = null;
        public ENCAPSULATED_TEXT subEncapsulator = null;

        public ENCAPSULATED_TEXT(string tag, string[] allSpeechAndTagsArray, int arrayProgress)
        {
            this.tag = tag;
            //this.endingTag = GenerateEndingTag();
            GenerateEndingTag();

            this.allSpeechAndTagArray = allSpeechAndTagsArray;
            this.arrayProgress = arrayProgress;

            if (allSpeechAndTagsArray.Length - 1 > arrayProgress)
            {
                string nextPart = allSpeechAndTagsArray[arrayProgress + 1];
                bool isATag = ((arrayProgress + 1) & 1) != 0;

                if (!isATag)
                    targetText = nextPart;

                else
                    subEncapsulator = new ENCAPSULATED_TEXT(string.Format("<{0}>", nextPart), allSpeechAndTagsArray, arrayProgress + 1);
                //increment progress so the next attempted part is updated.
                this.arrayProgress++;
            }
        }

        void GenerateEndingTag()
        {
            endingTag = tag.Replace("<", "").Replace(">", "");
            if (endingTag.Contains("="))
            {
                endingTag = string.Format("</{0}>", endingTag.Split('=')[0]);            
            }
            else
            {
                endingTag = string.Format("</{0}>", endingTag);
            }
        }

        public bool Step()
        {
            if (isDone)
                return true;

            if (subEncapsulator != null && !subEncapsulator.isDone)
            {
                return subEncapsulator.Step();
            }

            else
            {
                if (currentText == targetText)
                {
                    if (allSpeechAndTagArray.Length > arrayProgress + 1)
                    {
                        string nextPart = allSpeechAndTagArray[arrayProgress + 1];
                        bool isATag = ((arrayProgress + 1) & 1) != 0;

                        if (isATag)
                        {
                            if (string.Format("<{0}>", nextPart) == endingTag)
                            {
                                _isDone = true;

                                if (encapsulator != null)
                                {
                                    string taggedText = (tag + currentText + endingTag);
                                    encapsulator.currentText += taggedText;
                                    encapsulator.targetText += taggedText;

                                    UpdateArrayProgress(2);
                                }
                            }
                            else
                            {
                                subEncapsulator = new ENCAPSULATED_TEXT(string.Format("<{0}>", nextPart), allSpeechAndTagArray, arrayProgress + 1);
                                subEncapsulator.encapsulator = this;

                                UpdateArrayProgress();
                            }
                        }
                        else
                        {
                            targetText += nextPart;
                            UpdateArrayProgress();
                        }
                    }
                    else
                    {
                        _isDone = true;
                    }
                }
                else
                {
                    currentText += targetText[currentText.Length];

                    UpdateDisplay("");
                    return true;
                }
            }
            
            return false;
        }

        void UpdateArrayProgress(int val = 1)
        {
            arrayProgress += val;

            if (encapsulator != null)
                encapsulator.UpdateArrayProgress(val);
        }

        void UpdateDisplay(string subValue)
        {
            _displayText = string.Format("{0}{1}{2}{3}", tag, currentText, subValue, endingTag);

            if (encapsulator != null)
            {
                encapsulator.UpdateDisplay(displayText);
            }
        }
    }
}
