using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.InitEnemy();
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.MoveEnemy();
    }
}
