using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXAnimation : MonoBehaviour
{
    public AnimationClip clip;
    public bool loop;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play(clip.name);
        if (!loop)
        {
            StartCoroutine(DestroyAfter(clip.length));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyAfter(float time) {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
