using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    public KeyCode input;

    public GameObject notePrefab;

    private List<Note> notes = new List<Note>();

    public List<double> timeStamps = new List<double>();

    
    private int spawnIndex = 0;
    private int inputIndex = 0;

    
    private int takeDamageHash = Animator.StringToHash("TakeDamage");

    void Update()
    {
        // Handle spawning notes
        // Check if there's more notes TO be spawned
        
        // Checks the timeStamps to see if there's more notes to spawn
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime) ;
            {
                GameObject note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                // Assignedtime is the spawn time minus the amount of time the note is supposed to be available for
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex] - SongManager.Instance.noteTime - (float)SongManager.Instance.travelTime; //  
                spawnIndex++;
            }
        }

        // Check each timestamps for notes one by one
        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() -
                               (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input))
            {
                // Checks if the note should be destroyed based on the timing
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note ");
                }
            }

            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }

    private void Hit()
    {
        ComboManager.Hit();
    }

    private void Miss()
    {
        ComboManager.Miss();
        SongManager.Instance.playerAnimator.SetTrigger(takeDamageHash);
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                // note.Time returns a time into a tempoMap. Use a timeConverter to convert to metric time stamp.
                MetricTimeSpan metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double) metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double) metricTimeSpan.Milliseconds/1000f); // Milliseconds is 1/1000 seconds
            }
        }
    }
}
