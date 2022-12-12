using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin :Enemy
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
        //play goblin voice
    }
}
