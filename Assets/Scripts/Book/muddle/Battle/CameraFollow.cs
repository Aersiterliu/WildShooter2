using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    new public Camera camera;

    private void Start()
    {
        camera=Camera.main;
    }

    private void LateUpdate()
    {

        camera.transform.position = new Vector3(this.transform.position.x, this.transform.position. y, camera.transform.position.z);
    }
}
