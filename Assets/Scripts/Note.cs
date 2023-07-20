using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private double timeInstantiated;

    public float assignedTime; // Time the note is supposed to be tapped by the player
    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - assignedTime;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
        
        // Destroy game object if 
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // Movement towards the end of the screen
            transform.localPosition = Vector3.Lerp(Vector3.right * SongManager.Instance.noteSpawnX,
                Vector3.right * SongManager.Instance.noteDespawnX, t);
        }
    }
}
