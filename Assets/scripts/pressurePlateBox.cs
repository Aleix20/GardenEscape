using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlateBox : MonoBehaviour
{
    [SerializeField]
    public GameObject door;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
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
        string a = collider.tag;
        if (collider.tag.Equals("Box") )
        {
            // Player entered collider!
            door.SetActive(false);
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
