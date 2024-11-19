using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 5.0f;

    private Vector3 offset;
    private float yaw = 0f;  //horizontal rotation
    private float pitch = 0f;  //vertical rotation

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            yaw += mouseX;
            pitch -= mouseY;

            pitch = Mathf.Clamp(pitch, -45f, 45f);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 newPosition = player.transform.position + rotation * offset;

        transform.position = newPosition;
        transform.LookAt(player.transform.position);
    }
}

