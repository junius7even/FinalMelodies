using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : StateHandler
{
    public Animator portraitAnimator;
    public Animator accentAnimator;
    [SerializeField] private Image portraitImage;
    // Black image used to give depth to the portrait
    [SerializeField] private Image accentImage;
    public bool animationFinished;
    private string PortraitSpritePath = "../Portraits/";

    private void LateUpdate()
    {
        base.LateUpdate();
        animationFinished = portraitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }
    
    protected override void FadeInDialogue()
    {
       portraitAnimator.SetTrigger(FadeInHash);
       accentAnimator.SetTrigger(FadeInHash);
       if (portraitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && accentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
       {
           SwitchStates(States.DialogueStates.DisplayDialogue);
       }
    }
    public void SwapSpeakers(string nextSpeaker)
    {
        portraitAnimator.SetTrigger(FadeOutHash);
        portraitImage.sprite = Resources.Load<Sprite>(PortraitSpritePath+nextSpeaker);
        accentImage.sprite = Resources.Load<Sprite>(PortraitSpritePath+nextSpeaker);
        portraitAnimator.SetTrigger(FadeInHash);
    }

    protected override void FadeOutDialogue()
    {
        portraitAnimator.SetTrigger(FadeOutHash);
        accentAnimator.SetTrigger(FadeOutHash);
        if (portraitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && accentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            SwitchStates(States.DialogueStates.NoDialogue);
        }
    }
    
    
}
