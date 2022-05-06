using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabController : MonoBehaviour
{
    public Transform grabDetect;
    public Transform boxHolder;
    public float rayDist;

    bool itemGrabbed = false;
    

    // Update is called once per frame
    void Update()
    {

        RaycastHit grabCheck;
        Physics.Raycast(grabDetect.position, Vector3.down, out grabCheck ,rayDist );
        Debug.Log(grabCheck.collider.tag);
        if (grabCheck.collider != null  )
        {
            if (grabCheck.collider.tag == "Box" && Input.GetKeyDown(KeyCode.Space) && !itemGrabbed)
            {
                itemGrabbed = !itemGrabbed;

            }else if (grabCheck.collider.tag == "Box" && Input.GetKeyDown(KeyCode.Space) && itemGrabbed)
            {
                itemGrabbed = !itemGrabbed;
            }

            
            if (itemGrabbed )
            {
                Debug.Log("Pick up box");
                grabCheck.collider.gameObject.transform.parent = boxHolder;
                grabCheck.collider.gameObject.transform.position = boxHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            }
            else if(!itemGrabbed) 
            {
                Debug.Log("Drop box");
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
