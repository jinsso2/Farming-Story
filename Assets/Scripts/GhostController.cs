using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public float speed;
    public float distance;
    Transform player;
    Rigidbody2D rigid;
    void Start()
    {
        player = GameObject.Find("Racoon").transform;
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rigid.velocity = Vector3.zero;

        if (Mathf.Abs(transform.position.x - player.position.x) > distance)
        {
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime);
        }
        DirectionGhost();
    }

    void DirectionGhost()
    {
        if(transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        
    }
}
