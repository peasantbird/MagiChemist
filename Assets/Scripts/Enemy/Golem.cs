using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.InitEnemy();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play golem voice
    }

    

}
