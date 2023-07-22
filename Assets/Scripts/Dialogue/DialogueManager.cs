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
    private string nextStoryLine = "";
    private string nextSpeaker = "";
    private string currentSpeaker = "";

    private string[] validCharacters = { "penelope", "ithma", "titus", "thevoice" };
    
    // INK related variables
    private Story currentStory; // Holds the current script

    private bool displayingChoices;
    private bool animationFinished;
    
    // Start is called before the first frame update
    void Start()
    {
        EnterDialogueMode(storyToPlay);
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
        // Set the new speaker to the one now --> one to be displayed
        currentSpeaker = speakerName;
        textManager.ReceiveName(speakerName);
        if (swapSpeaker)
        {
            portraitManager.SwapSpeakers(speakerName + portraitNumber);
        }
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
        
    }
}
