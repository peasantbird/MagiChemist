using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpirit : Enemy
{
    public AudioClip ambientVoice;
    public AudioClip hitSound;
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

    //public override void ReactToElement(Item playerSelectedItem)
    //{
    //    Debug.Log(enemyName + " reacted to " + playerSelectedItem.itemType.ToString());
    //}
}
