using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberScattering : MonoBehaviour
{

    public CrowdManager crowdManager;


    // Start is called before the first frame update
    void Start()
    {
        crowdManager = transform.parent.GetComponent<CrowdManager>();

        transform.position = crowdManager.RandomPosInBounds();

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);


    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
