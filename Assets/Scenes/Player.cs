using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float xInput;
    void Start()
    {
        
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("Jump!");
        }
        
        xInput = Input.GetAxis("Horizontal");

    }
}
