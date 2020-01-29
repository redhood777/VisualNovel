using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{

    //line of data loaded from chapter file
    List<string> data = new List<string>();
    //progress in current data lists
    int progress = 0;

    void Start()
    {
        LoadChapterFile("Chapter0_start");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleLine(data[progress]);
            progress++;
        }
        //Debug.Log(HandleEventsFromLine.events);
    }

    public void LoadChapterFile(string filename)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + filename);
        progress = 0;
        cachedLastSpeaker = "";
    }

    void HandleLine(string line)
    {
        string[] dialogueAndActions = line.Split('"');

        if (dialogueAndActions.Length == 3)
        {
            HandleObject(dialogueAndActions[0], dialogueAndActions[1]);
            HandleEventsFromLine(dialogueAndActions[2]);

        }
        else
        {
            HandleEventsFromLine(dialogueAndActions[0]);
        }
    }

    string cachedLastSpeaker = "";
    void HandleObject(string dialogueDetails, string dialogue)
    {
        string speaker = cachedLastSpeaker;
        bool additive = dialogueDetails.Contains("+");

        if (additive)
            dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);

        if (dialogueDetails.Length > 0)
        {
            if (dialogueDetails[dialogueDetails.Length - 1] == ' ')
                dialogueDetails = dialogueDetails.Remove(dialogueDetails.Length - 1);

            speaker = dialogueDetails;
            cachedLastSpeaker = speaker;

        }

        if (speaker != "narrator")
        {
            Character character = CharacterManager.instance.GetCharacter(speaker);
            character.Say(dialogue, additive);
        }
        else
        {
            DialogueSystem.instance.Say(dialogue, speaker, additive);
        }
    }

    void HandleEventsFromLine(string events)
    {
        string[] actions = events.Split(' ');

        foreach (string action in actions)
        {
            HandleActions(action);
        }

    }

    void HandleActions(string action)
    {
        print("Handle actions [" + action + "]");
        string[] data = action.Split('(', ')');

        if (data[0] == "setBackground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.background);
            return;
        }
        if (data[0] == "setCinematic")
        {
            Command_SetLayerImage(data[1], BCFC.instance.cinematic);
            return;
        }
        if (data[0] == "setForeground")
        {
            Command_SetLayerImage(data[1], BCFC.instance.foreground);
            return;
        }
    }

    void Command_SetLayerImage(string data, BCFC.LAYER layer)
    {
        string texName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D tex = Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
        float spd = 2f;
        bool smooth = false;

        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            foreach (string p in parameters)
            {
                float fVal = 0;
                bool bVal = false;
                if (float.TryParse(p, out fVal))
                    spd = fVal; continue;
                if (bool.TryParse(p, out bVal))
                    smooth = bVal; continue;
            }
        }

        layer.TransitionToTexture(tex, spd, smooth);
    }
}
