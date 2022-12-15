using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MiniMap : MonoBehaviour
{
    private Tilemap miniMap;
    private TilemapGenerator tileMapGenerator;
    private PlayerController playerController;
    public TileBase[] tileBase;
    private int[,] miniMapArray;
    public GameObject playerIcon;
    void Awake()
    {
        tileMapGenerator = GameObject.Find("Player").GetComponent<TilemapGenerator>();
        miniMap = gameObject.GetComponent<Tilemap>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            UpdateMiniMap((int)playerController.targetPos.y, (int)playerController.targetPos.x);
        }
    }

    void GenerateMiniMap(int x, int y)
    {
        miniMapArray = tileMapGenerator.createBlankArray(99, x, y);
    }

    void UpdateMiniMap(int y, int x)
    {
        int ArraySize = 100*100;
        Vector3Int[] positions = new Vector3Int[ArraySize];
        TileBase[] tileArray = new TileBase[ArraySize];
        int index = 0;
        y = -y;
        for (int i = y - 4; i < y + 4; i++)
        {
            for (int j = x - 4; j < x + 4; j++)
            {
                if (j < tileMapGenerator.mapSizeX && j >= 0 && i < tileMapGenerator.mapSizeY && i >= 0)
                {
                    int thisTile = tileMapGenerator.getExactTileValueAtCoordinates(j, i);
                    int numberOfFloors = tileMapGenerator.checkAdjacentTilesForFloors(i, j);
                    if (numberOfFloors > 0)
                    {
                        tileArray[index] = tileBase[thisTile];
                        positions[index] = new Vector3Int(j, -i, 0);
                    }
                }
                ++index;
            }
        }
        miniMap.SetTiles(positions, tileArray);
        miniMap.SetTile(new Vector3Int(x, -y), tileBase[5]);
    }

    public void ClearAll()
    {
        miniMap.ClearAllTiles();
        /*for (int y = 0; y < tileMapGenerator.mapSizeY; y++)
        {
            for (int x = 0; x < tileMapGenerator.mapSizeX; x++)
            {
                //miniMap.SetTile(new Vector3Int(x,y,0), null);
            }
        }*/
    }

    public void ShrinkForDepth()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x-0.02f, gameObject.transform.localScale.y-0.02f, 0f);
    }
}
