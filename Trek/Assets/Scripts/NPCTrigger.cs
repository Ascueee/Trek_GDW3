using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{

    public Dialogue dialogue;
    public bool canTalk;

    private void Update()
    {
        if(canTalk)
        {
            TriggerDialogue();
        }
    }
    public void TriggerDialogue()
    {
        print("this is running");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue.sentences, dialogue.name);
        canTalk = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        canTalk = true;
    }

    private void OnTriggerExit(Collider other)
    {
        FindObjectOfType<DialogueManager>().ClearText();
        canTalk = false;
    }

}
