using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostGenerator : MonoBehaviour
{
    public GameObject GhostPrefab;
    public GameManager gameManager;
    float timer;
    float waitingTime;
    public bool isAppear;
    public GameObject go;
    void Start()
    {
        Debug.Log("GhostGenerator.cs - Start()");
        isAppear = false;
        timer = 0;
        waitingTime = 40;
    }

    public void resetTimer()
    {
        timer = 0;
    }

    void Update()
    {
        //유령 한번 생성했으면 더 이상 생성안해도 됨.
        if(isAppear)
            return;
        Debug.Log("GhostGnerator.cs - timer: " + timer + ", waitingTime: " + waitingTime);

        timer += Time.deltaTime;
        if(timer > waitingTime)
        {
            timer = 0;
            go = Instantiate(GhostPrefab) as GameObject;
            go.transform.parent = GameObject.Find("Stage " + (gameManager.stageIndex + 1).ToString()).transform;
            go.transform.position = new Vector3(5.5f, 1, 0);
            isAppear = true;
        }
    }
}
