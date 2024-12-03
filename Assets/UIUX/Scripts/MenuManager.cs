using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    GameObject currentMenu;


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {

            if (child.gameObject.activeSelf) {
                currentMenu = child.gameObject;
            
            
            }


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchMenu(GameObject menuToSwitchTo) {



        //if (deactivateParentOnSwitch)
        //{

        //    if (parentMenu)
        //    {
        //        Debug.Log("Deactivating:" + parentMenu.name + "!");

        //        parentMenu.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        transform.parent.gameObject.SetActive(false);
        //    }


        //}
        //else
        //{
        //    Debug.Log("NOT deactivating:" + parentMenu.name + "!");


        //}


        Debug.Log("Deactivating:" + currentMenu.name + "!");
        currentMenu.SetActive(false);

        currentMenu = menuToSwitchTo;

        menuToSwitchTo.SetActive(true);

    }




}
