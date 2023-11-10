using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerialPortSimulator : MonoBehaviour
{

    public static string ButtonMessage = "button_message";
    [SerializeField] private SceneController _controller;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("b down");
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("b up");
        }
    }
}
