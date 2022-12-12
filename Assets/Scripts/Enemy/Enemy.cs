using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public int hostility;
    public List<int> drops;
    public Type enemyType = new Type();
    public float speed;
    public float moveRate;
    public int enemySight;
    public float movableDistance;
    public LayerMask spellRangeLayer;
    protected TilemapGenerator tileMapGenerator;
    protected Vector2Int targetPos;
    protected int[,] currentMap;
    protected GameObject player;
    private Vector3 spawnPosition;
    private float nextMove;
    private Animator anim;
    private Vector2Int moveVector;



    public void InitEnemy()
    {
        player = GameObject.Find("Player");
        tileMapGenerator = GameObject.Find("Player").GetComponent<TilemapGenerator>();
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
        currentMap = tileMapGenerator.currentMap;
        nextMove = Time.time;
        anim = GetComponent<Animator>();
    }

    public void UpdateEnemy()
    {
        if (hp == 0)
        {
            Destroy(this.gameObject);
            Debug.Log(name + " is dead");
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.1f);//to let it be a little bit on top of the surface


        //if (transform.position == new Vector3(targetPos.x,targetPos.y,0.1f) && anim.GetBool("IsWalking")) {
        //    anim.SetBool("IsWalking", false);
        //    anim.SetBool("Up", false);
        //    anim.SetBool("Down", false);
        //    anim.SetBool("Right", false);
        //    anim.SetBool("Left", false);
        //}

    }

    private void OnMouseDown()
    {

        if (DetectCollision(spellRangeLayer))
        {

            React();
        }

    }

    private void React()
    {
        Debug.Log("Enemy took 1 dmg");
        hp--;

    }

    private void SetAnimationDir() {
        
        if (moveVector == Vector2Int.up)
        { //move up
            anim.SetBool("Up", true);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Left", false);
        }
        else if (moveVector == Vector2Int.down)
        { //move down

            anim.SetBool("Up", false);
            anim.SetBool("Down", true);
            anim.SetBool("Right", false);
            anim.SetBool("Left", false);
        }
        else if (moveVector == Vector2Int.left)
        { //move left
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Left", true);
        }
        else if (moveVector == Vector2Int.right)
        { //move right 
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", true);
            anim.SetBool("Left", false);

        }
    }
    private void MoveTowardsTargetPos()
    {
        SetAnimationDir();
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private bool DetectCollision(LayerMask layer)
    {
        bool collider = Physics2D.OverlapBox(transform.position + new Vector3(0.5f, 0.5f, -0.1f), new Vector3(0.5f, 0.5f, 0), 0, layer);
        return collider;
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
              //  SetAnimationDir();
            }
            else if (Time.time >= nextMove)
            {
                nextMove = Time.time + moveRate;
                RandomMovePos();
               // SetAnimationDir();
            }
            
        }
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

            moveVector = new Vector2Int(xMovement, yMovement);
            Vector2Int destination = targetPos + moveVector;
            if (Movable(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
            {
                targetPos += moveVector;
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


    private void RandomMovePos()
    {



        int random = Random.Range(0, 4);

        if (random == 0)
        {
            moveVector = Vector2Int.up;
            Vector2Int destination = targetPos + moveVector;
            if (Movable(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
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
            moveVector = Vector2Int.left;
            Vector2Int destination = targetPos + moveVector;
            if (Movable(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
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
            moveVector = Vector2Int.down;
            Vector2Int destination = targetPos + moveVector;
            if (Movable(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
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
            moveVector = Vector2Int.right;
            Vector2Int destination = targetPos + moveVector;
            if (Movable(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
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
        Vector2Int playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
        Vector2Int enemyPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        //todo: write an algorithm to check the player is within a certain distance
        //get magnitude (distance) between playerPos and enemyPos
        if ((enemyPos - playerPos).magnitude < enemySight)
        {
            if (CanEnemySeePlayer() == true) //Is the monster's sight obstructed by a wall tile?
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool CanEnemySeePlayer()
    {
        Vector2Int playerPos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
        Vector2Int enemyPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        Vector2Int differencePos = playerPos - enemyPos;
        int distanceBetween = (int)((enemyPos - playerPos).magnitude);
        Vector2Int checkUp = enemyPos + Vector2Int.up * distanceBetween;
        Vector2Int checkDown = enemyPos + Vector2Int.down * distanceBetween;
        Vector2Int checkLeft = enemyPos + Vector2Int.left * distanceBetween;
        Vector2Int checkRight = enemyPos + Vector2Int.right * distanceBetween;
        if (playerPos == checkUp)
        {
            for (int i = 1; i < enemySight; i++)
            {
                Vector2Int checkUp2 = enemyPos + Vector2Int.up * i;
                if (tileMapGenerator.checkTileAtCoordinates(checkUp2.x, -checkUp2.y) == 1)
                {
                    return false;
                }
                else if (i == enemySight)
                {
                    return true;
                }
            }
        }
        else if (playerPos == checkDown)
        {
            for (int i = 1; i < enemySight; i++)
            {
                Vector2Int checkDown2 = enemyPos + Vector2Int.up * i;
                if (tileMapGenerator.checkTileAtCoordinates(checkDown2.x, -checkDown2.y) == 1)
                {
                    return false;
                }
                else if (i == enemySight)
                {
                    return true;
                }
            }
        }
        else if (playerPos == checkRight)
        {
            for (int i = 1; i < enemySight; i++)
            {
                Vector2Int checkRight2 = enemyPos + Vector2Int.right * i;
                if (tileMapGenerator.checkTileAtCoordinates(checkRight2.x, -checkRight2.y) == 1)
                {
                    return false;
                }
                else if (i == enemySight)
                {
                    return true;
                }
            }
        }
        else if (playerPos == checkLeft)
        {
            for (int i = 1; i < enemySight; i++)
            {
                Vector2Int checkLeft2 = enemyPos + Vector2Int.left * i;
                if (tileMapGenerator.checkTileAtCoordinates(checkLeft2.x, -checkLeft2.y) == 1)
                {
                    return false;
                }
                else if (i == enemySight)
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
            moveVector = new Vector2Int(xMovement, yMovement);
            Vector2Int destination = targetPos + moveVector;
            if (tileMapGenerator.CheckMapLimit(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
            {
                targetPos += moveVector;
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
               // SetAnimationDir();
            }
            else if (Time.time >= nextMove)
            {
                nextMove = Time.time + moveRate;

                RandomMoveThroughWallsPos();
              //  SetAnimationDir();
            }
           
        }
    }

    private void RandomMoveThroughWallsPos()
    {

        int random = Random.Range(0, 4);

        if (random == 0)
        {
            moveVector = Vector2Int.up;
            Vector2Int destination = targetPos + moveVector;
            if (tileMapGenerator.CheckMapLimit(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
            {
                targetPos += Vector2Int.up;
                
            }
            else
            {
                RandomMoveThroughWallsPos();
            }
        }
        else if (random == 1)
        {
            moveVector = Vector2Int.left;
            Vector2Int destination = targetPos + moveVector;
            if (tileMapGenerator.CheckMapLimit(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
            {
                targetPos += Vector2Int.left;
            
            }
            else
            {
                RandomMoveThroughWallsPos();
            }
        }
        else if (random == 2)
        {
            moveVector = Vector2Int.down;
            Vector2Int destination = targetPos + moveVector;
            if (tileMapGenerator.CheckMapLimit(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
            {
                targetPos += Vector2Int.down;
               
            }
            else
            {
                RandomMoveThroughWallsPos();
            }
        }
        else if (random == 3)
        {
            moveVector = Vector2Int.right;
            Vector2Int destination = targetPos + moveVector;
            if (tileMapGenerator.CheckMapLimit(destination.x, destination.y) && WithinRestrictedDistance(destination.x, destination.y))
            {
                targetPos += Vector2Int.right;
               
            }
            else
            {
                RandomMoveThroughWallsPos();
            }
        }


    }

    public void SetSpawnPosition(Vector3 spawnPosition)
    {
        this.spawnPosition = spawnPosition;
    }

    private bool Movable(int xDestination, int yDestination)
    {

        int destinationTile = tileMapGenerator.checkTileAtCoordinates(xDestination, -yDestination);
        return destinationTile == 0;
    }

    private bool WithinRestrictedDistance(int xDestination, int yDestination)
    {
        bool withinDistance = (spawnPosition - new Vector3(xDestination, yDestination)).magnitude <= movableDistance;
        return withinDistance;
    }


}