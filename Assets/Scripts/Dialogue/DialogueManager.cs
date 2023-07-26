using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.SceneManagement;

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
    public TextAsset storyToPlay;
    [SerializeField] private TextAsset[] storiesToPlay;
    public SoundManager soundManager;

    private string nextStoryLine = "";
    private string nextSpeaker = "";
    private string currentSpeaker = "";

    private string[] validCharacters = { "penelope", "ithma", "titus", "thevoice", "knight", "elethea" };
    private string[] validSfx = { "Slash", "FireWood", "door break", "door knock1", "door knock2", "DropDown-Wood", "Heartbeat"};
    private string[] validMusic = { "Nocturne", "calm-music", "DiesIrae"};
    private string[] validBattles = { "Level1", "Level2", "Ending" };
    
    // INK related variables
    [SerializeField]public static int currentStoryNumber = 0;
    private Story currentStory; // Holds the current script

    private bool displayingChoices;
    private bool animationFinished;
    private bool finishedStory = false;

    private bool shouldSceneChange;
    
    // Start is called before the first frame update
    void Start()
    {
        // EnterDialogueMode(storyToPlay);

        // EnterDialogueMode(storiesToPlay[currentStoryNumber]);
        // storyToPlay = Resources.Load<TextAsset>($"Dialogue/A1S2");
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
            textManager.ReceiveDialogue(nextStoryLine);
            HandleTags();
            if (States.CurrentState == States.DialogueStates.NoDialogue)
                SwitchStates(States.DialogueStates.FadeInDialogue);
            Debug.Log("Next storyline: " + nextStoryLine);
            
        }
        else
        {
            SwitchStates(States.DialogueStates.FadeOutDialogue);
            finishedStory = true;
            if (currentStoryNumber < storiesToPlay.Length)
            {
                currentStoryNumber++;
                Debug.Log("Incremented: " + currentStoryNumber);
            }
        }
    }

    private void Update()
    {
        Debug.Log("Storiestoplay.length: " + storiesToPlay.Length);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SwitchStates(States.DialogueStates.NoDialogue);
        Debug.Log("Current story number: " + currentStoryNumber);
        EnterDialogueMode(storiesToPlay[currentStoryNumber]);
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
                    // TODO: Handle voiceover and music
                    if (validMusic.Contains(storyTag))
                    {
                        soundManager.PlayAmbience(storyTag);
                    }
                    else if (validSfx.Contains(storyTag))
                    {
                        soundManager.PlayEffect(storyTag);
                    }
                    else if (validBattles.Contains(storyTag))
                    {
                        SwitchStates(States.DialogueStates.FadeOutDialogue);
                        finishedStory = true;
                        if (currentStoryNumber < storiesToPlay.Length)
                        {
                            currentStoryNumber++;
                            Debug.Log("Incremented: " + currentStoryNumber);
                        }
                        SceneManager.LoadScene(storyTag);
                    }
                    // The rest are all voiceovers
                    else
                    {
                        soundManager.PlayVoiceOver(speakerName, storyTag);
                    }
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
        
        portraitManager.LoadPortrait(speakerName + portraitNumber);
        
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
        SwitchStates(States.DialogueStates.NoDialogue);
    }

    protected internal override void EnterDisplayDialogue()
    {
        base.EnterDisplayDialogue();
        portraitManager.EnterDisplayDialogue();
        textManager.EnterDisplayDialogue();
    }

    protected internal override void EnterNoDialogue()
    {
        base.EnterNoDialogue();
        portraitManager.EnterNoDialogue();
        textManager.EnterNoDialogue();
    }
}
