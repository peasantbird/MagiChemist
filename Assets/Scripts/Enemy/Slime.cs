using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
   // Start is called before the first frame update
    void Start()
    {
        base.InitEnemy();
        base.enemyMovableTiles = new int[] {0, 2, 4}; // Slimes cannot move on sand, so 3 is omitted

    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateEnemy();
       // base.MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play slime voice
    }
    public override void EnemyStartAction()
    {
        base.TakeAction();
    }

}
