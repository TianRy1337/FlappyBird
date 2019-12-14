using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Tapcontroller : MonoBehaviour
{
  public float tapForce = 10 ;
  public float tiltSmooth = 5;
  public Vector3 startPos;
  Rigidbody2D rigidbody;
  Quaternion downRotation;
  Quaternion forwardRotation;
  
    public void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0,0,-90);
        forwardRotation = Quaternion.Euler(0,0,35);
    }
    public void FixedUpdate(){
        if(Input.GetMouseButtonDown(0)){
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce , ForceMode2D.Force);

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "ScoreZone"){
            //計分
        }
        if(col.gameObject.tag =="DeadZone"){
            rigidbody.simulated=false;//死亡
            
        }
    }
    
}
