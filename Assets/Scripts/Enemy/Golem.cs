using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.InitEnemy();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play golem voice
    }

    public override void MoveEnemy()
    {
        bool moving = (Vector2)transform.position != targetPos;

        if (moving)
        {
            MoveTowardsTargetPos();
        }
        else
        {
            RandomMovePos();
        }
    }

    private void MoveTowardsTargetPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void RandomMovePos()
    {
        int random = Random.Range(0, 4);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (random == 0)
            {
                Vector2Int destination = targetPos + Vector2Int.up;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0)
                {
                    targetPos += Vector2Int.up;
                }
            }
            else if (random == 1)
            {
                Vector2Int destination = targetPos + Vector2Int.left;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0)
                {
                    targetPos += Vector2Int.left;
                }
            }
            else if (random == 2)
            {
                Vector2Int destination = targetPos + Vector2Int.down;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0)
                {
                    targetPos += Vector2Int.down;
                }
            }
            else if (random == 3)
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
