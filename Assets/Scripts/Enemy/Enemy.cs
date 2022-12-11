using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum Type // your custom enumeration
    {
        Earth,
        Living,
        Nonliving
    };
    public int enemyIndex;
    public List<int> drops;
    public Type enemyType = new Type();
    public float speed;
    protected TilemapGenerator tileMapGenerator;
    protected Vector2Int targetPos;


    public void InitEnemy() {
        tileMapGenerator = GameObject.Find("Player").GetComponent<TilemapGenerator>();
        targetPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        transform.position = (Vector2)targetPos;
    }

    public virtual void MoveEnemy() {
       //virtual method
    }

  

    public virtual void PlayVoice() { 
     //virtual method
    }

    
}