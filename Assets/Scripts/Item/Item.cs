using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType {
        Silicon,
        Silver,
        Mercury,
        Oxygen,
        Calcium,
        Iron,
        NormalAttack
    }

    public ItemType itemType;
    public int amount;
    public int itemRange;
    private void Start()
    {
        if (itemType == ItemType.NormalAttack) {
            amount = System.Int32.MaxValue;
        }
    }
    private void Update()
    {
        if (amount <= 0) {
            Destroy(this.gameObject);
        }
    }

    public void ReduceAmount(int num) {
        amount -= num;
    }

    //public void TakeEffect(Item item, Enemy enemy) { 
    //if()
    //}
}
