using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public Animator titleCover;

    public void StartFadeOut()
    {
        titleCover.SetTrigger("FadeOut");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
