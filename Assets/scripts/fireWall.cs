using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireWall : MonoBehaviour
{


    [SerializeField]
    public GameObject fireWallObject;
    public GameObject waterSplash;




    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Bucket"))
        {
            // Player entered collider!
            //Destroy fire object
            Destroy(fireWallObject);
            SoundManager.Instance.PlayFireExtinguishClip();
            //Show water splash after remove the fire
            waterSplash.SetActive(true);
            waterSplash.transform.position = fireWallObject.transform.position;

        }
    }

  
}
