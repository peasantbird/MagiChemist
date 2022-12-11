using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Type // your custom enumeration
    {
        Earth,
        Living,
        Nonliving
    };
    public int enemyIndex;
    public List<int> drops;
    public Type enemyType = new Type();
    public float speed;
    protected TilemapGenerator tileMapGenerator;
    protected Vector2Int targetPos;


    public void InitEnemy() {
        tileMapGenerator = GameObject.Find("Player").GetComponent<TilemapGenerator>();
    }

    public void MoveEnemy() {
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
        int random = Random.Range(0, 3);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            if (random==0)
            {
                Vector2Int destination = targetPos + Vector2Int.up;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0)
                {
                    targetPos += Vector2Int.up;
                }
            }
            else if (random==1)
            {
                Vector2Int destination = targetPos + Vector2Int.left;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0)
                {
                    targetPos += Vector2Int.left;
                }
            }
            else if (random==2)
            {
                Vector2Int destination = targetPos + Vector2Int.down;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0)
                {
                    targetPos += Vector2Int.down;
                }
            }
            else if (random==3)
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

    
}