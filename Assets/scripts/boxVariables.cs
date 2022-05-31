using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxVariables : MonoBehaviour
{
    [SerializeField]
    public GameObject box;
    // Start is called before the first frame update
    public Vector3 initPosition;
   
    void Start()
    {
        initPosition = box.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
