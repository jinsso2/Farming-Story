using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePotController : MonoBehaviour
{
    public GameObject snakePrefab;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            appearSnake();
        }
    }

    public void appearSnake()
    {
        Debug.Log("SnakePotController.cs - appearSnake(), 뱀 출현!");
        GameObject go = Instantiate(snakePrefab) as GameObject;
        go.transform.position = gameObject.transform.position;

        go.layer = 9;
        go.tag = "Enemy";

        for(int i = 1; i <= 3; i++) {
            string stageStr = "Stage ";
            stageStr += i.ToString();
            if(GameObject.Find(stageStr)) {
                go.transform.parent = GameObject.Find(stageStr).transform;
            }
        }
    }
}
