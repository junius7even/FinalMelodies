using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    protected int FadeInHash = Animator.StringToHash("FadeIn");
    protected int FadeOutHash = Animator.StringToHash("FadeOut");
    /*
    protected virtual void HandleEnterState(States.DialogueStates nextDialogueState)
    {
        States.LastState = States.CurrentState;
        States.CurrentState = nextDialogueState;
        switch (nextDialogueState)
        {
            case States.DialogueStates.FadeInDialogue:
            {
                EnterFadeInDialogue();
                break;
            }
            case States.DialogueStates.DisplayDialogue:
            {
                EnterDisplayDialogue();
                break;
            }
            case States.DialogueStates.FadeOutDialogue:
            {
                EnterFadeOutDialogue();
                break;
            }
            case States.DialogueStates.NoDialogue:
            {
                EnterNoDialogue();
                break;
            }
        }
    
    */
    protected virtual void LateUpdate()
    {
        HandleStates(States.CurrentState);
    }

    protected void SwitchStates(States.DialogueStates nextState)
    {
        States.LastState = States.CurrentState;
        States.CurrentState = nextState;
    }

    protected internal void HandleStates(States.DialogueStates currentDialogueState)
    {
        switch (currentDialogueState)
        {
            case States.DialogueStates.FadeInDialogue:
            {
                FadeInDialogue();
                break;
            }
            case States.DialogueStates.DisplayDialogue:
            {
                DisplayDialogue();
                break;
            }
            case States.DialogueStates.FadeOutDialogue:
            {
                FadeOutDialogue();
                break;
            }
            case States.DialogueStates.NoDialogue:
            {
                Debug.Log("No Dialogue");
                NoDialogue();
                break;
            }
        }
    }
    
    protected virtual void FadeInDialogue() {}
    protected virtual void DisplayDialogue() {}
    protected virtual void FadeOutDialogue() {}
    protected virtual void NoDialogue() {}
    
    /*
    protected virtual void EnterFadeInDialogue() {}
    protected virtual void EnterDisplayDialogue() {}
    protected virtual void EnterFadeOutDialogue() {}
    protected virtual void EnterNoDialogue() {}
    */
}
