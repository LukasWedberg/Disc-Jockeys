using UnityEngine;

public class Stars : MonoBehaviour
{
    public int maxScore;
    public SpriteRenderer[] starSprites = new SpriteRenderer[5];
    public Sprite targetSprite;

    private ScoreManager scoreManager;
    private int scoreThreshold;
    private int currentStars = 0;

    void Start()
    {
        scoreManager = ScoreManager.instance;
        scoreThreshold = maxScore / 5;
    }

    void Update()
    {
        int scoreBasedStars = Mathf.FloorToInt(scoreManager.currentScore / scoreThreshold);
        
        if (scoreBasedStars > currentStars && currentStars < 5)
        {
            starSprites[currentStars].sprite = targetSprite;
            currentStars++;
        }
    }
}