using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameEnded = false;
    
    [SerializeField] private float sceneTransitionDelay = 2f;
    [SerializeField] private string nextSceneName = "ResultScene";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!gameEnded && BeatScroller.instance != null)
        {
            // Check if all child objects are destroyed
            if (BeatScroller.instance.transform.childCount == 0)
            {
                EndGame();
            }
        }
    }

    public void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            StartCoroutine(TransitionToResultScene());
        }
    }

    private IEnumerator TransitionToResultScene()
    {
        // Store the final score
        PlayerPrefs.SetInt("FinalScore", ScoreManager.instance.currentScore);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(sceneTransitionDelay);
        
        // Load the result scene
        SceneController.Instance.LoadScene(nextSceneName);
    }
}