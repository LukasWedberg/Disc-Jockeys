using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager instance;

    [Header("References")]
    public AudioSource theMusic;
    public BeatScroller beatScroller;

    public bool startPlaying;

    void Start()
    {
        instance = this;
        
        float yOffset = beatScroller.transform.position.y;
        float timeOffset = Mathf.Abs(yOffset) / (beatScroller.beatTempo / 60f);
        theMusic.time = timeOffset;
    }

    void Update()
    {
        if (!startPlaying && Input.anyKeyDown)
        {
            startPlaying = true;
            beatScroller.hasStarted = true;
            theMusic.Play();
        }
    }
}