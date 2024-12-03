using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMemberZoomies : MonoBehaviour
{
    public float minZ = 0;
    public float maxZ = 10;

    public float speed = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float nextZ = transform.position.z + speed * Time.deltaTime;

        if (nextZ < minZ)
        {
            nextZ = maxZ + (minZ-nextZ);
        }

        transform.position = new Vector3( transform.position.x, transform.position.y, nextZ);


    }
}
