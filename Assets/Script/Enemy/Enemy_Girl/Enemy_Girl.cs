using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//敵人音效有雜音

public class Enemy_Girl : MonoBehaviour
{
    bool isAlive,      //活著
         isIdle,       //閒置中
         isJumpAttack, //跳躍攻擊
         isJumpUp,     //往上跳
         isSlideAttack,  //滑行攻擊
         isHurt,         //受傷中
         canBeHurt;      //可以被攻擊
         
    SpriteRenderer g_sr;             
                 
    Vector3 g_pos; //滑行時的主角位置
    BoxCollider2D g_col;  //敵人的碰撞體

    public GameObject g_atkCollider,
                      g_slideCollider; //滑行攻擊
    GameObject g_player; //主角
    Animator   g_anim;   //敵人動畫

    AudioSource g_audio;
    public AudioClip[] g_audioClip;

    public int   g_health; //敵人血量
    public float g_distance, //敵人與主角距離
                 g_jumpUpHeight,   //往上跳的高度
                 g_jumpDownHeight, //往下降的高度  
                 g_slideSpeed, //滑行速度
                 g_jumpUpSpeed,   //跳上速度
                 g_jumpDownSpeed,   //跳下速度
                 g_fallDownSpeed;  //墜落速度

    private void Awake() {

        isAlive       = true;   //活著
        isIdle        = true;   //閒置中
        isJumpAttack  = false;  //跳躍攻擊
        isJumpUp      = true;   //往上跳
        isSlideAttack = false;  //滑行攻擊
        isHurt        = false;  //受傷
        canBeHurt     = true;   //可以被攻擊
        
        g_player = GameObject.Find("Player");  //主角
        g_anim   = GetComponent<Animator>();   //敵人動畫
        g_col    = GetComponent<BoxCollider2D>(); //敵人碰撞體  
        g_sr     = GetComponent<SpriteRenderer>(); //敵人渲染器   
        g_audio  = GetComponent<AudioSource>(); //音效  
        
    }


    // Update is called once per frame
    void Update()
    {
        if (isAlive)  //活著才有動作
        {   
            if (isIdle) //閒置中
            { 
                LookAtPlayer(); //面向玩家

                //離主角近就滑行攻擊
                if (Vector3.Distance(transform.position, g_player.transform.position) <= g_distance)
                {
                    //滑行攻擊
                    isIdle       = false;
                    StartCoroutine("IdleToSlideAttack");                 

                } else {

                    //跳躍攻擊
                    isIdle       = false;
                    StartCoroutine("IdleToJumpAttack");
                    
                }
                
            } else if (isSlideAttack) {
 
                //滑行攻擊動畫
                g_anim.SetBool("Slide", true);

                transform.position = Vector3.MoveTowards(transform.position, g_pos, g_slideSpeed * Time.deltaTime);    
                //播放滑行攻擊音效
                //g_audio.PlayOneShot(g_audioClip[1]); 

                //移動到主角的位置就回到閒置狀態
                //敵人碰撞體恢復原狀
                if (transform.position == g_pos)
                {   
                    g_col.offset =  new Vector2(0.02802822f, -0.1494832f);
                    g_col.size   =  new Vector2(0.7053221f, 1.701034f); 
                    
                    g_anim.SetBool("Slide", false);
                    isIdle        = true;
                    isSlideAttack = false; 
                }

                

                
            } else if (isHurt){  //受傷時要回到地面
                
                    //設定兩個高度
                    //如果玩家高度 < 二樓, 敵人下落到高度一
                    //如果玩家高度 >= 二樓, 敵人下落到高度二
                    if (g_player.transform.position.y < -0.059f)
                    {
                        g_jumpDownHeight = -3.1f;

                    } else {

                        g_jumpDownHeight = 0.08f;

                    }  

                Vector3 l_pos = new Vector3(transform.position.x, g_jumpDownHeight, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, l_pos, g_fallDownSpeed * Time.deltaTime);

            } else if (isJumpAttack) { //跳躍攻擊

                LookAtPlayer(); //面向玩家

                //往上跳
                if (isJumpUp)
                {
                    //往上跳的目標位置
                    Vector3 l_target 
                        = new Vector3(g_player.transform.position.x,  g_jumpUpHeight, transform.position.z); 
                    
                    //跳到主角上方
                    transform.position = Vector3.MoveTowards(transform.position, l_target, g_jumpUpSpeed * Time.deltaTime);

                    //往上跳動畫
                    g_anim.SetBool("JumpUp", true);

                    
                } else {  //往下跳

                    g_anim.SetBool("JumpUp", false);

                    //設定兩個高度
                    //如果玩家高度 < 二樓, 敵人下落到高度一
                    //如果玩家高度 >= 二樓, 敵人下落到高度二
                    if (g_player.transform.position.y < -0.059f)
                    {
                        g_jumpDownHeight = -3.1f;

                    } else {

                        g_jumpDownHeight = 0.08f;

                    }  

                    //往下跳
                    Vector3 l_target 
                        = new Vector3(transform.position.x,  g_jumpDownHeight, transform.position.z);  

                    //跳到正下方
                    transform.position = Vector3.MoveTowards(transform.position, l_target, g_jumpDownSpeed * Time.deltaTime);  

                    //播放攻擊音效
                    //g_audio.PlayOneShot(g_audioClip[0]);  

                    //往下跳動畫
                    g_anim.SetBool("JumpDown", true);

                }

                //判斷敵人到指定高度之後往下跳
                if (transform.position.y == g_jumpUpHeight)
                {
                    isJumpUp = false;

                } else if (transform.position.y == g_jumpDownHeight) //敵人在地面高度
                {
                    //回到閒置狀態
                    isJumpAttack = false;  //跳躍攻擊

                    //讓敵人落到地面時, 先保持原本姿勢, 之後才變Idle
                    //作法: 延後執行 變成Idle的程式
                    StartCoroutine("JumpDownIdle");

                }              
            }        
        } else {

                Vector3 l_pos = new Vector3(transform.position.x, g_jumpDownHeight, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, l_pos, g_fallDownSpeed * Time.deltaTime);
        }   
    }

