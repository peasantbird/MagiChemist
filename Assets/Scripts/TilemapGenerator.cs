using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap worldTerrain;
    public TileBase[] floorPalette;
    public GameObject[] landscapeFeature;
    public List<Enemy> enemies;
    public int[,] currentMap;
    public int mapSizeX;
    public int mapSizeY;
    public int spawnNumber;
    private int newNoise;
    private GameObject enemyContainer;
    private GameObject terrainElementContainer;
    private List<Enemy> spawnedEnemies;
    private List<Vector2Int> usedPos; //prevent spawn overlap

    // Start is called before the first frame update
    private void Awake()
    {
        //currentMap = createBlankArray(0, 50, 50); // For coordinate testing
        spawnedEnemies = new List<Enemy>();
        usedPos = new List<Vector2Int>();
        currentMap = roomGenerator(21, 10, 50, 50);
        Vector2Int playerPos = getRandomFloorPos(new Vector2Int(100000,100000)); // Get random floor position on map
        usedPos.Add(playerPos);
        enemyContainer = new GameObject("Enemies");
        terrainElementContainer = new GameObject("TerrainElements");
        foreach (Enemy e in enemies)
        {//enermy walk test
            for (int i = 0; i <= spawnNumber; i++) // Ten of each enemy type
            {
                Vector2Int enemyPos = getRandomFloorPos(playerPos); //prevent from spawn around the player
                while (usedPos.Contains(enemyPos)) {
                    enemyPos = getRandomFloorPos(playerPos);
                }

                usedPos.Add(enemyPos);

                Enemy temp = Instantiate(e, new Vector3(0, 0, 0), Quaternion.identity);
                temp.name = temp.name + i;
                temp.transform.position = new Vector3Int(enemyPos.x, -enemyPos.y, 0);
                temp.SetSpawnPosition(temp.transform.position);
                temp.transform.parent = enemyContainer.transform;
                spawnedEnemies.Add(temp);
            }
        }
        transform.position = new Vector3Int(playerPos.x, -playerPos.y, 0); // Move player to random floor on map
    }

    private void Start()
    {
        RenderTerrain(50, 50, 0, 0, 0, 0); // Mapsize 50x50, offset Y axis 0
        //DebugConsoleMap(); // Prints the map in the console
    }

    private int[,] roomGenerator(int rooms, int maxSize, int xDimension, int yDimension)
    {
        //Create empty array
        int[,] numberedMap = createBlankArray(1, yDimension, xDimension);
        List<Vector2Int> roomCorridorPoints = new List<Vector2Int>(); // Store points on room border
        for (int i = 0; i < rooms; ++i)
        {
            // Spawn at random location within map, set to 0 (empty space)
            int roomX = Random.Range(1, xDimension - 1); // Ensure floors don't generate at edges   
            int roomY = Random.Range(1, yDimension - 1);
            int randomSizeX = Random.Range(1, maxSize);
            int randomSizeY = Random.Range(1, maxSize);
            //int floodChance = Random.Range(1, 5);
            for (int y = roomY; y < roomY + randomSizeY; ++y)
            {
                for (int x = roomX; x < roomX + randomSizeX; ++x)
                {
                    if (y < yDimension - 2 && y > 1 && x < xDimension - 2 && x > 1) // Check cell isn't out of bounds
                    {
                        numberedMap[y, x] = 0; // Carve floor
                    }
                }
            }
            // Add point on room border to array
            roomCorridorPoints.Add(new Vector2Int(roomX, roomY));
        }

        for (int i = 1; i < roomCorridorPoints.Count; ++i) // Connect each room to a tunnel
        {
            Vector2Int firstRoom = roomCorridorPoints[i - 1];
            Vector2Int secondRoom = roomCorridorPoints[i];
            generatePathBetweenTwoPoints(firstRoom, secondRoom, numberedMap);
        }

        getFloorTilesFromNoise(xDimension, yDimension, numberedMap);

        return numberedMap;
    }
    public void generatePathBetweenTwoPoints(Vector2 startPos, Vector2 endPos, int[,]numberedMap) 
    {
        bool moveInX = (Random.value > 0.5f);
        Vector2 pos = startPos;
        while(true) 
        {
            if (numberedMap[(int)pos.y, (int)pos.x] == 1) numberedMap[(int)pos.y, (int)pos.x] = 0;
            if (pos == endPos) return;
            if (moveInX) 
            {
                if (pos.x < endPos.x) pos.x++;
                else if (pos.x > endPos.x) pos.x--;
            } else 
            {
                if (pos.y < endPos.y) pos.y++;
                else if (pos.y > endPos.y) pos.y--;
            }
            if (Random.value < 0.1f) moveInX = !moveInX; // Small chance to change direction
        }
    }

    public void getFloorTilesFromNoise(int xDimension, int yDimension, int[,] numberedMap)
    {
        // Set Water if floor below certain height
        for (int y = 0; y < yDimension - 1; ++y) // This for loop could be streamlined by removing it and shifting interior code to earlier
        {
            for (int x = 0; x < xDimension - 1; ++x)
            {
                if (numberedMap[y, x] == 0)
                {
                    float scale = 0.0525f;
                    float tileHeight = Mathf.PerlinNoise((x + newNoise) * scale, (y + newNoise) * scale);
                    if (tileHeight < 0.32567)
                    {
                        numberedMap[y, x] = 2;
                    }
                    else if (tileHeight > 0.67025)
                    {
                        numberedMap[y, x] = 4;
                    }
                    else if (tileHeight < 0.6 && tileHeight > 0.5)
                    {
                        numberedMap[y, x] = 3;
                    }
                }
            }
        }
    }

    private int[,] createBlankArray(int num, int xDimension, int yDimension)
    {
        int[,] numberedMap = new int[xDimension, yDimension];
        for (int y = 0; y < yDimension; y++)
        {
            for (int x = 0; x < xDimension; x++)
            {
                numberedMap[y, x] = num;
            }
        }
        return numberedMap;
    }

    private void RenderTerrain(int screenX, int screenY, int playerX, int playerY, int offsetX, int offsetY)
    // offsetY should be -screenY
    // Location of player starts at x, y: 0, 0 by default unless changed
    {
        int ArraySize = screenX * screenY;
        Vector3Int[] positions = new Vector3Int[ArraySize];
        TileBase[] tileArray = new TileBase[ArraySize];
        TileBase[] tileArrayLandscape = new TileBase[ArraySize];
        int index = 0;
        for (int y = 0; y < screenY; ++y)
        {
            for (int x = 0; x < screenX; ++x)
            {
                int tileX = x + playerX;
                int tileY = y + playerY;
                positions[index] = new Vector3Int(offsetX + tileX, -tileY, 0); // TESTING
                int tileType = currentMap[tileY, tileX];
                if (tileType == 0)
                {
                    tileArray[index] = floorPalette[0]; // Floor
                    int oreSpawnChance = Random.Range(1, 30);
                    if (oreSpawnChance > 28)
                    {
                        int randomOreType = Random.Range(5, 8);
                        GameObject temp = Instantiate(landscapeFeature[randomOreType], new Vector3Int(tileX, -tileY, 0), Quaternion.identity); // Random Plant
                        temp.transform.parent = terrainElementContainer.transform;
                    }
                }
                else if (tileType == 2)
                {
                    GameObject temp = Instantiate(landscapeFeature[0], new Vector3Int(tileX, -tileY, 0), Quaternion.identity); // Water
                    temp.transform.parent = terrainElementContainer.transform;

                }
                else if (tileType == 3)
                {
                    int grassTileType = Random.Range(16, 20);
                    tileArray[index] = floorPalette[grassTileType]; // Grass
                    int plantSpawnChance = Random.Range(1, 10);
                    if (plantSpawnChance > 8)
                    {
                        int randomPlantType = Random.Range(1, 5);
                        GameObject temp = Instantiate(landscapeFeature[randomPlantType], new Vector3Int(tileX, -tileY, 0), Quaternion.identity); // Random Plant
                        temp.transform.parent = terrainElementContainer.transform;
                    }
                }
                else if (tileType == 4)
                {
                    int sandTileType = Random.Range(20, 24);
                    tileArray[index] = floorPalette[sandTileType]; // Sand
                }
                else if (tileType == 1)
                {
                    //int adjacentFloors = checkAdjacentTilesForFloors(tileY, tileX);
                    int[] adjacentFloors = checkAdjacentFloorsPosition(tileY, tileX); // Check clockwise from top left
                                                                                      // 0 top left, 
                                                                                      // 1 top mid
                                                                                      // 2 top right
                                                                                      // 3 mid left
                                                                                      // 4 mid mid, center tile (current tile)
                                                                                      // 5 mid right
                                                                                      // 6 bottom left
                                                                                      // 7 bottom mid
                                                                                      // 8 bottom right
                    if (adjacentFloors[1] == 0 && adjacentFloors[3] == 0 && adjacentFloors[5] == 0 && adjacentFloors[7] == 0)
                    {
                        tileArray[index] = floorPalette[1]; // Single wall
                    }
                    else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 1 && adjacentFloors[7] != 0)
                    {
                        tileArray[index] = floorPalette[2]; // Top and bottom floor, single wall
                    }
                    else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                    {
                        tileArray[index] = floorPalette[3]; // Left and right floor, single wall
                    }
                    else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5] != 1 && adjacentFloors[7] != 1)
                    {
                        tileArray[index] = floorPalette[4]; // Right, top and bottom floor, single wall
                    }
                    else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                    {
                        tileArray[index] = floorPalette[5]; // Left, top and bottom floor, single wall
                    }
                    else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 1 && adjacentFloors[5] != 1 && adjacentFloors[7] != 0)
                    {
                        tileArray[index] = floorPalette[6]; // Left, top and right floor, single wall
                    }
                    else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 1 && adjacentFloors[7] != 1)
                    {
                        tileArray[index] = floorPalette[7]; // Left, bottom and floor, single wall
                    }
                    else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 0)
                    {
                        tileArray[index] = floorPalette[8]; // right, up floor, single wall
                    }
                    else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5] != 1 && adjacentFloors[7] != 0)
                    {
                        tileArray[index] = floorPalette[9]; // left, up floor, single wall
                    }
                    else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                    {
                        tileArray[index] = floorPalette[10]; // right, down floor, single wall
                    }
                    else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 0 && adjacentFloors[5] != 1 && adjacentFloors[7] != 1)
                    {
                        tileArray[index] = floorPalette[11]; // left, down floor, single wall
                    }
                    else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 0 && adjacentFloors[5] != 1 && adjacentFloors[7] != 0)
                    {
                        tileArray[index] = floorPalette[12]; // right floor, single wall
                    }
                    else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 0)
                    {
                        tileArray[index] = floorPalette[13]; // left floor, single wall
                    }
                    else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5] != 0 && adjacentFloors[7] != 0)
                    {
                        tileArray[index] = floorPalette[14]; // up floor, single wall
                    }
                    else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 0 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                    {
                        tileArray[index] = floorPalette[15]; // bottom floor, single wall
                    }
                }
                ++index;
            }
        }
        worldTerrain.SetTiles(positions, tileArray);
    }

    Vector2Int getRandomFloorPos(Vector2Int awayFrom)
    {
        Vector2Int floorPos = Vector2Int.zero;
        int xPos = 0;
        int yPos = 0;
        while (currentMap[yPos, xPos] != 0 || (awayFrom-new Vector2Int(xPos,yPos)).magnitude<=5)
        {
            xPos = Random.Range(1, mapSizeX - 2);
            yPos = Random.Range(1, mapSizeY - 2);
        }
        floorPos = new Vector2Int(xPos, yPos);
        //for (int i = 0; i < 500; ++i)
        //{
        //    int xPos = Random.Range(1, mapSizeX - 2);
        //    int yPos = Random.Range(1, mapSizeY - 2);
        //    if (currentMap[yPos, xPos] == 0) 
        //    {
        //        floorPos = new Vector2Int(xPos, yPos); // if Floor
        //        break;
        //    }
        //}
        return floorPos;
    }

    public int checkAdjacentTilesForFloors(int y, int x)
    {
        int numberOfFloors = 0;
        int[,] directionsToCheck = new int[8, 2] { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 } };
        for (int i = 0; i < directionsToCheck.GetLength(0); ++i)
        {
            int checkX = x + directionsToCheck[i, 1];
            int checkY = y + directionsToCheck[i, 0];
            // Check that tile to be checked is within the bounds
            if ((checkY < mapSizeY && checkY >= 0 && checkX < mapSizeX && checkX >= 0))
            {
                if (currentMap[checkY, checkX] == 0)
                {
                    ++numberOfFloors;
                }
            }
        }
        //Debug.Log(numberOfFloors.ToString() + x.ToString() + ", " + y.ToString());
        return numberOfFloors; // If number of floors are zero, we do not instantiate that wall.
    }

    public int[] checkAdjacentFloorsPosition(int y, int x)
    {
        int[] mapVisualisation = new int[9] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        int[,] directionsToCheck = new int[9, 2] {
            {-1, -1},// top left, ROW,COLUMN: Y,X
            {-1, 0},// top mid
            {-1, 1},// top right
            {0, -1},// mid left
            {0, 0},// mid mid, center tile (current tile)
            {0, 1},// mid right
            {1, -1},// bottom left
            {1, 0},// bottom mid
            {1, 1},// bottom right
            };
        for (int i = 0; i < directionsToCheck.GetLength(0); ++i)
        {
            int checkX = x + directionsToCheck[i, 1];
            int checkY = y + directionsToCheck[i, 0];
            // Check that tile to be checked is within the bounds
            if ((checkY < mapSizeY && checkY >= 0 && checkX < mapSizeX && checkX >= 0))
            {
                if (currentMap[checkY, checkX] != 1)
                {
                    mapVisualisation[i] = 0;
                }
            }
        }
        //Debug.Log(numberOfFloors.ToString() + x.ToString() + ", " + y.ToString());
        return mapVisualisation; // If number of floors are zero, we do not instantiate that wall.
    }

    private void DebugConsoleMap()
    {
        StringBuilder sb = new StringBuilder();
        int xDimension = currentMap.GetLength(1);
        int yDimension = currentMap.GetLength(0);
        for (int y = 0; y < yDimension; y++)
        {
            for (int x = 0; x < xDimension; x++)
            {
                sb.Append(currentMap[y, x]);
                sb.Append(' ');
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }

    public int checkTileAtCoordinates(int x, int y)
    {
        int tileAtCoordinates = currentMap[y, x];
        if (tileAtCoordinates != 1)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public int getExactTileValueAtCoordinates(int x, int y)
    {
        int tileAtCoordinates = currentMap[y, x];
        return tileAtCoordinates;
    }

    public bool CheckMapLimit(int xDestination, int yDestination)
    {
        if (xDestination < 0 || xDestination > mapSizeX - 1 || yDestination > 0 || yDestination < -(mapSizeY - 1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public List<Enemy> GetSpawnedEnemies() {
        return spawnedEnemies;
    }
}
