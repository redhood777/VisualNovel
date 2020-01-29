using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterTesting : MonoBehaviour
{
    public Character Raelin;

    // Start is called before the first frame update
    void Start()
    {
        Raelin = CharacterManager.instance.GetCharacter("Raelin",enableCreatedCharacterOnStart:true);
        Raelin.GetSprite(2);
    }

    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;
    public string[] speech;
    int i = 0;
    
    public int bodyIndex,expressionIndex = 0;
    public float speed = 5f;
    public bool smoothTransitions = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (i < speech.Length)
            {
                Raelin.Say(speech[i]);
            }
            else
            {
                DialogueSystem.instance.Close();
            }
            i++; 
        }
        if (Input.GetKey(KeyCode.M))
        {
            Raelin.MoveTo(moveTarget,moveSpeed,smooth);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Raelin.StopMoving(true);
        }
        if (Input.GetKey(KeyCode.B))
        {
            if (Input.GetKey(KeyCode.T))
                Raelin.TransitionBody(Raelin.GetSprite(bodyIndex), speed, smoothTransitions);
            else
                Raelin.SetBody(bodyIndex);
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.T))
                Raelin.TransitionExpression(Raelin.GetSprite(expressionIndex), speed, smoothTransitions);
            else
                Raelin.SetExpression(expressionIndex);
        }
    }
}
