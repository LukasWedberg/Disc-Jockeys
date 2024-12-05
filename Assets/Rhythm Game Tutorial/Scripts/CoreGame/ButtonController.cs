using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer thisSprite;

    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;
    
    // Start is called before the first frame update
    void Start()
    {
        thisSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            thisSprite.sprite = pressedImage;
        }

        if (Input.GetKeyUp(keyToPress))
        {
            thisSprite.sprite = defaultImage;
        }
    }
}
