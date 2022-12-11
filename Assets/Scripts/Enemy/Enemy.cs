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
    public int hp;
    public List<int> drops;
    public Type enemyType = new Type();
    public float speed;
    protected TilemapGenerator tileMapGenerator;
    protected Vector2Int targetPos;
    protected int[,] currentMap;
    protected GameObject player;


    public void InitEnemy()
    {
        player = GameObject.Find("Player");
        tileMapGenerator = GameObject.Find("Player").GetComponent<TilemapGenerator>();
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
        currentMap = tileMapGenerator.currentMap;
    }

    public void MoveEnemy()
    {
        bool moving = (Vector2)transform.position != targetPos;
        bool keyPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);

        if (moving)
        {
            MoveTowardsTargetPos();
        }
        else if (keyPressed)
        {
            if (PlayerIsAround())
            {
                SimpleChasePlayer();
            } else 
            {
                RandomMovePos();
            }
        }
    }

    private void MoveTowardsTargetPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void ChasePlayer()
    {
        //Debug.Log("Enemy has spotted player and is giving chase.");
        //If time use a refined algorithm
    }

    private void SimpleChasePlayer()
    {   
        Vector3 playerPos = player.transform.position;
        float currentX = transform.position.x;
        float currentY = transform.position.y;

        int xMovement = 0;
        int yMovement = 0;
        int loseInterestChance = (int)Random.Range(1, 11);
        
        if (loseInterestChance < 9)
        {
            if (playerPos.x < currentX)
            { //player is at the left
                if (Movable(targetPos.x - 1, targetPos.y))
                {
                    xMovement = -1;
                }
            }
            else if (playerPos.x > currentX)
            { //player is at the right
                if (Movable(targetPos.x + 1, targetPos.y))
                {
                    xMovement = 1;
                }
            }


            if (playerPos.y < currentY)
            { //player is at the bottom
                if (Movable(targetPos.x, targetPos.y - 1))
                {
                    yMovement = -1;
                }
            }
            else if (playerPos.y > currentY)
            { //player is at the top

                if (Movable(targetPos.x, targetPos.y + 1))
                {
                    yMovement = 1;
                }
            }


            if (xMovement != 0 && yMovement != 0)
            { //prevent diagonal movement
                int randomNum = Random.Range(0, 1);
                if (randomNum == 0)
                {
                    xMovement = 0;
                }
                else
                {
                yMovement = 0;
                }
            }

            Vector2Int destination = targetPos + new Vector2Int(xMovement, yMovement);
            if (Movable(destination.x, destination.y))
            {
                targetPos += new Vector2Int(xMovement, yMovement);
            }
            else 
            {
                RandomMovePos();
            }
        }
        else 
        {
            RandomMovePos();
        }
    }

    private bool Movable(int xDestination, int yDestination)
    {
        int destinationTile = tileMapGenerator.checkTileAtCoordinates(xDestination, -yDestination);
        return destinationTile == 0;
    }

    private bool CheckMapLimit(int xDestination, int yDestination)
    {
        if (xDestination < 0|| xDestination > tileMapGenerator.mapSizeX - 1 || yDestination > 0|| yDestination < -(tileMapGenerator.mapSizeY - 1) )
        {
            return false;
        } else {
            return true;
        }
    }
    private void RandomMovePos()
    {

        int random = Random.Range(0, 4);

        if (random == 0)
        {
            Vector2Int destination = targetPos + Vector2Int.up;
            if (Movable(destination.x, destination.y))
            {
                targetPos += Vector2Int.up;
            }
            else
            {
                RandomMovePos();
            }

        }
        else if (random == 1)
        {
            Vector2Int destination = targetPos + Vector2Int.left;
            if (Movable(destination.x, destination.y))
            {
                targetPos += Vector2Int.left;
            }
            else
            {
                RandomMovePos();
            }

        }
        else if (random == 2)
        {
            Vector2Int destination = targetPos + Vector2Int.down;
            if (Movable(destination.x, destination.y))
            {
                targetPos += Vector2Int.down;
            }
            else
            {
                RandomMovePos();
            }

        }
        else if (random == 3)
        {
            Vector2Int destination = targetPos + Vector2Int.right;
            if (Movable(destination.x, destination.y))
            {
                targetPos += Vector2Int.right;
            }
            else
            {

                RandomMovePos();
            }

        }


    }


    public virtual void PlayVoice()
    {
        //virtual method
    }

    public bool PlayerIsAround()
    {
        //to check if player is around a certain distance
        Vector2Int playerPos = new Vector2Int ((int)player.transform.position.x, (int)player.transform.position.y);
        Vector2Int enemyPos = new Vector2Int ((int)transform.position.x, (int)transform.position.y);
        //todo: write an algorithm to check the player is within a certain distance
        //get magnitude (distance) between playerPos and enemyPos
        if ((enemyPos-playerPos).magnitude < 8)
        {
            if (CanEnemySeePlayer() == true) //Is the monster's sight obstructed by a wall tile?
            {
                return true;
            } else 
            {
                return false;
            }
        } else {
            return false;
        }
    }

    public bool CanEnemySeePlayer()
    {
        Vector2Int playerPos = new Vector2Int ((int)player.transform.position.x, (int)player.transform.position.y);
        Vector2Int enemyPos = new Vector2Int ((int)transform.position.x, (int)transform.position.y);
        Vector2Int differencePos = playerPos-enemyPos;
        int distanceBetween = (int)((enemyPos-playerPos).magnitude);
        Vector2Int checkUp = enemyPos + Vector2Int.up*distanceBetween;
        Vector2Int checkDown = enemyPos + Vector2Int.down*distanceBetween;
        Vector2Int checkLeft = enemyPos + Vector2Int.left*distanceBetween;
        Vector2Int checkRight = enemyPos + Vector2Int.right*distanceBetween;
        if (playerPos == checkUp)
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int checkUp2 = enemyPos + Vector2Int.up*i;
                if (tileMapGenerator.checkTileAtCoordinates(checkUp2.x, -checkUp2.y)==1) 
                {
                    return false;
                } else if (i == 8)
                {
                    return true;
                }
            }
        } else if (playerPos == checkDown)
        {
            for (int i = 1; i < 8; i++)
                {
                    Vector2Int checkDown2 = enemyPos + Vector2Int.up*i;
                    if (tileMapGenerator.checkTileAtCoordinates(checkDown2.x, -checkDown2.y)==1)
                    {
                        return false;
                    } else if (i == 8)
                    {
                        return true;
                    }
                }
        } else if (playerPos == checkRight) {
            for (int i = 1; i < 8; i++)
                {
                    Vector2Int checkRight2 = enemyPos + Vector2Int.right*i;
                    if (tileMapGenerator.checkTileAtCoordinates(checkRight2.x, -checkRight2.y)==1)
                    {
                        return false;
                    } else if (i == 8)
                    {
                        return true;
                    }
                }
        } else if (playerPos == checkLeft)
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int checkLeft2 = enemyPos + Vector2Int.left*i;
                if (tileMapGenerator.checkTileAtCoordinates(checkLeft2.x, -checkLeft2.y)==1)
                {
                    return false;
                } else if (i == 8)
                {
                    return true;
                }
            }
        } 
        return true;
    }

    private void ChaseThroughWalls()
    {   
        Vector3 playerPos = player.transform.position;
        float currentX = transform.position.x;
        float currentY = transform.position.y;

        int xMovement = 0;
        int yMovement = 0;
        int loseInterestChance = (int)Random.Range(1, 11);
        
        if (loseInterestChance < 8)
        {
            if (playerPos.x < currentX)
            { //player is at the left
                xMovement = -1;
            }
            else if (playerPos.x > currentX)
            { //player is at the right
                xMovement = 1;
            }


            if (playerPos.y < currentY)
            { //player is at the bottom
                yMovement = -1;
            }
            else if (playerPos.y > currentY)
            { //player is at the top
                yMovement = 1;
            }


            if (xMovement != 0 && yMovement != 0)
            { //prevent diagonal movement
                int randomNum = Random.Range(0, 1);
                if (randomNum == 0)
                {
                    xMovement = 0;
                }
                else
                {
                yMovement = 0;
                }
            }

            Vector2Int destination = targetPos + new Vector2Int(xMovement, yMovement);
            if (CheckMapLimit(destination.x, destination.y))
            {
                targetPos += new Vector2Int(xMovement, yMovement);
            }
            else 
            {
                RandomMoveThroughWallsPos();
            }
        }
        else 
        {
            RandomMoveThroughWallsPos();
        }
    }

    public void MoveEnemyThroughWalls()
    {
        bool moving = (Vector2)transform.position != targetPos;
        bool keyPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D);

        if (moving)
        {
            MoveTowardsTargetPos();
        }
        else if (keyPressed)
        {
            if (PlayerIsAround())
            {
                ChaseThroughWalls();
            } else 
            {
                RandomMoveThroughWallsPos();
            }
        }
    }

    private void RandomMoveThroughWallsPos()
    {

        int random = Random.Range(0, 4);

        if (random == 0)
        {
            Vector2Int destination = targetPos + Vector2Int.up;
            if (CheckMapLimit(destination.x, destination.y))
            {
                targetPos += Vector2Int.up;
            } else
            {
                RandomMoveThroughWallsPos();
            }
        }
        else if (random == 1)
        {
            Vector2Int destination = targetPos + Vector2Int.left;
            if (CheckMapLimit(destination.x, destination.y))
            {
                targetPos += Vector2Int.left;
            } else
            {
                RandomMoveThroughWallsPos();
            }
        }
        else if (random == 2)
        {
            Vector2Int destination = targetPos + Vector2Int.down;
            if (CheckMapLimit(destination.x, destination.y))
            {
                targetPos += Vector2Int.down;
            } else
            {
                RandomMoveThroughWallsPos();
            }
        }
        else if (random == 3)
        {
            Vector2Int destination = targetPos + Vector2Int.right;
            if (CheckMapLimit(destination.x, destination.y))
            {
                targetPos += Vector2Int.right;
            } else
            {
                RandomMoveThroughWallsPos();
            }
        }


    }
}