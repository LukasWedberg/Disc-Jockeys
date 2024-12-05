using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public GameObject[] notePrefabs = new GameObject[3];

    public Transform notesHolder;

    public BeatScroller notesHolderScroller;

    public Transform disc;

    public Vector2 leftAndRightBounds = new Vector2(-1,1);


    public Vector3 spawnPos;


    public string rptcID;
    public AK.Wwise.Event wwiseSongEvent;


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

    

    void Start()
    {
        beatScroller.speed = noteSpeed;
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
            notesHolder.transform.position -= Vector3.up * amountToWaitForSync - Vector3.up * extraTinyOffset;

            
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
            else if(notesHolderScroller.hasStarted == false) {
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
            Debug.Log("01 val:" + scaledVal01);
            //Using this scaled value, we can approximate a derivative of the loudness to detect when beats happen.

            float dx = Mathf.Abs(scaledVal01 - lastLoudnessValue);

            float dx2 = Mathf.Abs(dx - lastLoudnessDerivative);






            //float leftAndRightScaledVal = (scaledVal01*2f)-1f;

            float leftAndRightScaledVal = ScaleValue(meterValue, minLoudness, 0.0f, leftAndRightBounds.x, leftAndRightBounds.y);
            Debug.Log("border val:" + leftAndRightScaledVal);


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

            float spawnPosHeightOffset = Mathf.Sqrt((discRadius * discRadius) - (leftAndRightScaledVal * leftAndRightScaledVal));


            spawnPos = new Vector3(leftAndRightScaledVal, transform.position.y + spawnPosHeightOffset, transform.position.z);
            //spawnPos = new Vector3( transform.position.x, transform.position.y + spawnPosHeightOffset, transform.position.z );


            

            if (dx > loudnessDerivativeBeatDetectionThreshold && makeOrPlayChart == playType.MAKECHART && beatsPerSecondLimiterTimer > beatsPerSecondLimiter)
            {
                GameObject newNote = null;// = Instantiate(notePrefabs[0], notesHolder);

                if ( (scaledVal01 - lastLoudnessValue)/2 > loudnessDerivativeBeatDetectionThreshold )
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


            lastLoudnessValue = scaledVal01;

            lastLoudnessDerivative = dx;


        }



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
    }

    public void NoteMissed()
    {
        steakNumber = 0;
        currentMultiplier = 1; // Reset multiplier on a miss
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
