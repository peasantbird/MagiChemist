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
    public int[,]currentMap;
    public int mapSizeX;
    public int mapSizeY;
    private int newNoise;
    // Start is called before the first frame update
    private void Awake()
    {
        //currentMap = createBlankArray(0, 50, 50); // For coordinate testing
        Vector2Int playerPos = getRandomFloorPos(); // Get random floor position on map

        foreach (Enemy e in enemies) {//enermy walk test
            for (int i = 0; i < 5; i++) // Ten of each enemy type
            {
                Vector2Int enemyPos = getRandomFloorPos();

                Enemy temp = Instantiate(e, new Vector3(0,0,0),Quaternion.identity);
                temp.transform.position = new Vector3Int(enemyPos.x, -enemyPos.y, 0);
            }
        }
        transform.position = new Vector3Int(playerPos.x, -playerPos.y, 0); // Move player to random floor on map
    }

    private void Start(){
        RenderTerrain(50,50,0,0,0,0); // Mapsize 50x50, offset Y axis 0
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
            int roomX = Random.Range(1, yDimension - 1); // Ensure floors don't generate at edges   
            int roomY = Random.Range(1, xDimension - 1); 
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
            Vector2Int firstRoom = roomCorridorPoints[i-1];
            Vector2Int secondRoom = roomCorridorPoints[i];
            int checkY = secondRoom.y - firstRoom.y;
            int checkX = secondRoom.x - firstRoom.x;
            int lesserHeight = 0, lesserWidth = 0, greaterHeight = 0, greaterWidth = 0;
            if (checkY < 0) 
            { 
                greaterHeight = firstRoom.y;
                lesserHeight = secondRoom.y;
            } else if (checkY > 0)
            {   greaterHeight = secondRoom.y;
                lesserHeight = firstRoom.y;
            } else if (checkY == 0)
            { 
                greaterHeight = -1; // invalid
            }
            if (checkX < 0) 
            { 
                greaterWidth = firstRoom.x;
                lesserWidth = secondRoom.x;
            } else if (checkX > 0)
            { 
                greaterWidth = secondRoom.x;
                lesserWidth = firstRoom.x;
            } else if (checkX == 0)
            {
                greaterWidth = -1; // invalid
            }
            if (greaterHeight == -1)
            {   if (greaterWidth != -1)
                {
                    int y = firstRoom.y;
                    for (int x = lesserWidth; x < greaterWidth; ++x) // Testing
                    {
                        if (y < yDimension - 2 && y > 1 && x < xDimension - 2 && x > 1)
                        {
                        numberedMap[y, x] = 0; // Carve a floor
                        }
                    }
                }
            } else if (greaterWidth == -1)
            {
                int x = firstRoom.x;
                for (int y = lesserHeight; y < greaterHeight; ++y)
                    {
                        if (y < yDimension - 2 && y > 1 && x < xDimension - 2 && x > 1)
                        {
                        numberedMap[y, x] = 0; // Carve a floor
                        }
                    }
            } else {
                int x = lesserWidth; 
                for (int y = lesserHeight; y < greaterHeight; ++y)
                {
                    if (y < yDimension - 2 && y > 1 && x < xDimension - 2 && x > 1)
                        {
                        numberedMap[y, x] = 0; // Carve a floor
                        }
                }
                int j = lesserHeight;
                for (int k = lesserWidth; k < greaterWidth; ++k)
                {
                    if (j < yDimension - 2 && j > 1 && k < xDimension - 2 && k > 1)
                        {
                        numberedMap[j, k] = 0; // Carve a floor
                        }
                }
            }
        }
        // Set Water if floor below certain height
        for (int y = 0; y < yDimension - 1; ++y) // This for loop could be streamlined by removing it and shifting interior code to earlier
        {
            for (int x = 0; x < xDimension - 1; ++x)
            {
                if (numberedMap[y, x] == 0)
                {
                    float scale = 0.0525f;
                    float tileHeight = Mathf.PerlinNoise((x+newNoise)*scale, (y+newNoise)*scale);
                    if (tileHeight < 0.32567)
                    {
                        numberedMap[y, x] = 2;
                    } else if (tileHeight > 0.67025)
                    {
                        numberedMap[y, x] = 4;
                    } else if (tileHeight < 0.6 && tileHeight > 0.5)
                    {
                        numberedMap[y, x] = 3;
                    }
                }
            }
        }
        return numberedMap;
    }

    private int[,] createBlankArray(int num, int xDimension, int yDimension) 
    {
        int[,] numberedMap = new int[xDimension, yDimension]; 
        for (int y = 0; y < yDimension; y++) 
        { 
            for (int x = 0; x < xDimension; x++) 
            {  
            numberedMap[y,x] = num;      
            }    
        }    
        return numberedMap;  
    }

    private void RenderTerrain(int screenX, int screenY, int playerX, int playerY, int offsetX, int offsetY) 
    // offsetY should be -screenY
    // Location of player starts at x, y: 0, 0 by default unless changed
    {
        int ArraySize = screenX*screenY;
        Vector3Int[] positions = new Vector3Int[ArraySize];
        TileBase[] tileArray = new TileBase[ArraySize];
        TileBase[] tileArrayLandscape = new TileBase[ArraySize];
        int index = 0;
            for (int y = 0; y < screenY; ++y) 
            {
                for (int x = 0; x < screenX; ++x) 
                {
                    int tileX =  x + playerX;
                    int tileY =  y + playerY;
                    positions[index] = new Vector3Int(offsetX + tileX, -tileY, 0); // TESTING
                    int tileType = currentMap[tileY, tileX];
                    if (tileType == 0) 
                    {
                        tileArray[index] = floorPalette[0]; // Floor
                    } else if (tileType == 2)
                    {
                        Instantiate(landscapeFeature[0], new Vector3Int(tileX, -tileY, 0),Quaternion.identity); // Water
                    } else if (tileType == 3)
                    {
                        int grassTileType = Random.Range(16, 20);
                        tileArray[index] = floorPalette[grassTileType]; // Grass
                        int plantSpawnChance = Random.Range(1, 10);
                        if (plantSpawnChance > 8)
                        {
                            int randomPlantType = Random.Range(1, 5);
                            Instantiate(landscapeFeature[randomPlantType], new Vector3Int(tileX, -tileY, 0),Quaternion.identity); // Random Plant
                        }
                    } else if (tileType == 4)
                    {
                        int sandTileType = Random.Range(20, 24);
                        tileArray[index] = floorPalette[sandTileType]; // Sand
                    } else if (tileType == 1)
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
                        } else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 1 && adjacentFloors[7] != 0)
                        {
                            tileArray[index] = floorPalette[2]; // Top and bottom floor, single wall
                        } else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                        {
                            tileArray[index] = floorPalette[3]; // Left and right floor, single wall
                        } else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5] != 1 && adjacentFloors[7] != 1)
                        {
                            tileArray[index] = floorPalette[4]; // Right, top and bottom floor, single wall
                        } else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                        {
                            tileArray[index] = floorPalette[5]; // Left, top and bottom floor, single wall
                        } else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 1 && adjacentFloors[5] != 1 && adjacentFloors[7] != 0)
                        {
                            tileArray[index] = floorPalette[6]; // Left, top and right floor, single wall
                        } else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 1 && adjacentFloors[7] != 1)
                        {
                            tileArray[index] = floorPalette[7]; // Left, bottom and floor, single wall
                        } else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 0)
                        {
                            tileArray[index] = floorPalette[8]; // right, up floor, single wall
                        } else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5]!= 1 && adjacentFloors[7] != 0)
                        {
                            tileArray[index] = floorPalette[9]; // left, up floor, single wall
                        } else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                        {
                            tileArray[index] = floorPalette[10]; // right, down floor, single wall
                        } else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 0 && adjacentFloors[5] != 1 && adjacentFloors[7] != 1)
                        {
                            tileArray[index] = floorPalette[11]; // left, down floor, single wall
                        } else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 0 && adjacentFloors[5] != 1 && adjacentFloors[7] != 0)
                        {
                            tileArray[index] = floorPalette[12]; // right floor, single wall
                        } else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 1 && adjacentFloors[5] != 0 && adjacentFloors[7] != 0)
                        {
                            tileArray[index] = floorPalette[13]; // left floor, single wall
                        } else if (adjacentFloors[1] != 1 && adjacentFloors[3] != 0 && adjacentFloors[5] != 0 && adjacentFloors[7] != 0)
                        {
                            tileArray[index] = floorPalette[14]; // up floor, single wall
                        } else if (adjacentFloors[1] != 0 && adjacentFloors[3] != 0 && adjacentFloors[5] != 0 && adjacentFloors[7] != 1)
                        {
                            tileArray[index] = floorPalette[15]; // bottom floor, single wall
                        }
                    }
                    ++index;
                }
            }
        worldTerrain.SetTiles(positions, tileArray);
    }

    Vector2Int getRandomFloorPos()
    {
        Vector2Int floorPos = Vector2Int.zero;
        int xPos = 0;
        int yPos = 0;
        while (currentMap[yPos, xPos]!=0) {
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
        int[,] directionsToCheck = new int[8,2] {{1, 0}, {-1, 0}, {0, 1}, {0, -1}, {1, 1}, {-1, -1}, {1, -1}, {-1, 1}};
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
        int[] mapVisualisation = new int[9] {1, 1, 1, 1, 1, 1, 1, 1, 1};
        int[,] directionsToCheck = new int[9,2] {
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
        for(int y = 0; y < yDimension; y++)
        {
            for(int x = 0; x < xDimension; x++)
            {
                sb.Append(currentMap[y,x]);
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
        return tileAtCoordinates;
    }

}
