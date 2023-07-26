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
    private string PortraitSpritePath = "Portraits/";

    private portraitStates currentState = portraitStates.DC;

    private enum portraitStates
    {
        FadingOut,
        DC,
    }
    private void LateUpdate()
    {
        base.LateUpdate();
        animationFinished = portraitAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
        Debug.Log(portraitAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name );

    }

    protected override void FadeInDialogue()
    {
        base.FadeInDialogue();
        if (animationFinished)
        {
            SwitchStates(States.DialogueStates.DisplayDialogue);
        }
    }

    protected override void FadeOutDialogue()
    {
        if (animationFinished)
        {
            SwitchStates(States.DialogueStates.NoDialogue);
        }
    }

    
    protected override internal void EnterFadeInDialogue()
    {
       portraitAnimator.SetTrigger(FadeInHash);
       accentAnimator.SetTrigger(FadeInHash);
    }
    
    public void SwapSpeakers(string nextSpeaker)
    {
        Debug.Log("Swapping speakers");
        Debug.Log("Last state: " +States.LastState);
        
        portraitImage.sprite = Resources.Load<Sprite>(PortraitSpritePath+nextSpeaker);
        accentImage.sprite = Resources.Load<Sprite>(PortraitSpritePath+nextSpeaker);
    }

    public void LoadPortrait(string imageName)
    {
        Debug.Log("Image name: " + imageName);
        Debug.Log("Path" + PortraitSpritePath + imageName);
        if (imageName == "")
            return;
        portraitImage.sprite = Resources.Load<Sprite>(PortraitSpritePath+imageName);
        accentImage.sprite = Resources.Load<Sprite>(PortraitSpritePath+imageName);
    }

    protected internal override void EnterFadeOutDialogue()
    {
        Debug.Log("Entering fadeout dialogue");
        portraitAnimator.SetTrigger(FadeOutHash);
        accentAnimator.SetTrigger(FadeOutHash);
    }
    
    
}
