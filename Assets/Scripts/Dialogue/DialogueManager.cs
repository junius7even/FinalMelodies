using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class States
{
    public enum DialogueStates
    {
        FadeInDialogue,
        DisplayDialogue,
        FadeOutDialogue,
        NoDialogue,
    }
}

public class DialogueManager : StateHandler
{
    [SerializeField] private PortraitManager portraitManager;
    [SerializeField] private TextManager textManager;
    private States.DialogueStates _currentState;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentState = States.DialogueStates.NoDialogue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void FadeInDialogue()
    {
        base.FadeInDialogue();
    }

    protected override void DisplayDialogue()
    {
        base.DisplayDialogue();
    }

    protected override void NoDialogue()
    {
        base.NoDialogue();
    }

    protected override void FadeOutDialogue()
    {
        base.FadeOutDialogue();
    }
}
