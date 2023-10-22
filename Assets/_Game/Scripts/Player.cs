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
    private void Start()
    {
        ChangeAnim("idle");
    }
    private void Update()
    {
        Vector3 scaledMovement = speed * Time.deltaTime * new Vector3(inputManager.MovementAmount.x, 0, inputManager.MovementAmount.y);
        if (scaledMovement.magnitude > 0)
        {
            MovePlayer(scaledMovement);
        }
        else
        {
            ChangeAnim("idle");
        }

    }

    private void MovePlayer(Vector3 scaledMovement)
    {
        transform.position += scaledMovement;
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
}
