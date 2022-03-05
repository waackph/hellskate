using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void FixedUpdate()
    {
        // Vector3 currentPosition = GetComponent<Transform>().position;
        // GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x + cameraSpeed * Time.deltaTime, currentPosition.y, currentPosition.z);
        GetComponent<Transform>().Translate(new Vector3(cameraSpeed, 0, 0) * Time.deltaTime);
    }
}
