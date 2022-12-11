using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    private int[,] currentMap;
    public TilemapGenerator tileMapGenerator;
    private Vector2Int targetPos;
    public float speed;
    public List<Item> inventory;
    private void Awake()
    {
        player = GameObject.Find("Player");
        currentMap = tileMapGenerator.currentMap;
    }
    // Start is called before the first frame update
    void Start()
    {
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
    }

    void FixedUpdate()
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
        if (Input.GetKey(KeyCode.W))
        {
            Vector2Int destination = targetPos + Vector2Int.up;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.up;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Vector2Int destination = targetPos + Vector2Int.left;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.left;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Vector2Int destination = targetPos + Vector2Int.down;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.down;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Vector2Int destination = targetPos + Vector2Int.right;
            int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
            if (destinationTile == 0)
            {
                targetPos += Vector2Int.right;
            }
        }
    }
}
