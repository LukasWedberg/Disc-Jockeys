using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI References")]
    public TMP_Text scoreText;
    public TMP_Text multiplierText;
    public TMP_Text streakText;

    [Header("Score Settings")]
    public int scorePerNote = 100;
    public int scorePerSpin = 250;
    public int[] multiplierThresholds;

    public int currentScore;
    private int currentMultiplier = 1;
    private int streakNumber = 0;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"{currentScore} pts";
        if (multiplierText != null) multiplierText.text = $"Multiplier\n{currentMultiplier}X";
        if (streakText != null) streakText.text = $"Streak\n{streakNumber}";
    }

    public void NoteHit(int scoreValue)
    {
        streakNumber++;
        UpdateMultiplier();
        currentScore += scoreValue * currentMultiplier;
    }

    public void NoteMissed()
    {
        streakNumber = 0;
        currentMultiplier = 1;
    }

    private void UpdateMultiplier()
    {
        currentMultiplier = 1;
        for (int i = 0; i < multiplierThresholds.Length; i++)
        {
            if (streakNumber >= multiplierThresholds[i])
            {
                currentMultiplier = i + 2;
            }
        }
    }
}