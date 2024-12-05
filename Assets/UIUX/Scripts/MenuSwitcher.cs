using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{

    public GameObject menuToSwitchTo;

    int myChildIndex;

    Transform menuHolder;

    GameObject nextMenu = null;

    Transform parentMenu = null;

    //public bool deactivateParentOnSwitch = true;

    public MenuManager menuManager;


    // Start is called before the first frame update
    void Start()
    {
        

        menuHolder = GameObject.Find("ChangingMenusHolder").transform;

        menuManager = menuHolder.GetComponent<MenuManager>();

        FindParentMenu();

        myChildIndex = parentMenu.GetSiblingIndex();


        nextMenu = menuHolder.GetChild((myChildIndex + 1)%menuHolder.childCount ).gameObject;




    }

    // Update is called once per frame
    void Update()
    {
        



    }


    public void SwitchMenu() {


        GameObject targetMenu = null;

        if (menuToSwitchTo)
        {
            Debug.Log("Activating:" + menuToSwitchTo.name + "!");

            targetMenu = menuToSwitchTo;
        }
        else
        {

            Debug.Log("Activating:" + nextMenu.name + "!");

            targetMenu = nextMenu.gameObject;
        }



        menuManager.switchMenu(targetMenu);

    }


    private void FindParentMenu()
    {
        foreach (Transform child in menuHolder) {

            if (transform.IsChildOf(child))
            {
                parentMenu = child;

                return;
            }
        
        
        }

        parentMenu = transform.parent;

    }

}
