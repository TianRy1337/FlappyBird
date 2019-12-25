using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
//RequireComponent指這個類一定要那些組件，沒有這些組件沒有被加上，就自動補上
public class Tapcontroller : MonoBehaviour
//MonoBehaviour是Unity的整合開發環境
{
    public delegate void PlayerDelegate(); //設定一個叫PlayerDelegate 的委派(delegate)
    public static event PlayerDelegate OnPlayerDied;//在PlayerDelegate內設置一個死亡的靜態事件
    public static event PlayerDelegate OnPlayerScored;//在PlayerDelegate內設置一個得分的靜態事件

    public float tapForce = 10 ;//點擊力量
    public float tiltSmooth = 5;//傾斜時的平滑
    public Vector3 startPos;//設置三維座標 叫 starPos
    Rigidbody2D newrigidbody;//設置一個Rigidbody變數叫newridigbody
    Quaternion downRotation;//設置一個四元數的變數叫downRotation
    Quaternion forwardRotation;//設置一個四元數的變數叫forwarRotation

    GameManager game;
  
    public void Start() {//遊戲開始時執行
        newrigidbody = GetComponent<Rigidbody2D>();//抓到該物件的Rigidbody2D ,回傳附加型態，如果沒有就傳Null
        downRotation = Quaternion.Euler(0,0,-90);//返回圍繞z軸-90的旋轉
        forwardRotation = Quaternion.Euler(0,0,35);//返回圍繞z軸35的旋轉
        game = GameManager.Instance;
        newrigidbody.simulated=false;//關閉該物件的Rigidbody2D
    }

    
    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted(){
        newrigidbody.velocity = Vector3.zero;
        newrigidbody.simulated = true;
    }


    void OnGameOverConfirmed(){
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }   

    public void FixedUpdate(){
        if(game.GameOver)return;
        if(Input.GetMouseButtonDown(0)){//如果按下滑鼠左鍵 0=左鍵 1=右鍵 2=中鍵
            transform.rotation = forwardRotation;//每次點擊鳥的弧度回到原位
            newrigidbody.velocity = Vector3.zero;//將速度重新為0 否則重力速度會大於點擊速度
            newrigidbody.AddForce(Vector2.up * tapForce , ForceMode2D.Force);//每次點擊都能向上

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);//鳥沿著Z軸迴轉的滑順度
    }
//設置一個Function負責判斷是否擊中Scorezone跟DeadZone
    public void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "ScoreZone"){
            //計分
            OnPlayerScored();// event sent to GameManager;
        }
        if(col.gameObject.tag =="DeadZone"){
            newrigidbody.simulated=false;//關閉碰撞
            OnPlayerDied();
            
        }
    }
    
}
