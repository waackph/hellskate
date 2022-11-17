using UnityEngine;

/// <summary>Class <c>CameraController</c> handles the camera movement at each update step
/// by translating the camera Transform by a given speed parameter 
/// times the deltaTime per fixedupdate step.</summary>
///

public class CameraController : MonoBehaviour
{
    [SerializeField] float cameraSpeed;

    void FixedUpdate()
    {
        // Vector3 currentPosition = GetComponent<Transform>().position;
        // GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x + cameraSpeed * Time.deltaTime, currentPosition.y, currentPosition.z);
        GetComponent<Transform>().Translate(new Vector3(cameraSpeed, 0, 0) * Time.deltaTime);
        // GetComponent<Transform>().position = new Vector3(GetComponent<Transform>().position.x, 0, GetComponent<Transform>().position.z);
    }
}
