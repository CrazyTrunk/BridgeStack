using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    float speed = 5f;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Animator animator;
    private string CurrentAnim;
    [Header("Ground Detetion")]
    [SerializeField] LayerMask groundMask;
    bool isGround;
    float radius = 0.5f;
    private float playerHeight;
    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 1f;

    Rigidbody rb;
    RaycastHit slopeHit;
    Vector3 slopeMovement;
    Vector3 moveMovement;

    private void Start()
    {
        ChangeAnim("idle");
        CapsuleCollider collider = transform.GetComponent<CapsuleCollider>();
        inputManager = InputManager.Instance;
        rb = transform.GetComponent<Rigidbody>();
        playerHeight = collider.height;
    }
    private void Update()

    {
        isGround = Physics.CheckSphere(transform.position, radius, groundMask);
        moveMovement = speed * Time.deltaTime * new Vector3(inputManager.MovementAmount.x, 0, inputManager.MovementAmount.y);
        ControlDrag();
        if (moveMovement.magnitude > 0)
        {

            MovePlayer(moveMovement);
        }
        else
        {
            if (OnSlope())
            {
                rb.AddForce(-slopeHit.normal * Mathf.Abs(Physics.gravity.y) * rb.mass);
            }
            ChangeAnim("idle");
        }
        slopeMovement = Vector3.ProjectOnPlane(moveMovement, slopeHit.normal);

    }
    void ControlDrag()
    {
        if (isGround)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
    private void MovePlayer(Vector3 scaledMovement)
    {
        if (isGround && !OnSlope())
        {
            rb.AddForce(moveMovement.normalized * 10f, ForceMode.Acceleration);

        }
        else if (isGround && OnSlope())
        {
            rb.AddForce(slopeMovement.normalized * 10f, ForceMode.Acceleration);

        }
        else if (!isGround)
        {
            rb.AddForce(moveMovement.normalized * 10f, ForceMode.Acceleration);
        }
        transform.transform.LookAt(transform.position + scaledMovement, Vector3.up);
        ChangeAnim("run");

    }
    protected void ChangeAnim(string animName)
    {
        if (CurrentAnim != animName)
        {
            animator.ResetTrigger(animName);
            CurrentAnim = animName;
            animator.SetTrigger(CurrentAnim);
        }
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
