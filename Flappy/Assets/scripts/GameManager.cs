using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();//設定一個叫GameDelegate 的委派(delegate)
    public static event GameDelegate OnGameStarted;//在GameDelegate內設置一個開始遊戲的靜態事件
    public static event GameDelegate OnGameOverConfirmed;//在GameDelegate內設置一個遊戲結束確定的靜態事件

    public static GameManager Instance;//創造一個靜態可及性參考，讓其他Scripts從這個Instance進來這個Class
    public GameObject startPage;//開始畫面
    public GameObject gameoverPage;//遊戲結束
    public GameObject countdownPage;//倒數
    public Text scoreText;//分數>>記得用UnityEngune.UI
    enum PageState{
        //總共會有這四種頁面
        None,
        Start,
        GameOver,
        CountDown
    }

    int score= 0;//儲存分數 
    bool gameOver = true;//給一個bool值用來確定遊戲是否結束
    public bool GameOver {get{return gameOver;}}//製造一個可用但無法被其他Script修改的變數
    void Awake(){
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else{
            Instance = this ;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable() {
        CountdwonText.OnCountdownFinished += OnCountdownFinished;
        Tapcontroller.OnPlayerDied += OnPlayerDied;
        Tapcontroller.OnPlayerScored += OnPlayerScored;
    }

    void OnDisable() {
        CountdwonText.OnCountdownFinished -= OnCountdownFinished;
        Tapcontroller.OnPlayerDied -= OnPlayerDied;
        Tapcontroller.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountdownFinished(){
        SetPageState(PageState.None);
        OnGameStarted();//event snet to tapcontroller
        score = 0;
        gameOver= false;
    }

    void OnPlayerDied(){
        gameOver = true;
        int savedScored = PlayerPrefs.GetInt("HighScore");
        if (score > savedScored) {
            PlayerPrefs.SetInt("HighScore",score);
        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored(){
        score++;
        scoreText.text = score.ToString();
    }


    void SetPageState(PageState state){//頁面確認及轉換
        switch(state){
            case PageState.None:
                startPage.SetActive(false);
                gameoverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameoverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameoverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.CountDown:
                startPage.SetActive(false);
                gameoverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }
    public void ConfirmGameOver(){
        //重玩被按下的時候>被附加在重玩鈕上
        OnGameOverConfirmed();//event snet to tapcontroller
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }
    public void StartGame(){
        //開始遊戲>被附加在開始遊戲鈕上
        SetPageState(PageState.CountDown);
    }

}
