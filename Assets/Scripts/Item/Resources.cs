using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    public List<Item> harvestableItems;
    public int minYield;
    public int maxYield;
    
   
    // Start is called before the first frame update
    void Start()
    {
       // player.transform.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Item> Harvest() {
        int yield = Random.Range(minYield, maxYield+1);
        List<Item> obtainedItems = new List<Item>();
        while (yield != 0) {
            if (harvestableItems.Count >= yield)
            {
                int index = Random.Range(0, harvestableItems.Count);
                obtainedItems.Add(harvestableItems[index]);
                harvestableItems.RemoveAt(index);
                yield--;
            }
            else {
                break;
            }
        }

        return obtainedItems;
        
    }

    public void DestroyResource() {
        Destroy(this.gameObject);
    }
}
