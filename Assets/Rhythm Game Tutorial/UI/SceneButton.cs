using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneButton : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private bool useAsyncLoading = false;
    
    private Button button;

    private void Start()
    {
        Debug.Log("Start called");
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        Debug.Log("OnButtonClick");
        if (useAsyncLoading)
        {
            SceneController.Instance.LoadSceneAsync(targetSceneName);
        }
        else
        {
            SceneController.Instance.LoadScene(targetSceneName);
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClick);
    }
}