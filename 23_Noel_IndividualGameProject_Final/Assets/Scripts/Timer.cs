using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timerIsRunning = false;
    public Text timeText;
    public TimerBar timerbar;
    public int currentTime;
    public int maxTime = 60;

    // Start is called before the first frame update
    void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
        currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        timerbar.SetMaxTimer(currentTime);

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                maxTime = 0;
                timerIsRunning = false;

                SceneManager.LoadScene("LoseScene");
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("TimeRemaining: {0:00}:{1:00}", minutes, seconds);
    }
}
