using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private GameObject currentMenu;

    void Start()
    {
        // Find first active menu
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                currentMenu = child.gameObject;
                break;
            }
        }
    }

    public void SwitchMenu(GameObject newMenu)
    {
        if (currentMenu != null)
            currentMenu.SetActive(false);
        
        currentMenu = newMenu;
        currentMenu.SetActive(true);
    }
}