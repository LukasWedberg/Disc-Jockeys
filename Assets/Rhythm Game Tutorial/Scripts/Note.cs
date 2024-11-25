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

    public NoteColor noteColor; // Set the note color in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check collision with tags
        if (noteColor == NoteColor.Red && other.CompareTag("Red"))
        {
            RhythmManager.instance.NoteHit();
            Destroy(gameObject); // Destroy or deactivate the note after hit
        }
        else if (noteColor == NoteColor.Blue && other.CompareTag("Blue"))
        {
            RhythmManager.instance.NoteHit();
            Destroy(gameObject);
        }
        else
        {
            RhythmManager.instance.NoteMissed();
            Destroy(gameObject);
        }
    }
}