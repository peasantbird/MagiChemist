using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGenerator : MonoBehaviour
{
    public Tilemap worldTerrain;
    public TileBase[] floorPalette;
    public int[,]currentMap;
    public int mapSizeX;
    public int mapSizeY;
    // Start is called before the first frame update
    private void Awake()
    {
        currentMap = roomGenerator(40, 10, mapSizeX, mapSizeY); // Placeholder random generation
    }

    private void Start(){
        RenderTerrain(50,50,0,0,0,-50); // Mapsize 50x50, offset Y axis -50
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
            for (int y = roomY; y < roomY + randomSizeY; ++y)
            {
                for (int x = roomX; x < roomX + randomSizeX; ++x)
                {
                    if (y < yDimension - 2 && y > 1 && x < xDimension - 2 && x > 1) // Check cell isn't out of bounds
                    {
                        numberedMap[y, x] = 0; // Carve a floor
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

    void RenderTerrain(int screenX, int screenY, int playerX, int playerY, int offsetX, int offsetY) 
    // offsetY should be -screenY
    // Location of player starts at x, y: 0, 0 by default unless changed
    {
        int ArraySize = screenX*screenY;
        Vector3Int[] positions = new Vector3Int[ArraySize];
        TileBase[] tileArray = new TileBase[ArraySize];
        TileBase[] tileArrayLandscape = new TileBase[ArraySize];
        int index = 0;
            for (int x = 0; x < screenX; ++x) 
            {
                for (int y = 0; y < screenY; ++y) 
                {
                    int tileX =  x + playerX;
                    int tileY =  y + playerY;
                    positions[index] = new Vector3Int(offsetX + tileX, offsetY + tileY, 0);
                    int tileType = currentMap[tileX, tileY];
                    if (tileType == 0) 
                    {
                        tileArray[index] = floorPalette[0];
                    } else if (tileType == 1)
                    {
                        tileArray[index] = floorPalette[1];
                    }
                    ++index;
                }
            }
        worldTerrain.SetTiles(positions, tileArray);
    }

}
