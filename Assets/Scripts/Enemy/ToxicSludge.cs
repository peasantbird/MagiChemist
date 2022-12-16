using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToxicSludge : Enemy
{
    public AudioClip ambientVoice;
    public AudioClip hitSound;
    //private Tilemap worldTerrain;
    public GameObject toxins;
    private IDictionary<Vector2Int, GameObject> toxinTiles;
    private IDictionary<Vector2Int, float> toxinTilesTimeToDespawn;
    private GameObject terrainElementContainer;
    // Start is called before the first frame update
    void Start()
    {
        //worldTerrain = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        base.InitEnemy();
        toxinTiles = new Dictionary<Vector2Int, GameObject>();
        toxinTilesTimeToDespawn = new Dictionary<Vector2Int, float>();
        terrainElementContainer = GameObject.Find("TerrainElements");
    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateEnemy();
        //base.MoveEnemy();
        if (moving)
        {
            if (!toxinTiles.ContainsKey(new Vector2Int((int)transform.position.x, (int)transform.position.y)))
            {
                spawnToxins();
            }
            UpdateToxins();
        }
    }

    public override void PlayVoice()
    {
        //play sludge voice
        base.SFX.PlayOneShot(ambientVoice);
    }

    public override void PlayHitSound()
    {
        //play slime voice
        AudioSource.PlayClipAtPoint(hitSound, new Vector3(transform.position.x, transform.position.y, 0));
    }

    private void spawnToxins()
    {
        Vector2Int spawnToxinsPos = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        GameObject thisToxin = Instantiate(toxins, new Vector3((int)transform.position.x, (int)transform.position.y, 0), Quaternion.identity);
        toxinTiles.Add(spawnToxinsPos, thisToxin);
        toxinTilesTimeToDespawn.Add(spawnToxinsPos, Random.Range(4f, 10f));
        thisToxin.transform.parent = terrainElementContainer.transform;
    }

    private void UpdateToxins()
    {
        List<Vector2Int> toxinsTimeToDecrease = new List<Vector2Int>();
        foreach(KeyValuePair<Vector2Int, float> entry in toxinTilesTimeToDespawn)
        {
            toxinsTimeToDecrease.Add(entry.Key);
        }
        foreach (Vector2Int key in toxinsTimeToDecrease)
        {
            toxinTilesTimeToDespawn[key] -= Time.deltaTime*5;
        }

        var toRemove = toxinTilesTimeToDespawn.Where(kvp => kvp.Value <= 0).ToList();
        foreach (var item in toRemove) {
            toxinTilesTimeToDespawn.Remove(item.Key);
            Destroy(toxinTiles[item.Key]);
            toxinTiles.Remove(item.Key);
        }
    }

    public override void EnemyStartAction()
    {
        base.TakeAction();
    }

    //public override void ReactToElement(Item playerSelectedItem)
    //{
    //    Item.ItemType itemType = playerSelectedItem.itemType;
    //    string msg = "";
    //    if (itemType == Item.ItemType.Mercury)
    //    {
    //        msg = "Oops!, " + itemType.ToString() + " strengthened " + enemyName + ", be careful!";
    //        hostility++;
    //    }
    //    playerController.message.PushMessage(msg, 2);

    //    Debug.Log(enemyName + " reacted to " + playerSelectedItem.itemType.ToString());
    //}
}
