using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Ink.Runtime;

public static class States
{
    public enum DialogueStates
    {
        FadeInDialogue,
        DisplayDialogue,
        FadeOutDialogue,
        NoDialogue,
    }

    public static DialogueStates CurrentState = DialogueStates.NoDialogue;
    public static DialogueStates LastState = DialogueStates.NoDialogue;
}

public class DialogueManager : StateHandler
{
    [SerializeField] private PortraitManager portraitManager;
    [SerializeField] private TextManager textManager;
    [SerializeField] private TextAsset storyToPlay;
    [SerializeField] private TextAsset[] storiesToPlay;
    private string nextStoryLine = "";
    private string nextSpeaker = "";
    private string currentSpeaker = "";

    private string[] validCharacters = { "penelope", "ithma", "titus", "thevoice" };
    private string[] validSfx = { "Stab", "FireWood"};
    
    // INK related variables
    private int currentStoryNumber = 0;
    private Story currentStory; // Holds the current script

    private bool displayingChoices;
    private bool animationFinished;
    private bool finishedStory = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // EnterDialogueMode(storyToPlay);
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            Debug.Log("Thingy");
            // Get the next line of dialogue
            nextStoryLine = currentStory.Continue();
            HandleTags();
            Debug.Log("Next storyline: " + nextStoryLine);
            textManager.ReceiveDialogue(nextStoryLine);
            if (States.CurrentState == States.DialogueStates.NoDialogue)
                SwitchStates(States.DialogueStates.FadeInDialogue);
        }
        else
        {
            SwitchStates(States.DialogueStates.FadeOutDialogue);
            finishedStory = true;
        }
    }

    private void HandleTags()
    {
        string speakerName = "";
        string portraitNumber = "";
        bool swapSpeaker = false;
        if (currentStory.currentTags.Count > 0)
        {
            for (int i = 0; i < currentStory.currentTags.Count; i++)
            {
                string storyTag = currentStory.currentTags[i];
                int number;
                // If there's a valid character
                Debug.Log("tag: " + storyTag);

                if (validCharacters.Contains(storyTag.ToLower()))
                {
                    speakerName = storyTag;
                    if (speakerName != currentSpeaker)
                    {
                        swapSpeaker = true;
                    }
                }
                else if (int.TryParse(storyTag, out number))
                {
                    portraitNumber = storyTag;
                }
                else 
                {
                    // TODO: Handle voiceover
                }
            }
        }
        else
        {
            if (currentSpeaker != "")
                swapSpeaker = true;
        }
        // Set the new speaker to the one now --> one to be displayed
        currentSpeaker = speakerName;
        textManager.ReceiveName(speakerName);
        if (swapSpeaker)
            portraitManager.LoadPortrait(speakerName + portraitNumber);
        else
        {
            portraitManager.LoadPortrait("");

        }
        /*
        if (swapSpeaker)
            portraitManager.SwapSpeakers(speakerName + portraitNumber);
        else
        {
            portraitManager.LoadPortrait(speakerName + portraitNumber);
        }
        */
    }
    
    // OVERRIDEN FUNCTIONS
    protected override void DisplayDialogue()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            ContinueStory();
    }

    protected override void NoDialogue()
    {
        base.NoDialogue();
        if (!finishedStory)
            EnterDialogueMode(storyToPlay);
    }

    protected internal override  void EnterFadeInDialogue()
    {
        base.EnterFadeInDialogue();
        textManager.EnterFadeInDialogue();
        portraitManager.EnterFadeInDialogue();
    }

    protected internal override void EnterFadeOutDialogue()
    {
        base.EnterFadeOutDialogue();
        textManager.EnterFadeOutDialogue();
        portraitManager.EnterFadeOutDialogue();
    }

    protected internal override void EnterDisplayDialogue()
    {
        base.EnterDisplayDialogue();
        portraitManager.EnterDisplayDialogue();
        textManager.EnterDisplayDialogue();
    }

    protected internal override void EnterNoDialogue()
    {
        if (currentStoryNumber < storiesToPlay.Length)
        base.EnterNoDialogue();
        portraitManager.EnterNoDialogue();
        textManager.EnterNoDialogue();
    }
}
