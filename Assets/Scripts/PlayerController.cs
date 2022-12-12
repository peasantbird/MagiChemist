using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public GameObject spellRangeElement;
    public TilemapGenerator tileMapGenerator;
    public float moveRate;
    public float speed;
    public int spellRange;
    [SerializeField] private UI_Inventory uiInventory;

    protected int[,] currentMap;
    private float nextMove;
    private Vector2Int targetPos;
    private Inventory inventory;
    private bool rangeSpawned;
    private GameObject spellRangeObject;


    private void Awake()
    {
        Application.targetFrameRate = 60; // Restrict frame rate for better WebGL performance
        player = GameObject.Find("Player");
        currentMap = tileMapGenerator.currentMap;
        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        nextMove = Time.time;
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

        bool moving = (Vector2)transform.position != targetPos;

        if (moving)
        {
            MoveTowardsTargetPos();
        }
        else
        {
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

    void SpawnRange()
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
                    if (j >= currentPosition.x - num && j <= currentPosition.x + num) {
                        GameObject temp = Instantiate(spellRangeElement, new Vector3(j, i, 0), Quaternion.identity);
                        temp.transform.SetParent(spellRangeObject.transform,true);
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

    void RemoveRange()
    {
        rangeSpawned = false;
        Destroy(spellRangeObject);
        spellRangeObject = null;
    }
}
