using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to save the initial position of any item
public class boxVariables : MonoBehaviour
{
    [SerializeField]
    public GameObject box;
    public Vector3 initPosition;
   
    void Start()
    {
        initPosition = box.transform.position;
    }

}
