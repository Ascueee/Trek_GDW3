using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI NPCName;
    public Image dialogueTextBox;

    public string[] textLines;
    [SerializeField] float textSpeed;
    [SerializeField] float lerpTimes;
    int index;

    private void Start()
    {
        dialogueText.text = string.Empty;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(dialogueText.text == textLines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = textLines[index];
            }
        }
    }
    
    public void StartDialogue(string[] NPCLines, string name)
    {
        LeanTween.init();
        dialogueText.text = string.Empty;
        LeanTween.value(dialogueTextBox.gameObject, dialogueTextBox.color.a, 1f, lerpTimes).setOnUpdate(ChangeBackGroundImageAlpha);
        LeanTween.value(NPCName.gameObject, NPCName.color.a, 1f, lerpTimes).setOnUpdate(NPCNameAlphaChange);
        LeanTween.value(dialogueText.gameObject, dialogueText.color.a, 1f, lerpTimes).setOnUpdate(DialogueTextAlphaChange);
        NPCName.text = name;
        textLines = NPCLines;
        
        index = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //type the dialogue onebyone
        foreach(char c in textLines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);

        }
    }
    public void ClearText()
    {
        dialogueText.text = string.Empty;
        NPCName.text = string.Empty;

        LeanTween.value(dialogueTextBox.gameObject, dialogueTextBox.color.a, 0f, lerpTimes).setOnUpdate(ChangeBackGroundImageAlpha);
        LeanTween.value(NPCName.gameObject, NPCName.color.a, 0f, lerpTimes).setOnUpdate(NPCNameAlphaChange);
        LeanTween.value(dialogueText.gameObject, dialogueText.color.a, 0f, lerpTimes).setOnUpdate(DialogueTextAlphaChange);
    }

    void NextLine()
    {
        if(index < textLines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueText.text = string.Empty;
        }
    }

    //LEAN TWEEN METHODS

    void ChangeBackGroundImageAlpha(float a)
    {
        var alphaChange = new Vector4(dialogueTextBox.color.r, dialogueTextBox.color.g, dialogueTextBox.color.b, a);

        dialogueTextBox.color = alphaChange;

    }

    void NPCNameAlphaChange(float a)
    {
        var alphaChange = new Vector4(NPCName.color.r, NPCName.color.g, NPCName.color.b, a);

        NPCName.color = alphaChange;
    }
    void DialogueTextAlphaChange(float a)
    {
        var alphaChange = new Vector4(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, a);

        dialogueText.color = alphaChange;
    }

}
