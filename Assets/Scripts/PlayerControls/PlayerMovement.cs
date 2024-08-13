using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Photon.Pun;
using System.Net;

public class PlayerMovement : MonoBehaviour
{

    
    InputManager inputManager;
    Rigidbody rb;
    CapsuleCollider capsule;
    Animator animator;
    //move
    private float verticalInput;
    private float horizontalInput;
    public int moveSpeed;
    //sprint

    [Header("Jump")]
    private bool isJump;
    public float jumpForce;
    private float curJumpForce;
    [Header("Fall")]
    public LayerMask GroundLayers;

    public Vector3 Gravity;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float fallTimeout = 0.15f;
    private float fallTimer;
    private bool isFallTimeout;

    private bool Grounded;

    //camera parameters
    
    Transform cameraManagerTransform;
    Transform cameraPivotTransform;
    Transform cameraTransform;
    private void Awake()
    {
        
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        cameraManagerTransform = FindObjectOfType<CameraManager>().transform;
        cameraPivotTransform = cameraManagerTransform.GetChild(0);
        cameraTransform = cameraPivotTransform.GetChild(0);
        
        
    }
    public void HandleAddMovements()
    {
        
        ReadInput();
        
        GroundedCheck();

        Move();

        JumpAndGravity();

        UpdateAnimation();

    }

    private void JumpAndGravity()
    {

        if (!Grounded)
        {
            fallTimer += Time.deltaTime;
            if (fallTimer > fallTimeout)
                isFallTimeout = true;
            rb.velocity += fallTimer * Gravity;
        }
        else
        {
            fallTimer = 0;
            isFallTimeout= false;
            if (isJump)
            {

            }
        }
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
        
    }

    

    private void ReadInput()
    {
        verticalInput = inputManager.movementInput.y / 2;
        horizontalInput = inputManager.movementInput.x / 2;
        
        if(inputManager.SprintKeyState && verticalInput > 0)
        {
            verticalInput *= 2;
        }
        
        isJump = inputManager.JumpKeyState;
    }
    
    private void Move()
    {
        if(rb.velocity.magnitude>0.1f)
            transform.rotation = cameraManagerTransform.rotation;
        rb.velocity = verticalInput * cameraManagerTransform.forward + horizontalInput * cameraManagerTransform.right;
        rb.velocity *= moveSpeed;

        
    }
    public void UpdateAnimation()
    {
        
        animator.SetFloat("VerticalInput", verticalInput);
        animator.SetFloat("HorizontalInput", horizontalInput);
        animator.SetFloat("Speed", inputManager.movementInput.magnitude);
        animator.SetBool("Grounded", Grounded);
        animator.SetBool("IsFallTimeout", isFallTimeout);
        animator.SetBool("Jump", isJump);
    }
}
