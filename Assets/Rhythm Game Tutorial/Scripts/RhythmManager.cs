using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public AudioSource theMusic;
    public BeatScroller beatScroller;

    public bool startPlaying;

    public static RhythmManager instance;

    public int currentScore;
    public int scorePerNote = 100;

    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                beatScroller.hasStarted = true;

                // Calculate the music offset based on the BeatScroller's position
                float yOffset = beatScroller.transform.position.y;
                float timeOffset = Mathf.Abs(yOffset) / (beatScroller.beatTempo / 60f); // Convert y offset to time

                // Set the starting time for the music
                theMusic.time = timeOffset;
                theMusic.Play();
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("NoteHit");

        currentScore += scorePerNote;
    }

    public void NoteMissed()
    {
        Debug.Log("NoteMissed");
    }
}