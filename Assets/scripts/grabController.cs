using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabController : MonoBehaviour
{
    public Transform grabDetect;
    public Transform boxHolder;
    public float rayDist;

    private Vector3  lastPosition;
    private float waitTime;

    private float fixTime = 3.0f;

    bool itemGrabbed = false;
    

    // Update is called once per frame
    void Update()
    {

        RaycastHit grabCheck;
        Physics.Raycast(grabDetect.position, Vector3.down, out grabCheck ,rayDist );
        Debug.Log("WaitTime is: " + waitTime);
        Debug.Log(grabCheck.collider.tag);
        if (grabCheck.collider != null  )
        {
            if (grabCheck.collider.tag == "Box" && waitTime >= fixTime && !itemGrabbed)
            {
                Debug.Log("Pick up box");
                itemGrabbed = !itemGrabbed;
                waitTime = 0.0f;

            }
            else if (grabCheck.collider.tag == "Box" && waitTime >= fixTime && itemGrabbed)
            {
                Debug.Log("Drop box");
                itemGrabbed = !itemGrabbed;
                waitTime = 0.0f;
            }

            
            if (itemGrabbed )
            {
                grabCheck.collider.gameObject.transform.parent = boxHolder;
                grabCheck.collider.gameObject.transform.position = boxHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            }
            else if(!itemGrabbed) 
            {
                
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
    void LateUpdate()
    {

        float distToLastPos = Vector3.Distance(grabDetect.position, lastPosition);

        if (distToLastPos < 0.3f)
        {
            waitTime += Time.deltaTime;
        }
        else
        {
            waitTime = 0.0f;
            lastPosition = grabDetect.position;
        }

    }
}
