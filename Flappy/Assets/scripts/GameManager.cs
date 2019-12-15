using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;
    public GameObject startPage;
    public GameObject gameoverPage;
    public GameObject countdownPage;
    public Text scoreText;
    enum PageState{
        None,
        Start,
        GameOver,
        CountDown
    }

    int score= 0;
    bool gameOver = true;
    public bool GameOver {get{return gameOver;}}
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


    void SetPageState(PageState state){
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
        //重玩被按下的時候
        OnGameOverConfirmed();//event snet to tapcontroller
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }
    public void StartGame(){
        //開始遊戲
        SetPageState(PageState.CountDown);
    }

}
