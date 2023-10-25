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
    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 1f;

    Rigidbody rb;
    RaycastHit slopeHit;
    Vector3 slopeMovement;
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
        moveMovement = speed * Time.deltaTime * new Vector3(inputManager.MovementAmount.x, 0, inputManager.MovementAmount.y);
        ControlDrag();
        if (moveMovement.magnitude > 0)
        {

            MovePlayer(moveMovement);
        }
        else
        {
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
            Debug.Log("not on slope");

            rb.AddForce(moveMovement.normalized  *movementMultiplier, ForceMode.Acceleration);

        }
        else if (isGround && OnSlope())
        {
            Debug.Log("OnSLope");
            rb.AddForce(slopeMovement.normalized  * movementMultiplier, ForceMode.Acceleration);
        }
        else if (!isGround)
        {
            rb.AddForce(moveMovement.normalized  *movementMultiplier * airMultiplier, ForceMode.Acceleration);
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
    private void CheckBridge()
    {
        RaycastHit hit;
        if (Physics.Raycast(brickPlacer.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Debug.Log("Hit");
        }
    }
}
