using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;

public class GameManager : MonoBehaviour
{

    public int stagePoint;
    public int stageIndex;
    public int health;
    public RacoonController player;
    public ItemQuantityController itemQuantity;
    public GameObject[] Stages;
    public TimerController timeCtl;
    public GhostGenerator GhostGen;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;

    GameObject[] items;
    GameObject[] pots;
    GameObject[] enemies;
    public int ItemAmount;
    public int savedScoreInStage;
    Vector3[] savedEnemyPosition = new Vector3[16];

    void Start()
    {
      savedScoreInStage = 0;
      items = GameObject.FindGameObjectsWithTag("Item");
      pots = GameObject.FindGameObjectsWithTag("Pot");
      saveEnemyPosition();
    }

    void Update()
    {
      UIPoint.text = stagePoint.ToString();

      ItemAmount = GameObject.FindGameObjectsWithTag("Item").Length;
      if(ItemAmount == 0) {
        NextStage();
      }
    }

    public void saveEnemyPosition()
    {
      int i = 0;
      enemies = GameObject.FindGameObjectsWithTag("Enemy");
      foreach (GameObject obj in enemies) {
        savedEnemyPosition[i] = obj.transform.position;
        i++;
      }
    }

    public void initFoodQuantities()
    {
      itemQuantity.ItemCount = 0;
      GameObject[] itemQuantities = GameObject.FindGameObjectsWithTag("ItemQuantity2");
      foreach (GameObject obj in itemQuantities) {
        Destroy(obj);
      }
      itemQuantity.gameObject.SetActive(false);
    }
    public void appearAllFood()
    {
      Debug.Log("GameManager.cs - appearAllFood(), 모든 음식들이 다시 나타납니다.");
      
      foreach (GameObject obj in items) {
        obj.SetActive(true);
      }
    }

    public void appearAllPot()
    {
      foreach (GameObject obj in pots) {
        obj.SetActive(true);
      }
    }

    public void NextStage()
    {
      disappearGhost();
      timeCtl.timeLeft = timeCtl.maxTime;
      savedScoreInStage = stagePoint;
      initFoodQuantities();
  
      //Change Stage
      if(stageIndex < Stages.Length - 1) {
        
        Stages[stageIndex].SetActive(false);
        stageIndex++;
        Stages[stageIndex].SetActive(true);
        PlayerReposition();
        saveEnemyPosition(); 

        UIStage.text = "STAGE " + (stageIndex + 1);
        items = GameObject.FindGameObjectsWithTag("Item");
        pots = GameObject.FindGameObjectsWithTag("Pot");
      }
      else
        {  //Game Clear
        //Player Control Lock
        Time.timeScale = 0;
        //Restart Button UI
        //Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
        //btnText.text = "Clear!";
        SceneManager.LoadScene("ClearScene");
      }
    }

    public void HealthDown()
    {
      if(health > 1) {
        timeCtl.timeLeft = timeCtl.maxTime;
        health--;
        UIhealth[health].color = new Color(1, 0, 0, 0.4f);
      }
      else {
        timeCtl.shouldTimerStop = true; 

        //All Health UI Off
        UIhealth[0].color = new Color(1, 0, 0, 0.4f);

        //Player Die Effect
        player.OnDie();
            //Retry Button UI
       SceneManager.LoadScene("GameOverScene");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
      Debug.Log("GameManager.cs - OnTriggerEnter2D, collision: " + collision.ToString() + ", 플레이어가 낭떠러지에 떨어졌습니다!");
      if(collision.gameObject.tag == "Player") {
        restartStage();
      }
    }

    public void restartStage()
    {
        Debug.Log("GameManager.cs - restartStage()");
        stagePoint = savedScoreInStage;
        initFoodQuantities();
        appearAllFood();
        appearAllPot();
        EnemyReposition();

        if(health > 1)
          PlayerReposition();

        HealthDown();
    }

    public void disappearGhost()
    {
      Debug.Log("GameManager.cs - disappearGhost()");
      Destroy(GhostGen.go);
      GhostGen.isAppear = false;
      GhostGen.resetTimer();
    }

    public void EnemyReposition()
    {

      //몬스터 위치 재배치
      int i = 0;
      enemies = GameObject.FindGameObjectsWithTag("Enemy");
      foreach (GameObject obj in enemies) {
        //유령 소멸
        disappearGhost();

        //항아리에 나온 뱀 소멸
        if(obj.name.Contains("Snake")) {
          obj.SetActive(false);
        }

        else {
          obj.transform.position = savedEnemyPosition[i];
          i++;
        }
      }
    }

    public void PlayerReposition()
    {
      player.transform.position = new Vector3(6.5f, 0, -1);
      player.VelocityZero();
    }

    void ViewBtn()
    {
      UIRestartBtn.SetActive(true);
    }

    public void Restart()
    {
      Time.timeScale = 1;
      SceneManager.LoadScene("StartScene");
    }
}
