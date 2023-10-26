using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] float speed = 5f;
    private InputManager inputManager;
    [SerializeField] private Animator animator;
    private string CurrentAnim;
    [Header("Ground Detetion")]
    [SerializeField] LayerMask groundMask;
    bool isGround;
    float radius = 0.5f;
    private float playerHeight;

    Rigidbody rb;
    Vector3 moveMovement;

    GameColor playerColorName;
    [Header("SO")]
    [SerializeField] public ColorDataSO colorDataSO;

    [Header("Brick Holder and Placer")]
    [SerializeField] public Transform brickHolder;
    [SerializeField] private Transform brickPrefab;
    [SerializeField] private Transform brickPlacer;
    float movementMultiplier = 20f;
    [SerializeField] float airMultiplier = 0.4f;

    float totalBrick = 0;

    [Header("Slope")]

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;


    private void Start()
    {
        ChangeAnim("idle");
        CapsuleCollider collider = transform.GetComponent<CapsuleCollider>();
        inputManager = InputManager.Instance;
        rb = transform.GetComponent<Rigidbody>();
        playerHeight = collider.height;
        RandomColorPlayer();
    }
    private void Update()
    {
        isGround = Physics.CheckSphere(transform.position, radius, groundMask);
        MovePlayer();
    }
    private bool OnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                Debug.Log("Hit");
                return true;
            }
        }
        return false;
    }
    private void MovePlayer()
    {
        moveMovement = speed * Time.deltaTime * new Vector3(inputManager.MovementAmount.x, 0, inputManager.MovementAmount.y);
        if (moveMovement.magnitude > 0)
        {
            Vector3 lookDirection = new Vector3(moveMovement.x, 0, moveMovement.z);

            if (OnSlope())
            {
                moveMovement += Vector3.down * playerHeight / 2 * slopeForce * Time.deltaTime;

            }
            transform.position += moveMovement;
            transform.forward = lookDirection.normalized;
            ChangeAnim("run");
        }
        else
        {
            ChangeAnim("idle");
        }
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
    private void OnTriggerEnter(Collider other)
    {
        Brick brick = other.transform.GetComponent<Brick>();

        if (other.CompareTag(Tag.Brick))
        {
            if (brick.colorName == playerColorName)
            {
                Destroy(other.gameObject);
                UpdatePlayerBrick(brick.color);
            }
        }
    }
    private void UpdatePlayerBrick(Color brickcolor)
    {
        Transform brick = Instantiate(brickPrefab, brickHolder);
        Vector3 brickPosition = new Vector3(0, 0 + (totalBrick * 0.2f), 0);
        brick.localPosition = brickPosition;
        brick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", brickcolor);
        totalBrick++;

    }
    private void RandomColorPlayer()
    {
        int randomColor = Random.Range(0, colorDataSO.ColorDatas.Count);
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", colorDataSO.ColorDatas[randomColor].color);
        playerColorName = colorDataSO.ColorDatas[randomColor].colorName;

    }
}
