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
        if(data[0] == "playMusic")
        {
            Command_PlayMusic(data[1]);
            return;
        }
        if (data[0] == "playSound")
        {
            Command_PlaySound(data[1]);
            return;
        }
        if (data[0] == "move")
        {
            Command_Move(data[1]);
            return;
        }
        if(data[0] == "setPosition")
        {
            Command_SetPosition(data[1]);
            return;
        }
        if(data[0] == "setExpression")
        {
            Command_SetExpression(data[1]);
            return;
        }
        if (data[0] == "stopDialogue")
        {
            Command_StopDialogue();
            return;
        }

        if (data[0] == "startDialogue")
        {
            Command_StartDialogue();
            return;
        }
    }

    void Command_SetLayerImage(string data, BCFC.LAYER layer)
    {
        string texName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D tex = texName == null ?null : Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
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

    void Command_PlaySound(string data)
    {
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;

        if (clip != null)
            AudioManager.instance.PLaySFX(clip);
        else
            Debug.LogError("Clip does not exist - " + data);

    }

    void Command_PlayMusic(string data)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;

        if (clip != null)
            AudioManager.instance.PlaySong(clip);
        else
            Debug.LogError("Clip does not exist - " + data);
    }

    void Command_Move(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);
        float speed = parameters.Length == 4 ? float.Parse(parameters[3]) : 1f;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed);
       

    }

    void Command_SetPosition(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = float.Parse(parameters[2]);

        Character c = CharacterManager.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));
    }

    void Command_SetExpression(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string region = parameters[1];
        string expression = parameters[2];
        float speed = parameters.Length == 4 ? float.Parse(parameters[3]) : 1f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        if (region.ToLower() == "body")
            c.TransitionBody(sprite, speed, false);
        if (region.ToLower() == "face")
            c.TransitionExpression(sprite, speed, false);
    }

    void Command_StopDialogue()
    {
        TutorialEvents.instance.DisableDialogue();
    }

    void Command_StartDialogue()
    {
        TutorialEvents.instance.EnableDialogue();
    }
    void Command_Flip(string data)
    {
        string[] characters = data.Split(',');

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.Flip();
        }  
    }

    void Command_FaceLeft(string data)
    {
        string[] characters = data.Split(',');

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FaceLeft();
        }
    }

    void Command_FaceRight(string data)
    {
        string[] characters = data.Split(',');

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FaceRight();
        }
    }
}