     //要在受傷的第一個Frame呼叫這個函式, 關閉攻擊collider
    public void SetIsHurt(){
        
        g_atkCollider.SetActive(false);
        g_slideCollider.SetActive(false);

    }

 
    
    //跳躍攻擊前等待
    IEnumerator IdleToJumpAttack(){
        yield return new WaitForSeconds(1.0f); 
        isJumpAttack = true;
    }

    //滑行攻擊前等待
    IEnumerator IdleToSlideAttack(){

        yield return new WaitForSeconds(1.0f); 
        //降低碰撞體高度
        g_col.offset =  new Vector2(0.02802822f, -0.4201604f);
        g_col.size   =  new Vector2(0.7053221f, 1.159679f); 

        //移動到主角位置
        g_pos = new Vector3(g_player.transform.position.x, transform.position.y, transform.position.z);
        LookAtPlayer(); //面向玩家
        isSlideAttack = true;
    }

    //落地後等待
    IEnumerator JumpDownIdle(){

        yield return new WaitForSeconds(0.5f); 

        isIdle       = true;   //閒置
        isJumpUp     = true;   //可以往上跳
        g_anim.SetBool("JumpUp", false);
        g_anim.SetBool("JumpDown", false);

    }

    //受傷0.5秒後恢復
    //2秒無敵
     IEnumerator SetAnimHurtToFalse(){

        yield return new WaitForSeconds(0.5f); 

        g_anim.SetBool("Hurt", false);
        //避免敵人恢復後出現跳&滑的動畫
        g_anim.SetBool("JumpUp", false);
        g_anim.SetBool("JumpDown", false);
        g_anim.SetBool("Slide", false);
        g_sr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f); //無敵時呈現半透明
        
        isHurt        = false;
        isIdle        = true;

        yield return new WaitForSeconds(2.0f); 
        canBeHurt = true;
        g_sr.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f); //恢復顏色

    }
    
    //面向玩家
    void LookAtPlayer(){

        //改變敵人面向
        if (transform.position.x > g_player.transform.position.x)
        {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

        } else
        {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

      //啟動Attack Collider
    public void SetAttackColliderOn(){
        g_atkCollider.SetActive(true);
    }

     //關閉Attack Collider
    public void SetAttackColliderOff(){
        g_atkCollider.SetActive(false);
    }

    //啟動Slide Collider
    public void SetSlideColliderOn(){
        g_slideCollider.SetActive(true);
    }

     //關閉Slide Collider
    public void SetSlideColliderOff(){
        g_slideCollider.SetActive(false);
    }

    //被主角攻擊
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "PlayerAttack")
        {   
            if (canBeHurt)
            {
                g_audio.PlayOneShot(g_audioClip[2]);

                 g_health --;

                 if (g_health > 0)
                {
                    isIdle        = false;   //閒置中
                    isJumpAttack  = false;  //跳躍攻擊
                    isSlideAttack = false;  //滑行攻擊
                    isHurt        = true;

                    StopCoroutine("JumpDownIdle");
                    StopCoroutine("IdleToSlideAttack");
                    StopCoroutine("IdleToJumpAttack");

                    g_anim.SetBool("Hurt", true);
            
                    StartCoroutine("SetAnimHurtToFalse");

                } else {

                    isAlive = false;
                    StopAllCoroutines();
                    g_anim.SetBool("Die", true);
                    g_col.enabled = false;
                    //把Collider物件摧毀, 避免和玩家碰撞
                    Destroy(g_atkCollider);
                    Destroy(g_slideCollider);

                    
                }

                canBeHurt = false;
            }
        }
    }
}
