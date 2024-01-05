using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//目前問題: 殭屍水平移動可能會走到半空中

public class Zombie_Male : ZombieFemale
{   

    public float g_runSpeed; //殭屍移動速度

    bool isAttackMode;

    protected override void Awake()
    {
        base.Awake();
        isAttackMode = true;
    }

    protected override void MoveAndAttack()
    {
        if (isAlive){

            if (isAttackMode == true){

                //當主角和殭屍的距離小於1.6單位, 進行攻擊
                //要面向主角才攻擊
                if (Vector3.Distance(g_player.transform.position, transform.position) < 4.0f){   

                    //判斷敵人面向
                    if ( transform.position.x <=g_player.transform.position.x ){ //主角在敵人右邊, 右轉
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    } else { //主角在敵人左邊, 左轉
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    }


            //避免攻擊動畫多出現一次
            // if (g_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || 
            //     g_anim.GetCurrentAnimatorStateInfo(0).IsName("AttackWait"))
            // {
            //     return;
            // }

            // g_anim.SetTrigger("Attack");

            //攻擊模式改為衝撞玩家
            //往定點移動
            //給現在位置, 目標位置, 和移動速度
            //避免殭屍的Y軸位置改變(僅能水平移動)
                    Vector3 l_playerPos = new Vector3(g_player.transform.position.x, transform.position.y, transform.position.z);

                    //只有在殭屍走路的時候會追蹤
                    if (g_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")){
                        //播放攻擊音效
                        //g_audio.PlayOneShot(g_clip[0]);
                        //快速衝向主角
                        transform.position = Vector3.MoveTowards(transform.position, l_playerPos, g_runSpeed * Time.deltaTime);
                    }
            
                        isAfterBattle = true;
                        return; //不執行下面的程式, 也就是攻擊的時候不移動

                } else { //主角不在敵人攻擊範圍內, 敵人照原本方向移動

                    if (isAfterBattle) //敵人剛攻擊完主角才會執行
                    {   
                        //如果敵人走向目標點
                        //if (g_turn_pos == g_target_pos)
                        //如果敵人位置離開敵人巡邏範圍
                        if (transform.position.x > g_turn_pos.x || transform.position.x < g_turn_pos.x) {

                            //判斷敵人的面向
                            //如果敵人跑到巡邏範圍的右邊
                            //面向左
                            if (transform.position.x > g_turn_pos.x) {
                                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

                                //如果敵人跑到巡邏範圍的左邊
                                //面向右
                            } else if (transform.position.x < g_turn_pos.x) {
                                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                            }

                        } else  {
                            //終點等於轉折點
                            if (g_turn_pos == g_target_pos){
                                //延後轉身的時間
                                //左轉
                                StartCoroutine(TurnLeft(true)); 
                                //起點等於轉折點 
                            } else if(g_turn_pos == g_origin_pos) {  
                                //右轉
                                StartCoroutine(TurnLeft(false));                
                            }
                        }  
                            isAfterBattle = false;
                    } 
                }
                
            } else {  //不是攻擊模式

                //改變面向
                if (transform.position.x > g_turn_pos.x) //敵人在轉折點右邊, 左轉
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

                } else //敵人在轉折點左邊, 右轉
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                //敵人在走路動畫播放時才移動
                if (g_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    //往轉折點移動
                    transform.position = Vector3.MoveTowards(transform.position, g_turn_pos, g_speed * Time.deltaTime);
                }
            
                //走回轉折點時, 開啟攻擊模式
                if (transform.position == g_turn_pos) {
                    isAttackMode = true;
                }

                return; //不執行下面程式碼
            }
            

            //當殭屍移動到目標位置
            if (transform.position.x == g_target_pos.x) {
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

                if (!isFirstIldle) {
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
            if (g_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")){
                //往定點移動
                //給現在位置, 目標位置, 和移動速度
                transform.position = Vector3.MoveTowards(transform.position, g_turn_pos, g_speed * Time.deltaTime);
        
            } 
        }    
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);

        if (other.tag == "StopPoint"){
            isAttackMode = false;
        }

        //讓殭屍被攻擊時, 轉換為攻擊模式
        if (other.tag == "PlayerAttack")
        {
            isAttackMode = true;
        }
    }
}
