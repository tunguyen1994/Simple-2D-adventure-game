using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOne : MonoBehaviour
{

    [SerializeField]
    float speed;

    
    public double damage;

    [SerializeField]
    float timeToDestory = 3;

    public static BulletOne instance;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void StartShoot(bool isFacingLeft)
    {
        Rigidbody2D bullet = GetComponent<Rigidbody2D>();

        if (isFacingLeft)
        {
            bullet.velocity = new Vector2(-speed, 0);
        }
        else
        {
            bullet.velocity = new Vector2(speed, 0);
        }

        Destroy(gameObject, timeToDestory);
        
    }

}
