using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFemale : MonoBehaviour
{
    public Vector3 g_target_pos; //目標位置
    Vector3 g_origin_pos, //起始位置
            g_turn_pos;   //折返位置
    public float g_speed;

    Animator g_anim;

    bool isFirstIldle; //第一次Idle

    private void Awake() {
        g_anim = GetComponent<Animator>();
        //紀錄殭屍原本的位置
        g_origin_pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isFirstIldle = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //當殭屍移動到目標位置
        if (transform.position.x == g_target_pos.x)
        {
            //播放Idle動畫
            g_anim.SetTrigger("Idle");
            //把初始位置當做新的目標位置
            g_turn_pos = g_origin_pos; 
            //延後轉身的時間
            StartCoroutine(TurnLeft(true));
            //不是第一次Idle
            isFirstIldle = false; 

        //當殭屍移動到初始位置
        } else if(transform.position.x == g_origin_pos.x){

            if (!isFirstIldle)
            {
                 //播放Idle動畫
                g_anim.SetTrigger("Idle");
            }

             //把目標位置當做新的目標位置
            g_turn_pos = g_target_pos; 
            //延後轉身的時間
            StartCoroutine(TurnLeft(false));
         
        }

        //播放移動動畫時才開始動
        // Animator的Base Layer就是 0
        if (g_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
        //往定點移動
        //給現在位置, 目標位置, 和移動速度
        transform.position = Vector3.MoveTowards(transform.position, g_turn_pos, g_speed * Time.deltaTime);
        }
        

    }

    //延後轉身的時間
    IEnumerator TurnLeft(bool trunLeft){
        yield return new WaitForSeconds(2.0f) ; 
        //殭屍轉向
        if (trunLeft) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        } else if (!trunLeft) {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }      
    }
}
