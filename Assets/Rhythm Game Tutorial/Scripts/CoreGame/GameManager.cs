using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameEnded = false;
    
    [SerializeField] private float sceneTransitionDelay = 2f;
    [SerializeField] private string nextSceneName = "ResultScene";

    BeatScroller scroller = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        scroller = GameObject.Find("Notes").GetComponent<BeatScroller>(); ;
    }

    private void Update()
    {
        if (!gameEnded && scroller != null)
        {
            // Check if all child objects are destroyed
            if (scroller.transform.childCount == 0)
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