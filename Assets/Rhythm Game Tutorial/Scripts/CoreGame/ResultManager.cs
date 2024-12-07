using UnityEngine;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TMP_Text finalScoreText;

    void Start()
    {
        // Retrieve and display the final score
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore} pts";
        }
    }
}