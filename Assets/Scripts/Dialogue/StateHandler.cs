using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    private States.DialogueStates _currentState;
    
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
                NoDialogue();
                break;
            }
        }
    }
    
    protected virtual void FadeInDialogue() {}
    protected virtual void DisplayDialogue() {}
    protected virtual void FadeOutDialogue() {}
    protected virtual void NoDialogue() {}
}
