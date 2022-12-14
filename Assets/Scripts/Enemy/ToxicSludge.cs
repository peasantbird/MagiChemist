using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicSludge : Enemy
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
        base.MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play sludge voice
    }
}
