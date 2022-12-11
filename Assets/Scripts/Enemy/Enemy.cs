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


    public void InitEnemy() {
        player = GameObject.Find("Player");
        tileMapGenerator = GameObject.Find("Player").GetComponent<TilemapGenerator>();
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
        currentMap = tileMapGenerator.currentMap;
    }

    public void MoveEnemy()
    {
        bool moving = (Vector2)transform.position != targetPos;
        bool keyPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        
        if (moving)
        {
            MoveTowardsTargetPos();
        }
        else if (PlayerIsAround() && keyPressed) {
            ChasePlayer();
        
        }
        else if (keyPressed)
        {
            RandomMovePos();
        }
    }

    private void MoveTowardsTargetPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void ChasePlayer() {
        Vector3 playerPos = player.transform.position;
        float currentX = transform.position.x;
        float currentY = transform.position.y;

        int xMovement = 0;
        int yMovement = 0;
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
            if (Movable(targetPos.x, targetPos.y-1))
            {
                yMovement = -1;
            }
        }
        else if (playerPos.y > currentY)
        { //player is at the top
            
            if (Movable(targetPos.x, targetPos.y+1))
            {
                yMovement = 1;
            }
        }
       

        if (xMovement != 0 && yMovement != 0) { //prevent diagonal movement
            int randomNum = Random.Range(0, 1);
            if (randomNum == 0)
            {
                xMovement = 0;
            }
            else {
                yMovement = 0;
            }
        }

        Vector2Int destination = targetPos + new Vector2Int(xMovement, yMovement);
        if (Movable(destination.x, destination.y))
        {
            targetPos += new Vector2Int(xMovement, yMovement);
        }
        else {
            RandomMovePos();
        }
      

    }

    private bool Movable(int xDestination, int yDestination) {
        int destinationTile = tileMapGenerator.checkTileAtCoordinates(xDestination, -yDestination);
        return destinationTile == 0;
    }
    private void RandomMovePos()
    {
        int random = Random.Range(0, 3);
        
            if (random == 0)
            {
                Vector2Int destination = targetPos + Vector2Int.up;
                if (Movable(destination.x, destination.y))
                {
                    targetPos += Vector2Int.up;
                }
                else {
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


    public virtual void PlayVoice() { 
     //virtual method
    }

    public bool PlayerIsAround() {
        //to check if player is around a certain distance

        //todo: write an algorithm to check the player is within a certain distance
        return false;
    }

    
}