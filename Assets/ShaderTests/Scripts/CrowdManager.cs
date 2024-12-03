using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{


    [Header("Use these to adjust the size and position \n of the cube, where the camera is coded to patrol.")]
    public Vector3 crowdBoundsMinPos = -Vector3.one;
    public Vector3 crowdBoundsMaxPos = Vector3.one;






    [Header("Cube Stats--for display purposes only!")]
    public Vector3 cubeCenter = Vector3.zero;
    public Vector3 cubeSize = Vector3.one * 2;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateBounds();
    }


    

    private void CalculateBounds() {
        cubeCenter = 0.5f * (crowdBoundsMinPos + crowdBoundsMaxPos);

        cubeSize = crowdBoundsMaxPos - crowdBoundsMinPos;
    }


    public bool CheckBounds(Vector3 posToCheck) { 
        
        Vector3 clampedPos = Vector3.Min(Vector3.Max(crowdBoundsMinPos, posToCheck), crowdBoundsMaxPos);

        if (posToCheck == clampedPos)
        {

            return true;
        }




        return false;

    }




    public Vector3 RandomPosInBounds()
    {
        Vector3 randomPos = cubeCenter;

        randomPos = cubeCenter + Vector3.Scale(cubeSize, .5f* new Vector3(  Random.value*2-1 , Random.value * 2 - 1, Random.value * 2 - 1)) ;



        return randomPos;




    }


    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(crowdBoundsMinPos, 0.1f);
        Gizmos.DrawSphere(crowdBoundsMaxPos, 0.1f);


        Gizmos.color = new Color(Mathf.Sin(Time.realtimeSinceStartup * 2) * .5f + .5f, Mathf.Sin(Time.realtimeSinceStartup * 2) *.5f + .5f, 1, .5f);



        CalculateBounds();

        Gizmos.DrawCube(cubeCenter,cubeSize);



        Gizmos.color = Color.green;


        //Gizmos.DrawSphere( RandomPosInBounds(), 1 );



    }


}
