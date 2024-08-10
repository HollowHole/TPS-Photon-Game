using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    private PlayerMovement playerMovement;
    InputManager inputManager;
    PhotonView photonView;
    CameraManager cameraManager;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
        if (!photonView.IsMine) return;
        cameraManager.playerTransform = transform;
        cameraManager.inputManager = inputManager;
    }
    private void Start()
    {

    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        playerMovement.HandleAddMovements();
    }
    private void LateUpdate()
    {
        if(!photonView.IsMine) { return; }
        cameraManager.HandleAllCameraMovement();
    }



}
