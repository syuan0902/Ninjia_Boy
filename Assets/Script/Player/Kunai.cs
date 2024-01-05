using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    GameObject g_player;
    Rigidbody2D g_rb;

    float g_live; //飛鏢壽命

    //String target; //Trigger的目標

    public float g_speed;    //飛鏢速度
          //g_kunai_dir; //飛鏢方向

    //講師的飛鏢是透過改變transform.localScale的值來翻轉
    //但我的飛鏢本身朝向就是錯的, 要透過transform.localRotation才能調整到正確位置

    //講師的飛鏢移動是使用g_rb.AddForce, 必須指定施力方向 
    //我是用 g_rb.velocity, 透過改變x軸的值來移動

    void Awake() {
        //透過GameObject.Find("物件名稱")找物件
        g_player = GameObject.Find("Player");

        g_rb = GetComponent<Rigidbody2D>();

        g_speed = 10.0f; //飛鏢速度
        g_live  = 2.0f;  //飛鏢壽命

        //target = "Enemy";

        //透過玩家的面向 決定飛鏢的轉動角度
        if (g_player.transform.localScale.x == 1.0f) {
                     
            //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
             //轉90度
             transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
             g_rb.AddForce(Vector2.right * g_speed, ForceMode2D.Impulse); //Impulse 給一次力量

             //g_kunai_dir = 1.0f;
            
        } else if (g_player.transform.localScale.x == -1.0f) {
            
             //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
             //轉負90度
             transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
             g_rb.AddForce(Vector2.left * g_speed, ForceMode2D.Impulse); //Impulse 給一次力量

             //g_kunai_dir = -1.0f;
        }

        //如果沒碰到目標, N秒後摧毀自己
        Destroy(gameObject, g_live);  //摧毀自己
    }

    // void FixedUpdate() {
    //     g_rb.velocity = new Vector2(g_speed * g_kunai_dir, g_rb.velocity.y);
        
    // }

    void OnTriggerEnter2D(Collider2D other) {

        //飛鏢碰到停止點之外的物件, 就消失
        if (other.tag != "StopPoint")
        {
            Destroy(gameObject);       //摧毀自己
        }

        // if (other.tag == "Enemy")
        // {
        //     Destroy(other.gameObject); //摧毀目標
        //     Destroy(gameObject);       //摧毀自己
        // }

        // if (other.tag == "Ground")
        // {
        //     Destroy(gameObject);       //摧毀自己
        // }         
    }
}
