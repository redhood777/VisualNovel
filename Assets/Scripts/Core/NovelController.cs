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

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    HandleLine(data[progress]);
        //    progress++;
        //}
        //Debug.Log(HandleEventsFromLine.events);
    }

    public void OnButtonDown()
    {
        HandleLine(data[progress]);
        progress++;
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

        switch (data[0])
        {
            case ("setBackground"):
                Command_SetLayerImage(data[1], BCFC.instance.background);
                break;

            case ("setCinematic"):
                Command_SetLayerImage(data[1], BCFC.instance.cinematic);
                break;

            case ("setForeground"):
                Command_SetLayerImage(data[1], BCFC.instance.foreground);
                break;
            case ("playMusic"):
                Command_PlayMusic(data[1]);
                break;
            case ("playSound"):
                Command_PlaySound(data[1]);
                break;
            case ("move"):
                Command_Move(data[1]);
                break;
            case ("setPosition"):
                Command_SetPosition(data[1]);
                break;
            case ("setFace"):
                Command_SetFace(data[1]);
                break;
            case ("setBody"):
                Command_SetBody(data[1]);
                break;
            case ("flip"):
                Command_Flip(data[1]);
                break;
            case ("faceLeft"):
                Command_FaceLeft(data[1]);
                break;
            case ("faceRight"):
                Command_FaceRight(data[1]);
                break;
            case ("showImage"):
                Command_ShowImage();
                break;
            case ("enter"):
                Command_Enter(data[1]);
                break;
            case ("exit"):
                Command_Exit(data[1]);
                break;

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
        float locationY = parameters.Length == 3 ? float.Parse(parameters[2]):0;
        float speed = parameters.Length == 4 ? float.Parse(parameters[3]) : 10f;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : true;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed,smooth);
       

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

    void Command_SetFace(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string expression = (parameters[1]);
        float speed = parameters.Length == 3 ? float.Parse(parameters[2]) : 15f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        c.TransitionExpression(sprite, speed, false);
    }

    void Command_SetBody(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string expression = (parameters[1]);
        float speed = parameters.Length == 3 ? float.Parse(parameters[2]) : 15f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        c.TransitionBody(sprite, speed, false);
    }

    void Command_Flip(string data)
    {
        string[] characters = data.Split(',');

        foreach(string s in characters)
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

    void Command_ShowImage()
    {
        TutorialEvents.instance.ShowImage();
    }

    void Command_Exit(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;

        for(int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0;
            bool bVal = false;
            if (float.TryParse(parameters[i], out fVal)) 
            {
                speed = fVal;
                continue;
            }
            if(bool.TryParse(parameters[i], out bVal))
            {
                smooth = bVal;
                continue;
            }
        }
        foreach(string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FadeOut(speed, smooth);
        }
    }

    void Command_Enter(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;

        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0;
            bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            {
                speed = fVal;
                continue;
            }
            if (bool.TryParse(parameters[i], out bVal))
            {
                smooth = bVal;
                continue;
            }
        }
        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s,true,false);
            if (!c.enabled)
            {
                c.renderers.bodyRenderer.color = new Color(1, 1, 1, 0);
                c.renderers.expressionRenderer.color = new Color(1, 1, 1, 0);
                c.enabled = true;

                c.TransitionBody(c.renderers.bodyRenderer.sprite, speed, smooth);
                c.TransitionExpression(c.renderers.expressionRenderer.sprite, speed, smooth);
            }
            else
                c.FadeIn(speed, smooth);
        }
    }
    
}
