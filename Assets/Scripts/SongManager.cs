using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System.Numerics;
using UnityEngine.Networking;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public int inputDelayInMilliseconds;
    public float songDelayInSeconds; // InSeconds
    public double marginOfError; // in seconds
    public double travelTime;
    public string fileLocation;
    public float noteTime; // player reaction time
    public float noteSpawnX, noteTapX;

    public Animator playerAnimator;
    public Transform playerTransform;
    private int attackHash = Animator.StringToHash("Attack");
    
    public float noteDespawnX
    {
        get
        {
            return noteTapX - (noteSpawnX - noteTapX);
        }
    }

    public static void PlayVoiceOver(string character, string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"Sound/VA/{character.ToUpper()}/{clipName}");

    }

    public static MidiFile midiFile;

    public Lane[] lanes;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        else
        {
            ReadFromFile();
        }
    }

    private void Update()
    {
        HandlePlayerInput();
    }

    private void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Z) )
        {
            playerAnimator.SetTrigger(attackHash);
            playerTransform.transform.position = new UnityEngine.Vector3(-5, 2, 0);

        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            playerAnimator.SetTrigger(attackHash);
            playerTransform.transform.position = new UnityEngine.Vector3(-5, -0.1f, 0);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            playerAnimator.SetTrigger(attackHash);
            playerTransform.transform.position = new UnityEngine.Vector3(-5, -3, 0);

        }
    }

    private IEnumerator ReadFromWebsite()
    {
        // Make web request
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();
            
            if (www.isNetworkError || www.isHttpError)
                Debug.LogError(www.error);
            else
            {
                {
                    byte[] results = www.downloadHandler.data;
                    using (var stream = new MemoryStream(results))
                    {
                        // Load this stream to the midiFile
                        midiFile = MidiFile.Read(stream);
                        GetDataFromMidi();
                    }
                }
            }
        }
    }

    private void GetDataFromMidi()
    {
        // Want to get notes!
        var notes = midiFile.GetNotes();
        // Cast to vector
        Melanchall.DryWetMidi.Interaction.Note[] notesList = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(notesList, 0);

        foreach (var lane in lanes)
        {
            lane.SetTimeStamps(notesList);
        }
        
        // Further manipulation later

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public static double GetAudioSourceTime()
    {
        // AudioSource.time returns a float instead of a double. We NEED the accuracy.
        
        // Return timeSamples divided by the sample rate/frequency of the clip

        return (double) Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    
    public void StartSong()
    {
        // Plays Audio Source
        audioSource.Play();

        //audioSource.PlayOneShot(audioSource.clip);
        
    }

    private void ReadFromFile()
    {
        Debug.Log(Application.streamingAssetsPath + "/" + fileLocation);
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }
}
