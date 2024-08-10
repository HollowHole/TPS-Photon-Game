using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform playerTransform;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraFollowSpeed = 0.3f;
    public InputManager inputManager;

    Transform cameraPivotTransform;

    private float cameraVerticalInput;
    private float cameraHorizontalInput;
    public float cameraMoveSpeed;
    private float cameraLookAngle = 0;
    private float cameraPivotAngle = 0;
    private void Awake()
    {
        
        cameraPivotTransform = transform.GetChild(0);
    }
    public void HandleAllCameraMovement()
    {
        ReadInput();
        FollowTarget();
        HandleRotate();
    }
    private void ReadInput()
    {
        if (inputManager != null)
        {
            cameraHorizontalInput = inputManager.cameraMovementInput.x;
            cameraVerticalInput = inputManager.cameraMovementInput.y;
        }

    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, playerTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }
    private void HandleRotate()
    {
        cameraLookAngle += cameraHorizontalInput * cameraMoveSpeed;

        cameraPivotAngle += cameraVerticalInput * cameraMoveSpeed;
        cameraPivotAngle = Mathf.Clamp(cameraPivotAngle, -90f, 90f);
        Quaternion targetRotation = Quaternion.Euler(0, cameraLookAngle, 0);
        transform.rotation = targetRotation;
        targetRotation = Quaternion.Euler(-cameraPivotAngle, 0, 0);
        cameraPivotTransform.localRotation = targetRotation;
    }
}
