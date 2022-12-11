using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    private int[,] currentMap;
    public TilemapGenerator tileMapGenerator;
    private Vector2Int targetPos;
    public float moveRate;
    private float nextMove;
    public float speed;
    [SerializeField] private UI_Inventory uiInventory;
    private Inventory inventory;
    private void Awake()
    {
        player = GameObject.Find("Player");
        currentMap = tileMapGenerator.currentMap;
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        nextMove = Time.time;
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
    }

    void Update()
    {

        bool moving = (Vector2)transform.position != targetPos;

        if (moving)
        {
            MoveTowardsTargetPos();
        }
        else
        {
            NewTargetPos();
        }
    }

    private void MoveTowardsTargetPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void NewTargetPos()
    {

        if (Input.GetKeyDown(KeyCode.W) && Time.time >= nextMove)
        {
            nextMove = Time.time + moveRate;
            Vector2Int destination = targetPos + Vector2Int.up;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) && Time.time >= nextMove)
        {
            nextMove = Time.time + moveRate;
            Vector2Int destination = targetPos + Vector2Int.left;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) && Time.time >= nextMove)
        {
            nextMove = Time.time + moveRate;
            Vector2Int destination = targetPos + Vector2Int.down;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && Time.time >= nextMove)
        {
            nextMove = Time.time + moveRate;
            Vector2Int destination = targetPos + Vector2Int.right;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.right;
            }
        }

    }
}
