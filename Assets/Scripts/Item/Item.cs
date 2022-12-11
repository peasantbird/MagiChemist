using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType {
        Silicon,
        Silver,
        Mercury,
        Oxygen,
        Calcium,
        Iron
    }

    public ItemType itemType;
    public int amount;
}
