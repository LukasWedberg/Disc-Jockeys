using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public enum NoteColor
    {
        Blue,
        Red
    }


    public float speed = 1f;

    public NoteColor noteColor; // Set the note color in the Inspector

    public float timeSpawned = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check collision with tags
        if (noteColor == NoteColor.Red && other.CompareTag("Red"))
        {
            ScoreManager.instance.NoteHit(ScoreManager.instance.scorePerNote);
            Destroy(gameObject);
        }
        else if (noteColor == NoteColor.Blue && other.CompareTag("Blue"))
        {
            ScoreManager.instance.NoteHit(ScoreManager.instance.scorePerNote);
            Destroy(gameObject);
        }
        else
        {
            ScoreManager.instance.NoteMissed();
            Destroy(gameObject);
        }
    }

    public void FixedUpdate()
    {
        //transform.position += Vector3.down * speed * Time.fixedDeltaTime;   



    }
}