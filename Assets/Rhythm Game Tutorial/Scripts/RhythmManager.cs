using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager instance;

    public AudioSource theMusic;
    public BeatScroller beatScroller;
    public TMP_Text scoreText;
    public TMP_Text multiplierText;
    public TMP_Text streakText;

    public bool startPlaying;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerSpin = 250;

    public int currentMultiplier = 1;
    public int[] multiplierThresholds;

    public int steakNumber = 0;

    void Start()
    {
        instance = this;
        UpdateScoreText();

        // Calculate the music offset based on the BeatScroller's position
        float yOffset = beatScroller.transform.position.y;
        float timeOffset = Mathf.Abs(yOffset) / (beatScroller.beatTempo / 60f); // Convert y offset to time

        // Set the starting time for the music
        theMusic.time = timeOffset;
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

        if (multiplierText != null)
        {
            multiplierText.text = $"Multiplier\n{currentMultiplier}X";
        }

        if (streakText != null)
        {
            streakText.text = $"Streak\n{steakNumber}";
        }
    }

    public void NoteHit(int scoreValue)
    {
        steakNumber++;
        UpdateMultiplier();
        currentScore += scoreValue * currentMultiplier;
    }

    public void NoteMissed()
    {
        steakNumber = 0;
        currentMultiplier = 1; // Reset multiplier on a miss
    }

    private void UpdateMultiplier()
    {
        currentMultiplier = 1; // Default multiplier
        for (int i = 0; i < multiplierThresholds.Length; i++)
        {
            if (steakNumber >= multiplierThresholds[i])
            {
                currentMultiplier = i + 2; // Set multiplier to index + 1
            }
        }
    }
}
