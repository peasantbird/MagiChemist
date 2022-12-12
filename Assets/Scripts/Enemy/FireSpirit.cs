using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpirit : Enemy
{
    void Start()
    {
        base.InitEnemy();

    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateEnemy();
        base.MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play fire voice
    }
}
