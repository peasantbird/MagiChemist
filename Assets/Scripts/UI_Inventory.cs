using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform ItemSlot;
    private Transform ItemTemplate;

    private void Awake() {
        ItemSlot = transform.Find("ItemSlot");
        //ItemTemplate = ItemSlot.Find("ItemTemplate");
    }

    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;
       // RefreshInventoryItems();
    }

    public void AddToInventoryUI(Item item) {
        bool isAdded = false;
        for (int i = 0; i < ItemSlot.childCount; i++) {
            Transform itemTemplate  = ItemSlot.GetChild(i);
            Transform itemContainer = itemTemplate.Find("Item");
            if (itemContainer.GetComponentInChildren<Item>()!=null)
            {
                Item itemTemp = itemContainer.GetComponentInChildren<Item>();
                if (itemTemp.itemType == item.itemType)
                {
                    isAdded = true;
                    itemTemp.amount += item.amount;
                    break;
                }
            }
           
        
        }

        if (!isAdded)
        {
            for (int i = 0; i < ItemSlot.childCount; i++)
            {
                Transform itemTemplate = ItemSlot.GetChild(i);
                Transform itemContainer = itemTemplate.Find("Item");
                if (itemContainer.GetComponentInChildren<Item>() == null)
                {
                    //add the item to the empty container
                    Item itemTemp = Instantiate(item, item.transform.position, Quaternion.identity);
                    itemTemp.name = itemTemp.gameObject.name.Replace("(Clone)", "");
                    itemTemp.transform.parent = itemContainer.transform;
                    itemTemp.transform.localPosition = Vector3.zero;
                    break;
                }
               

            }
        }

    }

    //private void RefreshInventoryItems() {
    //    int x = 0;
    //    int y = 0;
    //    float itemSlotCellSize = 55f;
    //    foreach (Item item in inventory.GetItemList()) {
    //        RectTransform itemSlotRectTransform = Instantiate(ItemTemplate, ItemSlot).GetComponent<RectTransform>();
    //        itemSlotRectTransform.gameObject.SetActive(true);
    //        itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
    //        x++;
    //    }
    //}
}
