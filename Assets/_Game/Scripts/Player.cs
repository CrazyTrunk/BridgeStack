using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Character
{
    private InputManager inputManager;
    private float wallCheckDistance = 0.3f;

    [Header("SO")]
    [SerializeField] private PlayerDataSO playerDataSO;


    [Header("Slope")]

    [SerializeField] private float slopeForce;
    [SerializeField] private float slopeForceRayLength;

    [Header("Stair Brick")]
    [SerializeField] private LayerMask stairLayer;
    private bool hasExitedDoor = false;
    public override void Start()
    {
        base.Start();
        OnInitPlayer();
        int randomColor = ColorManager.Instance.GetNextColorIndex();
        transform.GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", ColorManager.Instance.colorDataSO.ColorDatas[randomColor].color);
        playerDataSO.PlayerData.playerColor = ColorManager.Instance.colorDataSO.ColorDatas[randomColor].colorName;

    }
    public void OnInitPlayer()
    {
        ChangeAnim("idle");
        inputManager = InputManager.Instance;
        playerDataSO.PlayerData.Zone = 0;
    }
    private void Update()
    {
        if(inputManager.MovementAmount.x != 0 || inputManager.MovementAmount.y != 0)
            MovePlayer();
        else
            ChangeAnim("idle");
    }
    private bool OnSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, characterHeight / 2 * slopeForceRayLength))
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

        if (inputManager.MovementAmount.y <= 0)
            return true;
        RaycastHit hit;
        if (Physics.Raycast(brickPlacer.position + Vector3.up * 2f, Vector3.down, out hit, Mathf.Infinity, stairLayer))
        {
            if (totalBrick == 0)
            {
                RegenerateBrick(playerDataSO.PlayerData.Zone);
            }
            StairBrick brick = hit.collider.gameObject.GetComponent<StairBrick>();
            if ((totalBrick > 0 && brick.colorName == GameColor.NoColor) || (totalBrick > 0 && brick.colorName != playerDataSO.PlayerData.playerColor))
            {
                ColorData currentPlayerColorData = FindColorDataByGameColor(playerDataSO.PlayerData.playerColor);
                brick.brickRenderer.material.color = currentPlayerColorData.color;
                brick.colorName = currentPlayerColorData.colorName;
                brick.color = currentPlayerColorData.color;
                brick.brickRenderer.enabled = true;
                base.RemoveBrick();
                brick.SetColor(currentPlayerColorData.colorName);
                return true;
            }
            if ((brick.colorName == GameColor.NoColor && totalBrick == 0) || (totalBrick == 0 && brick.colorName != playerDataSO.PlayerData.playerColor))
            {
                return false;
            }

        }
        return true;
    }

    private void RegenerateBrick(int zone)
    {
        BrickGenerators[zone].RegenerateBricks(playerDataSO.PlayerData.playerColor);
    }



    private void MovePlayer()
    {

        if (!CheckBridgeStair())
            return;
        moveMovement = Speed * Time.deltaTime * new Vector3(inputManager.MovementAmount.x, 0, inputManager.MovementAmount.y);
        if (IsFacingWall(moveMovement))
        {
            return; 
        }
        if (moveMovement.magnitude > 0)
        {
            Vector3 lookDirection = new Vector3(moveMovement.x, 0, moveMovement.z);
            if (OnSlope())
            {
                moveMovement += Vector3.down * characterHeight / 2 * slopeForce * Time.deltaTime;
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
        if (collision.collider.CompareTag(Tag.GROUND))
        {
            Zone zone = collision.collider.GetComponent<Zone>();
            if (zone != null)
            {
                playerDataSO.PlayerData.Zone = zone.ZoneID;
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
    private bool IsFacingWall(Vector3 direction)
    {
        RaycastHit hit;
        Debug.DrawRay(brickPlacer.position, direction.normalized * wallCheckDistance, Color.red);

        if (Physics.Raycast(brickPlacer.position, direction.normalized, out hit, wallCheckDistance))
        {
            if (hit.collider.CompareTag(Tag.WALL))
            {
                return true;
            }
        }
        return false;
    }
    public ColorData FindColorDataByGameColor(GameColor targetColor)
    {
        foreach (ColorData data in ColorManager.Instance.colorDataSO.ColorDatas)
        {
            if (data.colorName == targetColor)
            {
                return data;
            }
        }
        return null;
    }
}
