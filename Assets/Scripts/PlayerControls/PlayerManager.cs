using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    
    InputManager inputManager;
    PhotonView photonView;
    CameraManager cameraManager;

    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        inputManager = GetComponent<InputManager>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
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
        playerShooting.HandleAllGunActions();
    }
    private void LateUpdate()
    {
        if(!photonView.IsMine) { return; }
        cameraManager.HandleAllCameraMovement();
    }



}
