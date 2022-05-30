using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireWall : MonoBehaviour
{
    public float targetScale; // 1
    public float timeToReachTarget; // 2
    private float startScale;  // 3
    private float percentScaled = 0; // 4
    [SerializeField]
    public GameObject fireWallObject;
    public GameObject waterSplash;
    // Start is called before the first frame update
    void Start()
    {
        startScale = 0;
        targetScale = 20; 
    }


    private void OnTriggerEnter(Collider collider)
    {
        string a = collider.tag;
        if (collider.tag.Equals("Bucket"))
        {
            // Player entered collider!
            Destroy(fireWallObject);
            waterSplash.SetActive(true);
            waterSplash.transform.position = fireWallObject.transform.position;
            //while (percentScaled < 1f) // 1
            //{
            //    percentScaled += Time.deltaTime / timeToReachTarget; // 2
            //    float scale = Mathf.Lerp(startScale, targetScale, percentScaled); // 3
            //    waterSplash.transform.localScale = new Vector3(scale, scale, 1); // 4
            //    new WaitForSeconds(percentScaled);
            //}
        }
    }

  
}
