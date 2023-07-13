using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;
    public AudioSource tapSound;
    public AudioSource scoreSound;
    public AudioSource dieSound;

    Rigidbody2D thisRigidBody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    GameManager game;

    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -100);
        forwardRotation = Quaternion.Euler(0, 0, 40);
        game = GameManager.Instance;
        thisRigidBody.simulated = false;
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

    void OnGameStarted()
    {
        thisRigidBody.velocity = Vector3.zero;
        thisRigidBody.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (game.GameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            thisRigidBody.velocity = Vector2.zero;
            transform.rotation = forwardRotation; //每次點擊鳥的弧度回到原位
            thisRigidBody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            tapSound.Play();
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("ScoreZone"))
        {
            OnPlayerScored();
            scoreSound.Play();
        }
        if (col.gameObject.CompareTag("DeadZone"))
        {
            thisRigidBody.simulated = false;
            OnPlayerDied();
            dieSound.Play();
        }
    }

}