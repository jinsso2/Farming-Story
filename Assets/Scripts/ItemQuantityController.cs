using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemQuantityController : MonoBehaviour
{
    public int ItemCount = 0;
    void Update()
    {
        gameObject.GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Item").GetComponent<SpriteRenderer>().sprite;      
    }
    public void UpdateItemQuantity()
    {
        if(ItemCount > 1) {
            GameObject go = Instantiate(gameObject) as GameObject;
            go.tag = "ItemQuantity2";
            go.transform.parent = GameObject.Find("Canvas").transform;
            
            Vector3 pos = gameObject.transform.position;
            pos.y += 55 * (ItemCount - 1);
            go.transform.position = pos;
        }
    }
}
