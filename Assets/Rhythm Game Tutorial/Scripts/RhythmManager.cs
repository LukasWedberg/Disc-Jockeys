using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RhythmManager : MonoBehaviour
{
    public AudioSource theMusic;
    public BeatScroller beatScroller;

    public bool startPlaying;

    public static RhythmManager instance;

    public int currentScore;
    public int scorePerNote = 100;
    
    public TMP_Text scoreText;

    void Start()
    {
        instance = this;
        UpdateScoreText();
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

        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore} pts";
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