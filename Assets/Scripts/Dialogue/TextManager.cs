using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : StateHandler
{
    public TextMeshProUGUI nameText;
    
    public TextMeshProUGUI accentNameText;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI[] choicesTexts;

    public GameObject dialoguePanel;
    public GameObject namePanel;

    public string currentSpeaker;

    private void Start()
    {
        /*
        dialogueText.text = "";
        nameText.text = "";
        accentNameText.text = "";
        */
    }

    public void ReceiveDialogue(string dialogue)
    {
        
        Debug.Log("Dialogue: " + dialogue);
        dialogueText.text = dialogue;
        Debug.Log("DialogueText is: "+ dialogueText.text);
    }

    public void ReceiveName(string speakerName)
    {
       // currentSpeaker = name;
        SetName(speakerName);
    }

    private void SetName(string speakerName)
    {
        accentNameText.text = speakerName;
        nameText.text = speakerName;
    }
    
    
    protected internal override void EnterFadeInDialogue()
    {
        dialoguePanel.GetComponent<Animator>().SetTrigger(FadeInHash);
        namePanel.GetComponent<Animator>().SetTrigger(FadeInHash);
    }

    protected internal override void EnterFadeOutDialogue()
    {
        dialoguePanel.GetComponent<Animator>().SetTrigger(FadeOutHash);
        nameText.text = "";
        accentNameText.text = "";
        dialogueText.text = "";
        namePanel.GetComponent<Animator>().SetTrigger(FadeOutHash);
    }
}
