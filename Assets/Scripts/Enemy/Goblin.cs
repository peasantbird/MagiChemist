using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin :Enemy
{
    public AudioClip ambientVoice;
    public AudioClip hitSound;
    void Start()
    {
        base.InitEnemy();

    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateEnemy();
       // base.MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play goblin voice
        base.SFX.PlayOneShot(ambientVoice);
    }

    public override void PlayHitSound()
    {
        //play slime voice
        AudioSource.PlayClipAtPoint(hitSound, new Vector3(transform.position.x, transform.position.y, 0));
    }

    public override void EnemyStartAction()
    {
        base.TakeAction();
    }
}
