using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PsychedelicTilemap : MonoBehaviour
{
    public Tilemap worldTerrain;
    private SpriteRenderer playerSprite;
    private float timeLeft;
    private Color[] targetColours;
    private Vector3Int[] colourPositions;
    // Start is called before the first frame update
    void Start()
    { 
        playerSprite = GameObject.Find("Player Animation").GetComponent<SpriteRenderer>();
        timeLeft = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= Time.deltaTime)
        {
            targetColours = new Color[512];
            colourPositions = new Vector3Int[512];
            int index = 0;
            for (int y= -8; y < 8; ++y)
            {
                for (int x=-8; x< 8; ++x)
                {
                    colourPositions[index] = new Vector3Int ((int)transform.position.x + x, (int)transform.position.y + y, 0);
                    targetColours[index] = GetRandomColour();
                    //SetTileColour(targetColour, magicColor, worldTerrain);
                    timeLeft = 1.0f;
                    ++index;
                }
            }
            targetColours[++index] = GetRandomColour();
        } else
        {
            int index = 0;
            for (int y= -8; y < 8; ++y)
            {
                for (int x=-8; x< 8; ++x)
                {
                    Color currentColour = worldTerrain.GetColor(new Vector3Int ((int)transform.position.x + x, (int)transform.position.y + y, 0));
                    SetTileColour(Color.Lerp(currentColour, targetColours[index], Time.deltaTime / timeLeft), colourPositions[index], worldTerrain);
                    ++index;
                }
            }
            Color currentPlayerColour = playerSprite.color;
            playerSprite.color = Color.Lerp(currentPlayerColour, targetColours[++index], Time.deltaTime / timeLeft);
            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }

    private void SetTileColour(Color colour, Vector3Int position, Tilemap tilemap)
    {
        worldTerrain.SetTileFlags(position, TileFlags.None);

        // Set the colour.
        worldTerrain.SetColor(position, colour);
    }

    private Color GetRandomColour()
    {
        Color randomColor = new Color (Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        return randomColor;
    }
}
