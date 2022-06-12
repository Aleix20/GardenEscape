using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class pressurePlateBox : MonoBehaviour
{
    [SerializeField]
    public GameObject door;
    public float timer;
    public int indexLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        //Time to open the door
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                door.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Open the door if detects a Box on top
        if (collider.tag.Equals("Box") )
        {
            // Player entered collider!
            door.SetActive(false);
            SoundManager.Instance.PlayDoorOpenClip();
            if (this.tag.Equals("nextLevel"))
            {
                SceneManager.LoadScene(indexLevel);
            }
        }
   
        
      
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag.Equals("Box"))
        {
            // Player still on top of collider!
            timer = 0.2f;
        }
    }
}
