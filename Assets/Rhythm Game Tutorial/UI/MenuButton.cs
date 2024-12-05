using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject targetMenu;
    [SerializeField] private bool cycleToNext;
    
    private MenuManager menuManager;

    void Start()
    {
        menuManager = transform.root.GetComponentInChildren<MenuManager>();
        
        if (cycleToNext && !targetMenu)
        {
            Transform menuHolder = menuManager.transform;
            int nextIndex = transform.parent.GetSiblingIndex() + 1;
            targetMenu = menuHolder.GetChild(nextIndex % menuHolder.childCount).gameObject;
        }
    }

    public void OnClick()
    {
        if (targetMenu && menuManager)
            menuManager.SwitchMenu(targetMenu);
    }
}
