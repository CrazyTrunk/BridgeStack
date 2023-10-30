using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] public ColorDataSO colorDataSO;
    protected float totalBrick = 0;

    [Header("Brick Holder and Placer")]
    [SerializeField] public Transform brickHolder;
    [SerializeField] public Transform brickPrefab;
    [SerializeField] public Transform brickPlacer;

    public float Speed { get => speed; set => speed = value; }

    public virtual void OnInit()
    {
        ChangeAnim("idle");
        CapsuleCollider collider = transform.GetComponent<CapsuleCollider>();
        characterHeight = collider.height;
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
    protected int RandomColorCharacter()
    {
        int randomColor = Random.Range(0, colorDataSO.ColorDatas.Count);
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", colorDataSO.ColorDatas[randomColor].color);
        // More logic for color if needed
        return randomColor;
    }
    public void RemovePlayerBrick()
    {
        totalBrick--;
        Destroy(brickHolder.GetChild(brickHolder.childCount - 1).gameObject);
    }
}
