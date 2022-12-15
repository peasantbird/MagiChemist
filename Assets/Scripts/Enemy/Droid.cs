using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droid : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.InitEnemy();
        
    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateEnemy();
        //base.MoveEnemyThroughWalls();
    }

    public override void PlayVoice()
    {
        //play droid voice
    }

    public override void EnemyStartAction()
    {
        base.TakeAction();
    }



}
