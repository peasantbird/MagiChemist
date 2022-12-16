using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour
{
    private List<Item> itemList;
    public List<Item> initialItems;
    
    public UI_Inventory uiInventory;

    void Start()
    {
       // initialItems = new List<Item>();
        itemList = new List<Item>();
        foreach (Item item in initialItems) {
            item.amount = 1;
            AddItem(item);
        }
    }

    void Update()
    {
        
    }

    public void AddItem(Item item)
    {
        this.transform.GetComponent<PlayerController>().message.PushMessage("You obtained " + item.amount + " " + item.itemType.ToString() + "!", 1);
        itemList.Add(item);
        uiInventory.AddToInventoryUI(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}