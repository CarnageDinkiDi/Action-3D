using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lookSensitivity;
    public float lookSmoothness;

    private Transform player;
    private Vector2 smoothedVelocity;
    private Vector2 lookingDirection;

    public float cameraClamp;
    private void Start()
    {
        player = transform.root;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 cameraRotationInput = new Vector2(Input.GetAxisRaw("CameraX"), -Input.GetAxisRaw("CameraY"));
        cameraRotationInput = Vector2.Scale(cameraRotationInput, new Vector2(lookSensitivity*lookSmoothness,lookSensitivity*lookSmoothness));
        smoothedVelocity = Vector2.Lerp(smoothedVelocity, cameraRotationInput, 1 / lookSmoothness);
        lookingDirection += smoothedVelocity;

        lookingDirection.y = Mathf.Clamp(lookingDirection.y, -cameraClamp, cameraClamp);
        transform.localRotation = Quaternion.AngleAxis(-lookingDirection.y, Vector3.right);
        player.localRotation = Quaternion.AngleAxis(lookingDirection.x, player.transform.up);
    }
}
