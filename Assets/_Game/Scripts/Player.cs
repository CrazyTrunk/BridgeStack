using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    [SerializeField] float speed = 5f;
    private InputManager inputManager;
    [SerializeField] private Animator animator;
    private string CurrentAnim;
    [Header("Ground Detetion")]
    [SerializeField] LayerMask groundMask;
    //bool isGround;
    //float radius = 0.5f;
    public float playerHeight;

    Vector3 moveMovement;

    [Header("SO")]
    [SerializeField] private ColorDataSO colorDataSO;
    [SerializeField] private PlayerDataSO playerDataSO;

    [Header("Brick Holder and Placer")]
    [SerializeField] public Transform brickHolder;
    [SerializeField] private Transform brickPrefab;
    [SerializeField] private Transform brickPlacer;

    float totalBrick = 0;
    [Header("Slope")]

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [Header("Stair Brick")]
    [SerializeField] private LayerMask stairLayer;
    [SerializeField] BrickGenerator[] BrickGenerators;

    private bool hasExitedDoor = false;

    private void Start()
    {
        OnInit();
        RandomColorPlayer();
    }
    public void OnInit()
    {
        ChangeAnim("idle");
        CapsuleCollider collider = transform.GetComponent<CapsuleCollider>();
        inputManager = InputManager.Instance;
        playerHeight = collider.height;
        BrickGenerators = FindObjectsOfType<BrickGenerator>().OrderBy(generator => generator.name).ToArray();
        playerDataSO.PlayerData.Zone = 0;
        BrickGenerators[playerDataSO.PlayerData.Zone].SpawnBricks();
    }
    private void Update()
    {
        //isGround = Physics.CheckSphere(transform.position, radius, groundMask);
        MovePlayer();
    }
    private bool OnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight / 2 * slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckBridgeStair()
    {
        if (totalBrick == 0)
        {
            RegenerateBrick(playerDataSO.PlayerData.Zone);
        }
        if (inputManager.MovementAmount.y <= 0)
            return true;
        RaycastHit hit;
        if (Physics.Raycast(brickPlacer.position + Vector3.up * 2f, Vector3.down, out hit, Mathf.Infinity, stairLayer))
        {
            StairBrick brick = hit.collider.gameObject.GetComponent<StairBrick>();
            if (totalBrick > 0 && brick.colorName == GameColor.NoColor)
            {
                ColorData currentPlayerColorData = FindColorDataByGameColor(playerDataSO.PlayerData.playerColor);
                brick.brickRenderer.material.color = currentPlayerColorData.color;
                brick.colorName = currentPlayerColorData.colorName;
                brick.color = currentPlayerColorData.color;
                brick.brickRenderer.enabled = true;
                brick.pathway.BrickPlaced++;
                RemovePlayerBrick();
                if (brick.pathway.BrickPlaced == brick.pathway.TotalStair)
                {
                    brick.pathway.OpenDoor();
                }
                return true;
            }
            if (brick.colorName == GameColor.NoColor && totalBrick == 0)
            {
                return false;
            }

        }
        return true;
    }

    private void RegenerateBrick(int zone)
    {
        BrickGenerators[zone].RegenerateBricks();
    }

    private void RemovePlayerBrick()
    {
        totalBrick--;
        Destroy(brickHolder.GetChild(brickHolder.childCount - 1).gameObject);
    }

    private void MovePlayer()
    {
        if (!CheckBridgeStair())
            return;

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

        if (other.CompareTag(Tag.BRICK))
        {
            if (brick.colorName == playerDataSO.PlayerData.playerColor)
            {
                Destroy(other.gameObject);
                BrickGenerators[playerDataSO.PlayerData.Zone].MakeRemovedBrick(brick.brickNumber);
                UpdatePlayerBrick(brick.color);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tag.DOOR) && !hasExitedDoor)
        {
            hasExitedDoor = true;

            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            BrickGenerators[playerDataSO.PlayerData.Zone].SpawnBricks();
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tag.GROUND) )
        {
            if (collision.collider.name == "Zone1")
            {
                playerDataSO.PlayerData.Zone = 0;
                Debug.Log($"Zone 1 {playerDataSO.PlayerData.Zone}");
            }
            else if (collision.collider.name == "Zone2")
            {
                playerDataSO.PlayerData.Zone = 1;
                Debug.Log($"Zone 2 {playerDataSO.PlayerData.Zone}");
            }
        }
    }
    private void UpdatePlayerBrick(Color brickcolor)
    {
        Transform brick = Instantiate(brickPrefab, brickHolder);
        Vector3 brickPosition = new Vector3(0, 0 + (totalBrick * 0.2f), 0);
        brick.localPosition = brickPosition;
        playerDataSO.PlayerData.totalBrickCollected++;
        brick.GetChild(0).GetComponent<MeshRenderer>().material.SetColor("_Color", brickcolor);
        totalBrick++;
    }
    private void RandomColorPlayer()
    {
        int randomColor = Random.Range(0, colorDataSO.ColorDatas.Count);
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", colorDataSO.ColorDatas[randomColor].color);
        playerDataSO.PlayerData.playerColor = colorDataSO.ColorDatas[randomColor].colorName;
    }

    public ColorData FindColorDataByGameColor(GameColor targetColor)
    {
        foreach (ColorData data in colorDataSO.ColorDatas)
        {
            if (data.colorName == targetColor)
            {
                return data;
            }
        }
        return null;
    }
}
