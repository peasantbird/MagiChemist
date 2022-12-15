using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpirit : Enemy
{
    void Start()
    {
        base.InitEnemy();
        base.enemyMovableTiles = new int[] {0, 3, 4}; // Fire spirit cannot move on water, so 2 is omitted
    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateEnemy();
       // base.MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play fire voice
    }
    public override void EnemyStartAction()
    {
        base.TakeAction();
    }
}
