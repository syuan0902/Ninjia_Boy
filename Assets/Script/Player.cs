using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float g_speed,    //左右移動速度
                 g_upspeed;  //向上移動速度
  
    [HideInInspector]  //在Inspector 不顯示       
    public Animator g_anim;      //動畫元件

    Rigidbody2D g_rb; //鋼體元件

    [HideInInspector] 
    public bool isJumpPressed, //按下跳按鈕
                canJump;  //可以跳

    //程式執行第一個被CALL的函式
    void Awake() {
        //取得動畫元件
        g_anim = GetComponent<Animator>();
        g_rb   = GetComponent<Rigidbody2D>();

        g_speed   = 5.0f;
        g_upspeed = 20.0f ;

        isJumpPressed = false;
        canJump       = true;
    }
    
    //判斷按鈕有沒按 不適合放在FixedUpdate
    void Update() {
         if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            isJumpPressed = true;  //按下空白鍵, 跳 
            canJump       = false;
        }
    }
   
    // 使用鋼體移動要搭配Fixed Update
    void FixedUpdate() {

        //左右移動
        //按D為0.0~1.0; 按A為0.0~-1.0 
        //float Input_X = Input.GetAxis("Horizontal");
        float Input_X = Input.GetAxisRaw("Horizontal");

        //上下移動
        //按W為0.0~1.0; 按S為0.0~-1.0 
        // float Input_Y = Input.GetAxisRaw("Vertical");

        //根據移動方向決定腳色面向
        if(Input_X < 0){

            transform.localScale = new Vector3(-1f, 1f, 1f);

        } else if(Input_X > 0){    

            transform.localScale = new Vector3(1f, 1f, 1f);

        }

        //設定動畫切換機制
        //絕對值大於0.1才會切換Run
        //切換動畫跟著上下 or 左右移動的邏輯
        // if (Mathf.Abs(Input_X) > 0.1f && Input_Y == 0.0f){                   //左右移動

            g_anim.SetFloat("Run", Mathf.Abs(Input_X));

        // } else if (Mathf.Abs(Input_Y) > 0.1f && Input_X == 0.0f){            //上下移動

        //     g_anim.SetFloat("Run", Mathf.Abs(Input_Y));

        // } else if (Mathf.Abs(Input_X) > 0.1f && Mathf.Abs(Input_Y) > 0.1f){  // 斜角移動
           
        //     g_anim.SetFloat("Run", Mathf.Abs(Input_X));

        // } else {                                                             //閒置
           
        //     g_anim.SetFloat("Run", 0.0f);
        // }

        //改變移動速度
        //Time.deltaTime 是 for transform
        //fixedDeltaTime 是 for 鋼體
        //Input_X *= Time.fixedDeltaTime * g_speed; 
        // Input_Y *= Time.fixedDeltaTime * g_speed; 

        //改變腳色位置
        // transform 是唯一一個不需要GetComponent就能使用的元件
        // transform.position = new Vector3( transform.position.x + Input_X, 
        //                                   transform.position.y + Input_Y,
        //                                   transform.position.z );
        //改成用鋼體移動
        // g_rb.position = new Vector2( g_rb.position.x + Input_X, 
        //                              g_rb.position.y + Input_Y ); 
        //改成用鋼體的Velocity移動
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     g_rb.AddForce(Vector2.up * g_upspeed, ForceMode2D.Impulse );  //施力方向, 給一次力量
        // }

        if (isJumpPressed)
        {
             g_rb.AddForce(Vector2.up * g_upspeed, ForceMode2D.Impulse );  //施力方向, 給一次力量
             g_anim.SetBool("Jump", true);
             isJumpPressed = false;        
        } 

        g_rb.velocity = new Vector2(Input_X * g_speed, g_rb.velocity.y);
        
    }

    //判斷玩家是否碰到地板
    //改成用子物件的Trigger控制
    // void OnCollisionEnter2D(Collision2D other) {

    //     //Bug: 碰到牆壁就不會切換成Jump動畫
    //     if (other.collider.tag == "Ground")
    //     {
    //         canJump = true;
    //         g_anim.SetBool("Jump", false);
    //     }
        
    // }

}
