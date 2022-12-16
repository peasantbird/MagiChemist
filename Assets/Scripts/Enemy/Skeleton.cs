using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public AudioClip ambientVoice;
    public AudioClip hitSound;
    public AudioClip strengthen;
    // Start is called before the first frame update
    void Start()
    {
        base.InitEnemy();

    }

    // Update is called once per frame
    void Update()
    {
        base.UpdateEnemy();
      //  base.MoveEnemy();
    }

    public override void PlayVoice()
    {
        //play skele voice
        base.SFX.PlayOneShot(ambientVoice);
    }
    public override void PlayHitSound()
    {
        //play slime voice
        AudioSource.PlayClipAtPoint(hitSound, new Vector3(transform.position.x, transform.position.y, 0));
    }

    public override void PlayStrengthenSound()
    {
        AudioSource.PlayClipAtPoint(strengthen, new Vector3(transform.position.x, transform.position.y, 0));
    }
    //public override void ReactToElement(Item playerSelectedItem)
    //{
    //    Debug.Log(enemyName + " reacted to " + playerSelectedItem.itemType.ToString());
    //}

    public override void EnemyStartAction()
    {
        base.TakeAction();
    }
}
