using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called before the first frame update
    PhotonView photonView;
    PlayerControls playerControls;

    public Vector2 movementInput;
    public Vector2 cameraMovementInput;
    public bool SprintKeyState;
    public bool JumpKeyState;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnEnable()
    {
        if (playerControls == null) 
        { 
            playerControls=new PlayerControls ();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.CameraMovement.performed += i => cameraMovementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Sprint.started += i=>SprintKeyState = true;
            playerControls.PlayerMovement.Sprint.canceled += i => SprintKeyState = false;
            playerControls.PlayerMovement.Jump.started += i => JumpKeyState = true;
            playerControls.PlayerMovement.Jump.canceled += i => JumpKeyState = false;

            playerControls.PlayerMovement.Jump.started += i => Debug.Log(photonView.IsMine + "respond");
        }
        playerControls.Enable();

    }


    private void OnDisable()
    {
        playerControls.Disable();
    }


}
