using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public GameManager gameManager;
    Image timerBar;
    public float maxTime;
    public float timeLeft;
    public bool shouldTimerStop = false;

    void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    void Update()
    {
        if(shouldTimerStop) {
            timerBar.fillAmount = 0;
            return;
        }

        if(timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        } else {
            gameManager.restartStage();
            //SceneManager.LoadScene("StartScene");
            timeLeft = maxTime;
        }
    }
}
