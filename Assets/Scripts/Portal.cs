using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private TilemapGenerator tileMapGenerator;
    private PlayerController playerController;
    private AudioSource SFX;
    private void Start()
    {
        tileMapGenerator = GameObject.Find("Player").GetComponent<TilemapGenerator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.SFX.PlayOneShot(playerController.soundEffects[5]);
            DestroyCurrentMapObjects();
            GenerateNextLevel();
        }
    }

    private void DestroyCurrentMapObjects()
    {
        GameObject enemyContainer = GameObject.Find("Enemies");
            GameObject terrainElementContainer = GameObject.Find("TerrainElements");
            foreach (Transform child in enemyContainer.transform) {
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in terrainElementContainer.transform) {
                GameObject.Destroy(child.gameObject);
            }
    }

    private void GenerateNextLevel()
    {
        tileMapGenerator.depth++;
        tileMapGenerator.mapSizeX+=10;
        tileMapGenerator.mapSizeY+=10;
        tileMapGenerator.numberOfRooms = tileMapGenerator.mapSizeX/2 + Random.Range(-3, 3);
        tileMapGenerator.maxRoomSize = (int)Random.Range(5, 11);
        tileMapGenerator.GenerateLevel();
        playerController.RefreshPlayerPosition();
        tileMapGenerator.RenderTerrain(tileMapGenerator.mapSizeX, tileMapGenerator.mapSizeY, 0, 0, 0, 0);
        Debug.Log("New Level Generated");
    }
}
