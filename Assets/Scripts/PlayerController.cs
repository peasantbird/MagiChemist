using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject spellRangeElement;
    public TilemapGenerator tileMapGenerator;
    public float moveRate;
    public float speed;
    public int spellRange;
    public LayerMask enemyLayer;
    public int randomMoveInWaterPerc;
    public bool playerCanPassThroughEnemy;
    public int playerSight;
    public Item selectedItem;
    public Item defaultItem;
    public Item testItem;
    [SerializeField] private UI_Inventory uiInventory;

    protected int[,] currentMap;
    private float nextMove;
    private bool isMoving;
    private Vector2Int targetPos;
    private Inventory inventory;
    private bool rangeSpawned;
    private GameObject spellRangeObject;
    private Vector2Int playerMoveDir;
    private float initialSpeed;
    private bool noEnemyIsStillMoving;
    private bool noEnemyIsChasing;
    private bool noEnemyIsAround;
    


    public AudioClip[] soundEffects;
    public AudioSource SFX;

    // For HealthBar
    private Image healthBar;
   
    public int currentHealth;
    public int maxHealth;
    //

    public Vector2Int nextDirection;

    private void Awake()
    {
        Application.targetFrameRate = 60; // Restrict frame rate for better WebGL performance
        player = GameObject.Find("Player");
        currentMap = tileMapGenerator.currentMap;
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        nextMove = Time.time;
        playerMoveDir = new Vector2Int();
        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        maxHealth = 5;
        currentHealth = maxHealth;
        initialSpeed = speed;
        nextDirection = Vector2Int.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60; // Since it's turn based, to not be too resource intensive, we should limit FPS in the WEBGL build.
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
        rangeSpawned = false;
    }

    void Update()
    {
        CheckEnemyState();
        if (selectedItem == null) {
            RemoveRange();
            selectedItem = defaultItem;
            spellRange = selectedItem.itemRange;
            SpawnRange();
        }
        spellRange = selectedItem.itemRange;
        if (noEnemyIsAround)//move continuously
        {
            isMoving = (Vector2)transform.position != targetPos;

            if (isMoving)
            {
                MoveTowardsTargetPos();
                StartEnemyAction();
                if (rangeSpawned)
                {
                    RemoveRange();
                    SpawnRange();
                }
            }
            else
            {
                speed = initialSpeed;
                NewTargetPosContinuous();
               
            }

        }
        else //move grid based
        {

            if (isMoving && (Vector2)transform.position == targetPos && noEnemyIsStillMoving)//at the moment a player reach the destination, start enemies' turn
            {
                StartEnemyAction();
            }

            isMoving = (Vector2)transform.position != targetPos;

            if ((Vector2)transform.position != targetPos)
            {
                MoveTowardsTargetPos();
                if (rangeSpawned)
                {
                    RemoveRange();
                    SpawnRange();
                }
            }
            else if (noEnemyIsStillMoving)
            {
                speed = initialSpeed;
                NewTargetPos();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!rangeSpawned)
                {
                    SpawnRange();
                }
                else
                {
                    RemoveRange();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.X) && !isMoving)//press x to skip a turn
        {
            // Debug.Log(selectedItem.gameObject.name);
            testItem.amount = 1;
            transform.GetComponent<Inventory>().AddItem(testItem);
            StartEnemyAction();
        }

        if (Input.GetKeyDown(KeyCode.E) && !isMoving)//press e to harvest an item
        {
            //todoÅF harvest item
            StartEnemyAction();
        }
    }

    private void CheckEnemyState() {
        noEnemyIsStillMoving = true;
        noEnemyIsChasing = true;
        List<Enemy> enemies = tileMapGenerator.GetSpawnedEnemies();
        foreach (Enemy e in enemies) {
            if (e.moving) {
                noEnemyIsStillMoving = false;

            }
            if (e.isChasing) {
                noEnemyIsChasing = false;
            }
        }

        Collider[] hitColliders = Physics.OverlapBox(transform.position+new Vector3(0.5f,0.5f,0), new Vector3(playerSight/2,playerSight/2,0.1f), Quaternion.identity, enemyLayer);
        if (hitColliders.Length == 0)
        {
            noEnemyIsAround = true;
        }
        else {
            noEnemyIsAround = false;
        }

    }
    private void MoveTowardsTargetPos()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

    }

    private void NewTargetPosContinuous() {
        if (nextDirection == Vector2Int.zero) // If player is not prevented from moving
        {
            if (Input.GetKey(KeyCode.W) )
            {
                
                Vector2Int destination = targetPos + Vector2Int.up;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0 && TileHaveNoEnemy(destination.x, destination.y))
                {
                    playerMoveDir = Vector2Int.up;
                    targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
            else if (Input.GetKey(KeyCode.A) )
            {
                
                Vector2Int destination = targetPos + Vector2Int.left;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0 && TileHaveNoEnemy(destination.x, destination.y))
                {
                    playerMoveDir = Vector2Int.left;
                    targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
            else if (Input.GetKey(KeyCode.S) )
            {
               
                Vector2Int destination = targetPos + Vector2Int.down;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0 && TileHaveNoEnemy(destination.x, destination.y))
                {
                    playerMoveDir = Vector2Int.down;
                    targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
            else if (Input.GetKey(KeyCode.D) )
            {
                
                Vector2Int destination = targetPos + Vector2Int.right;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0 && TileHaveNoEnemy(destination.x, destination.y))
                {
                    playerMoveDir = Vector2Int.right;
                    targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
        }
        else if (nextDirection != Vector2Int.zero)
        { // When player is in water, water moves them again

    
            targetPos = nextDirection;
            floorBehaviourAndSound(nextDirection.x, nextDirection.y);
            nextDirection = Vector2Int.zero;
        }
        else
        {
            Debug.Log("Something broke.");
        }
    }
    private void NewTargetPos()
    {   if (nextDirection == Vector2Int.zero) // If player is not prevented from moving
        {
            if (Input.GetKeyDown(KeyCode.W) && Time.time >= nextMove)
            {
                nextMove = Time.time + moveRate;
                Vector2Int destination = targetPos + Vector2Int.up;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0  && TileHaveNoEnemy(destination.x,destination.y))
                {
                    playerMoveDir = Vector2Int.up;
                    targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) && Time.time >= nextMove)
            {
                nextMove = Time.time + moveRate;
                Vector2Int destination = targetPos + Vector2Int.left;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0 && TileHaveNoEnemy(destination.x, destination.y))
                {
                    playerMoveDir = Vector2Int.left;
                    targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S) && Time.time >= nextMove)
            {
                nextMove = Time.time + moveRate;
                Vector2Int destination = targetPos + Vector2Int.down;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0 && TileHaveNoEnemy(destination.x, destination.y))
                {
                    playerMoveDir = Vector2Int.down;
                   targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
            else if (Input.GetKeyDown(KeyCode.D) && Time.time >= nextMove)
            {
                nextMove = Time.time + moveRate;
                Vector2Int destination = targetPos + Vector2Int.right;
                int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
                if (destinationTile == 0 && TileHaveNoEnemy(destination.x, destination.y))
                {
                    playerMoveDir = Vector2Int.right;
                    targetPos += playerMoveDir;
                    floorBehaviourAndSound(destination.x, destination.y);
                }
            }
        } else if (nextDirection != Vector2Int.zero) { // When player is in water, water moves them again
           
            nextMove = Time.time + moveRate;
            targetPos = nextDirection;
            floorBehaviourAndSound(nextDirection.x, nextDirection.y);
            nextDirection = Vector2Int.zero;
        } else {
            Debug.Log("Something broke.");
        }

    }

    private void floorBehaviourAndSound(int x, int y)
    {
        int currentFloor = tileMapGenerator.getExactTileValueAtCoordinates(x, -y);
        if (currentFloor == 0) // Regular stone dungeon floor
        {
            speed *= 1; // 100% speed on dungeon floor
            SFX.PlayOneShot(soundEffects[0]); // normal walk
        } else if (currentFloor == 2) // Water
        {
            int randomNum = Random.Range(0, 99);
            speed *= 0.5f; //Player's movement speed halved in water
            if (randomNum >= 100-randomMoveInWaterPerc)
            {
                MovementInWater();
            }
            SFX.PlayOneShot(soundEffects[1]); // water walk
        } else if (currentFloor == 3) // Grass
        {
            speed *= 0.85f; // 10% slower on grass
            SFX.PlayOneShot(soundEffects[2]); // grass walk
        } else if (currentFloor == 4) // Sand
        {
            speed *= 0.7f; // Player's movement speed is -30% on sand (mostly cosmetic, due to turn-based unless projectiles are live)
            SFX.PlayOneShot(soundEffects[3], 0.2f); // sand walk
        }
    }

    private void MovementInWater()
    {
        
        List<Vector2Int> directions = new List<Vector2Int>();
        if (playerMoveDir != Vector2Int.down)
        {
            directions.Add(Vector2Int.up);
        }
        if (playerMoveDir != Vector2Int.up)
        {
            directions.Add(Vector2Int.down);
        }
        if (playerMoveDir != Vector2Int.right)
        {
            directions.Add(Vector2Int.left);
        }
        if (playerMoveDir != Vector2Int.left)
        {
            directions.Add(Vector2Int.right);
        }
       
       

        Vector2Int destination = targetPos +  directions[Random.Range(0, directions.Count)];
        int destinationTile = tileMapGenerator.checkTileAtCoordinates(destination.x, -destination.y);
        if (destinationTile == 0)
        {
            nextDirection = destination;
        } else 
        {
            MovementInWater();
        }
    }

    public void SpawnRange()
    {
        rangeSpawned = true;
        Vector2Int currentPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        spellRangeObject = new GameObject("SpellRange");
        spellRangeObject.transform.position = new Vector3(currentPosition.x,currentPosition.y,0);
        int num = 0;
     //   string debug = "";
        for (int i = currentPosition.y - spellRange; i <= currentPosition.y + spellRange; i++) {

            for (int j = currentPosition.x - spellRange; j <= currentPosition.x + spellRange; j++) {
                if (tileMapGenerator.CheckMapLimit(j, i))
                {
                    //  int tileAtCoordinates = currentMap[-i, j];
                    //  debug += tileAtCoordinates + " ";
                    if (tileMapGenerator.checkTileAtCoordinates(j, -i) == 0)
                    {
                        if (j >= currentPosition.x - num && j <= currentPosition.x + num) {
                            GameObject temp = Instantiate(spellRangeElement, new Vector3(j, i, 0), Quaternion.identity);
                            temp.transform.SetParent(spellRangeObject.transform,true);
                        }
                    }
                }
                else {
              //      debug += "NULL";
                }
            
            }
            if (i < currentPosition.y)
            {
                num++;
            }
            else if (i >= currentPosition.y) {
                num--;
            }
           // debug += "\n";
        }
        spellRangeObject.transform.SetParent(transform);
       // Debug.Log(debug);


    }
    private void StartEnemyAction() {
        List<Enemy> enemies = tileMapGenerator.GetSpawnedEnemies();
        foreach (Enemy e in enemies)
        {
            e.EnemyStartAction();
        }
    }
  
    private bool TileHaveNoEnemy(int x, int y) {
        if (!playerCanPassThroughEnemy)
        {
            Vector3 loc = new Vector3(x + 0.5f, y + 0.5f, 0.1f);
            Vector3 size = new Vector3(0.1f, 0.1f, 0.1f);
            Collider[] hitColliders = Physics.OverlapBox(loc, size, Quaternion.identity, enemyLayer);
            return hitColliders.Length == 0;
        }
        else {
            return true;
        }
       
    }
    public void RemoveRange()
    {
        rangeSpawned = false;
        Destroy(spellRangeObject);
        spellRangeObject = null;
    }


    public void RefreshHealthBar()
    {
        healthBar.fillAmount = (float)currentHealth/(float)maxHealth;
    }
    //private void OnDrawGizmos()
    //{
    //    if (hitCollidersLength != 0)
    //    {
    //        Gizmos.DrawCube(transform.position, new Vector3(playerSight, playerSight, playerSight));
    //    }
    //}

}
