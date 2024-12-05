using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    




    public Transform orbitDJPoint;
    public float orbitDJDist = 1;
    public float orbitDJSpeed = .1f;




    public Vector3 scrollTrajectory = Vector3.zero;
    private Vector3 scrollStartPos;
    public float scrollOutwardOffset;
    public float scrollBackwardOffset;
    public float crowdScrollSpeed;
    //public float preferredScrollTrajectoryAngle;

    public Transform crowdHolder;


    public enum camState {
        IDLE,
        ORBITING_DJ,
        SCROLLING_CROWD,
        DADES_ORIGINAL_POV,
        RANDOM_FOCUS_CHARACTER   
    
    }

    public camState currentState = camState.IDLE;


    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) { 
            case camState.IDLE:
                

                break; 
        

            case camState.ORBITING_DJ:
                
                transform.position = orbitDJPoint.position + new Vector3( Mathf.Sin(Time.realtimeSinceStartup * orbitDJSpeed), 0, Mathf.Cos(Time.realtimeSinceStartup * orbitDJSpeed)) * orbitDJDist;
                
                transform.LookAt(orbitDJPoint.position);


                break;

            case camState.SCROLLING_CROWD:

                //So first we'll need to find a good trajectory to fling the camera in.


                //We'll find a trajectory by looking for straight lines between 3 crowd members.


                if (scrollTrajectory == Vector3.zero)
                {
                    for (int i = 1; i < crowdHolder.childCount; i++)
                    {



                        int previousIndex = (i - 1) % crowdHolder.childCount;
                        int nextIndex = (i + 1) % crowdHolder.childCount;

                        Debug.Log("Previous: " + previousIndex + "\n" + "Current: " + i + "\n" + "Next: " + nextIndex + "\n");

                        Transform previousChild = crowdHolder.GetChild(previousIndex);

                        Transform currentChild = crowdHolder.GetChild(i);

                        Transform nextChild = crowdHolder.GetChild(nextIndex);

                        Vector3 previousDirection = (currentChild.position - previousChild.position).normalized;

                        Vector3 nextDirection = (nextChild.position - currentChild.position).normalized;

                        if (Vector3.Dot(ScaleVectorPurelyLateral(previousDirection), ScaleVectorPurelyLateral(nextDirection)) > .5f)
                        {

                            //Getting the average of the two directions for eased scrolling
                            scrollTrajectory = ScaleVectorPurelyLateral(nextDirection + previousDirection).normalized;

                            //But we also want to offset the camera in such a way that when we
                            //scroll we're looking at the faces of the crowd members and not the back!

                            Vector3 testingOffsetDirection = Vector3.Cross(scrollTrajectory, Vector3.up);

                            Vector3 averageCrowdMemberFacingDirection = ScaleVectorPurelyLateral((previousChild.forward + currentChild.forward + nextChild.forward).normalized);


                            Vector3 offsetDirection = (Vector3.Dot(averageCrowdMemberFacingDirection, testingOffsetDirection) > 0) ? testingOffsetDirection : -testingOffsetDirection;



                            //Now to set the initial position and rotation of the camera

                            transform.position = previousChild.position + offsetDirection * scrollOutwardOffset;

                            transform.position -= scrollTrajectory * scrollBackwardOffset * .5f;

                            transform.LookAt(previousChild);

                            transform.position -= scrollTrajectory * scrollBackwardOffset * .5f;



                            previousChild.position += Vector3.up;
                            currentChild.position += Vector3.up;
                            nextChild.position += Vector3.up;





                            break;
                        }






                    }

                    if (scrollTrajectory == Vector3.zero)
                    {
                        //This means we haven't found a good spot to scroll the camera through.
                        //Better just jerryrig the whole thing!

                        scrollTrajectory = Vector3.right;

                        transform.position = orbitDJPoint.position - transform.forward * orbitDJDist;

                        transform.position -= scrollTrajectory * scrollBackwardOffset * .5f;

                        transform.LookAt(orbitDJPoint);

                        transform.position -= scrollTrajectory * scrollBackwardOffset * .5f;



                    }

                }
                else {


                    //Then we actually send the camera that way

                    Debug.DrawRay(transform.position, scrollTrajectory, Color.yellow);

                    transform.position += scrollTrajectory * Time.deltaTime * crowdScrollSpeed;
                
                }

               



                


                
                break;


            case camState.DADES_ORIGINAL_POV:

                transform.position = orbitDJPoint.position + new Vector3(0,3, 8);

                transform.LookAt(orbitDJPoint.position);

                break;


            case camState.RANDOM_FOCUS_CHARACTER:



                break;

              



        }
        


    }

    //This function modifies a vector so that it has the same length, but no vertical direction.
    //It will only go along the x and z axis.
    public Vector3 ScaleVectorPurelyLateral(Vector3 vectorToScale) {
        
        Vector3 scaledVector = Vector3.Scale(vectorToScale, Vector3.one-Vector3.up ).normalized * vectorToScale.magnitude;

        return scaledVector;
    }


    public void RandomState(AkEventCallbackMsg in_info)
    {

        //AkEventCallbackMsg _info

        int randomIndex = (int)Mathf.Floor(Random.value * 4.9999f);

        //currentState = (camState)randomIndex;

        AkMusicSyncCallbackInfo mInfo = (AkMusicSyncCallbackInfo)in_info.info;

        int attemptedParse;
            
        

        if (int.TryParse(mInfo.userCueName, out attemptedParse))
        {
            Debug.Log(mInfo.userCueName + " is a number!");

            currentState = (camState)attemptedParse;
        }


        //AkUnitySoundEngine.StringFromIntPtrString(AkUnitySoundEnginePINVOKE.CSharp_AkMusicSyncCallbackInfo_userCueName_get(swigCPtr));

        //in_info.info.

        
       
        

        Debug.Log(mInfo.userCueName.ToString());


        

        

        
    }



    private void OnDrawGizmosSelected()
    {

       
        
    }

}
