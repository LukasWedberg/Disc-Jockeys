using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using System.Xml;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager instance;

    public AudioSource theMusic;
    public BeatScroller beatScroller;
    public TMP_Text scoreText;
    public TMP_Text multiplierText;
    public TMP_Text streakText;

    public bool startPlaying;

    public int currentScore;
    public int scorePerNote = 100;
    public int scorePerSpin = 250;

    public int currentMultiplier = 1;
    public int[] multiplierThresholds;

    public int steakNumber = 0;






    //This is Lukas adding a few new parameters for Wwise integration!

    public enum playType { 
    
        MAKECHART,
        PLAYCHART


    }


    public playType makeOrPlayChart = playType.PLAYCHART;



    public float notesSpawnDistanceAway = 1;

    public float noteSpeed = 1;

    public float notesDelay = 0;

    public GameObject[] notePrefabs = new GameObject[4];

    public Transform notesHolder;

    public BeatScroller notesHolderScroller;

    public Transform disc;

    public Vector2 leftAndRightBounds = new Vector2(-1,1);


    public Vector3 spawnPos;


    public string rptcID;
    public AK.Wwise.Event wwiseSongEvent;
    public AK.Wwise.Event wwiseHitEvent;
    public AK.Wwise.Event wwiseMissEvent;


    public float meterValue;

    public float minLoudness = -48.0f;


    public float centerBias = 2f;



    private float lastLoudnessValue = 0;

    private float lastLoudnessDerivative = 0;


    public float loudnessDerivativeBeatDetectionThreshold = .1f;


    public float beatsPerSecondLimiter = 1;
    private  float beatsPerSecondLimiterTimer = 0;

    public float delayBeforeSongPlays = 1;
    private float delayTimer = 0;



    private float discRadius = 4;
    private float amountToWaitForSync;

    public float extraTinyOffset = 0;


    public float minDistanceFromSliderToCull = 3;

    private bool currentlyBlue = true;

    private bool beganSong = false;
    

    void Start()
    {
        notesHolderScroller.scrollSpeed = noteSpeed;
        notesHolder.transform.position = new Vector3(0f, 0f, 0f);

        instance = this;
        UpdateScoreText();

        //// Calculate the music offset based on the BeatScroller's position
        //float yOffset = beatScroller.transform.position.y;
        //float timeOffset = Mathf.Abs(yOffset) / (beatScroller.beatTempo / 60f); // Convert y offset to time

        //// Set the starting time for the music
        //theMusic.time = timeOffset;




        //I commented the old stuff out. I'll check later what we'll need.
        //So one thing we'll definitely need to calculate is how long it will take the note to hit the disc's surface

        discRadius = disc.lossyScale.x / 2f;

        float discOuterEdgeElevation = disc.position.y + discRadius;
 

        float spawnerDistFromDiscEdge = transform.position.y  - discOuterEdgeElevation;

        amountToWaitForSync = spawnerDistFromDiscEdge;

        setupChart();


    }


    void setupChart( ) {
        if (makeOrPlayChart == playType.PLAYCHART)
        {
            notesHolder.transform.position -= Vector3.up * amountToWaitForSync - Vector3.up * extraTinyOffset;// - Vector3.up * delayBeforeSongPlays;

            //notesHolderScroller.hasStarted = true;

        }
        else if (makeOrPlayChart == playType.MAKECHART)
        {
            disc.gameObject.SetActive(false);

            //If we're making a new chart, we'd better destroy all pre-existing notes, first!
            foreach (Transform child in notesHolder)
            {
                Destroy(child.gameObject);

               

            }

            
            notesHolderScroller.hasStarted = true;

            //wwiseSongEvent.Post(gameObject);
        }    
    
    
    }


    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                //startPlaying = true;
                //beatScroller.hasStarted = true;
                //theMusic.Play();
            }
        }

        UpdateScoreText();

    }

    private float ScaleValue(float inValue, float inMin, float inMax, float outMin, float outMax)
    {

        return (((outMax - outMin) * (inValue - inMin)) / (inMax - inMin)) + outMin;
    }

    private void FixedUpdate()
    {

        if (makeOrPlayChart == playType.PLAYCHART)
        {
            if (delayTimer < delayBeforeSongPlays)
            {
                delayTimer += Time.fixedDeltaTime;

                Debug.Log("DING DONG, DING DONG, THIS IS THE WAITING SONG, THIS IS THE WAITING SONG");
            }
            else if(beganSong == false) {
                beganSong = true;

                //Time to play the song!

                //PostEvent(eventID, gameObject);

                wwiseSongEvent.Post(gameObject);

                notesHolderScroller.hasStarted = true;
            
            }


        }else if( makeOrPlayChart == playType.MAKECHART) {


            int type = 1;
            AkSoundEngine.GetRTPCValue(rptcID, gameObject, 0, out meterValue, ref type);

            //leftAndRightBounds = new Vector2(-1, 1) * discRadius/2;


            float scaledVal01 = ScaleValue(meterValue, minLoudness, 0.0f, 0.0f, 1.0f);
            //Debug.Log("01 val:"+ (scaledVal01 - lastLoudnessValue) );
            //Using this scaled value, we can approximate a derivative of the loudness to detect when beats happen.

            float dx = Mathf.Abs(scaledVal01 - lastLoudnessValue);

            float dx2 = Mathf.Abs(dx - lastLoudnessDerivative);






            //float leftAndRightScaledVal = (scaledVal01*2f)-1f;

            float leftAndRightScaledVal = ScaleValue(meterValue, minLoudness, 0.0f, leftAndRightBounds.y, leftAndRightBounds.x);
            //Debug.Log("border val:" + leftAndRightScaledVal);


            //Debug.Log(leftAndRightScaledVal);


            //if (leftAndRightScaledVal < 0.0f) {

            //    leftAndRightScaledVal = leftAndRightScaledVal * leftAndRightBounds.x;


            //}
            //else {
            //    leftAndRightScaledVal = leftAndRightScaledVal * leftAndRightBounds.y;

            //}


            //leftAndRightScaledVal = Mathf.Clamp( Mathf.Sign(leftAndRightScaledVal) * Mathf.Pow( Mathf.Abs(leftAndRightScaledVal), centerBias) , leftAndRightBounds.x, leftAndRightBounds.y );

            //meterValue = leftAndRightScaledVal;


            leftAndRightScaledVal = Mathf.Clamp(leftAndRightScaledVal, leftAndRightBounds.x, leftAndRightBounds.y);

            float spawnPosHeightOffset = Mathf.Sqrt( Mathf.Max( (discRadius * discRadius) - (leftAndRightScaledVal * leftAndRightScaledVal),0 ));


            spawnPos = new Vector3(leftAndRightScaledVal, transform.position.y + spawnPosHeightOffset, transform.position.z);
            //spawnPos = new Vector3( transform.position.x, transform.position.y + spawnPosHeightOffset, transform.position.z );



            


            if (dx > loudnessDerivativeBeatDetectionThreshold && makeOrPlayChart == playType.MAKECHART && beatsPerSecondLimiterTimer > beatsPerSecondLimiter)
            {




                GameObject newNote = null;// = Instantiate(notePrefabs[0], notesHolder);

                if ( currentlyBlue ) //(scaledVal01-lastLoudnessValue*2) > 0 )
                {
                    newNote = Instantiate(notePrefabs[0], notesHolder);
                }
                else {

                    newNote = Instantiate(notePrefabs[1], notesHolder);
                }


                Note noteComponent = newNote.GetComponent<Note>();

                noteComponent.speed = noteSpeed;

                noteComponent.timeSpawned = Time.time;

                noteComponent.transform.position = spawnPos;




                beatsPerSecondLimiterTimer = 0;

            }
            else
            {
                beatsPerSecondLimiterTimer += Time.fixedDeltaTime;


            }



            lastLoudnessDerivative = scaledVal01 - lastLoudnessValue;

            lastLoudnessValue = scaledVal01;

            


        }



    }


    public void SwitchColor() {
        currentlyBlue = !currentlyBlue;

        //Debug.Log("Color swap!");
    
    }




    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore} pts";
        }

        if (multiplierText != null)
        {
            multiplierText.text = $"Multiplier\n{currentMultiplier}X";
        }

        if (streakText != null)
        {
            streakText.text = $"Streak\n{steakNumber}";
        }
    }

    public void NoteHit(int scoreValue)
    {
        steakNumber++;
        UpdateMultiplier();
        currentScore += scoreValue * currentMultiplier;

        wwiseHitEvent.Post(gameObject);
        
    }

    public void NoteMissed()
    {
        steakNumber = 0;
        currentMultiplier = 1; // Reset multiplier on a miss


        wwiseMissEvent.Post(gameObject);
    }

    private void UpdateMultiplier()
    {
        currentMultiplier = 1; // Default multiplier
        for (int i = 0; i < multiplierThresholds.Length; i++)
        {
            if (steakNumber >= multiplierThresholds[i])
            {
                currentMultiplier = i + 2; // Set multiplier to index + 1
            }
        }
    }


    public void SpawnNote(AkEventCallbackMsg in_info) {

        //if (makeOrPlayChart == playType.MAKECHART)
        //{
        //    AkMusicSyncCallbackInfo infoWithcueName = (AkMusicSyncCallbackInfo)in_info.info;

        //    string cueName = infoWithcueName.userCueName;

        //    Debug.Log("Cue Name: " + cueName);

        //    GameObject newNote = Instantiate(notePrefabs[0], notesHolder);

        //    Note noteComponent = newNote.GetComponent<Note>();

        //    noteComponent.speed = noteSpeed;

        //    noteComponent.timeSpawned = Time.time;

        //    noteComponent.transform.position = spawnPos;
        //}

    }


    public void ReadCue(AkEventCallbackMsg in_info) {

        if (makeOrPlayChart == playType.MAKECHART)
        {
            AkMusicSyncCallbackInfo infoWithcueName = (AkMusicSyncCallbackInfo)in_info.info;

            string cueName = infoWithcueName.userCueName;

            Debug.Log("Cue Name: " + cueName);

            if (cueName == "LEFT")
            {
                Debug.Log("JUST MAKE THE DARN SLIDER!!!a");

                GameObject newNote = Instantiate(notePrefabs[2], notesHolder);

                

                newNote.transform.position = transform.position + Vector3.up * discRadius/2f;
            }

            if (cueName == "RIGHT")
            {
                GameObject newNote = Instantiate(notePrefabs[3], notesHolder);

                newNote.transform.position = transform.position + Vector3.up * discRadius / 2f;
            }

            float attemptedNumberParse;

            if (float.TryParse( cueName , out attemptedNumberParse ))
            {
                loudnessDerivativeBeatDetectionThreshold = attemptedNumberParse;


            }




        }

        Debug.Log("THIS IS WHERE WE MAKE A SLIDER");



    }






    void FinishChart(AkEventCallbackMsg msg) {

        if (makeOrPlayChart == playType.MAKECHART)
        {

            notesHolderScroller.hasStarted = false;

        }



    }


    private void OnDrawGizmos()
    {
        //leftAndRightBounds = new Vector2( -1 , 1) * disc.lossyScale.x/2;

        //Draw a sphere for the left bounds
        Gizmos.DrawSphere(transform.position + Vector3.right * leftAndRightBounds.x,  1);    
    
        
        //Draw a sphere for the right bounds
        Gizmos.DrawSphere(transform.position + Vector3.right * leftAndRightBounds.y,  1);

        Gizmos.DrawCube( spawnPos, Vector3.one);


    }


}
