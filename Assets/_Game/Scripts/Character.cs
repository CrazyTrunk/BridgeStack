using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] private Animator animator;
    private string CurrentAnim;
    [Header("Ground Detetion")]
    [SerializeField] LayerMask groundMask;
    //bool isGround;
    //float radius = 0.5f;
    public float characterHeight;

    public Vector3 moveMovement;
    [Header("SO")]
    protected float totalBrick = 0;

    [Header("Brick Holder and Placer")]
    [SerializeField] public Transform brickHolder;
    [SerializeField] public Transform brickPrefab;
    [SerializeField] public Transform brickPlacer;

    public float Speed { get => speed; set => speed = value; }
    public BrickGenerator[] BrickGenerators;

    public virtual void Start()
    {
        ChangeAnim("idle");
        CapsuleCollider collider = transform.GetComponent<CapsuleCollider>();
        characterHeight = collider.height;
        BrickGenerators = FindObjectsOfType<BrickGenerator>().OrderBy(generator => generator.name).ToArray();

    }
    public void ChangeAnim(string animName)
    {
        if (CurrentAnim != animName)
        {
            animator.ResetTrigger(animName);
            CurrentAnim = animName;
            animator.SetTrigger(CurrentAnim);
        }
    }
    public void RemovePlayerBrick()
    {
        totalBrick--;
        Destroy(brickHolder.GetChild(brickHolder.childCount - 1).gameObject);
    }
}
