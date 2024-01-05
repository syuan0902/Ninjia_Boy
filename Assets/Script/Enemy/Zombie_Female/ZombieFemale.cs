using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFemale : MonoBehaviour
{
    public int g_enemyHealth;

    [SerializeField] 
    public Vector3 g_target_pos; //目標位置
     [SerializeField] 
    protected Vector3 g_origin_pos, //起始位置
            g_turn_pos;   //折返位置
    public float g_speed;

    protected Animator g_anim;
    protected BoxCollider2D g_enemyCol; //敵人的collider
    protected SpriteRenderer g_sr; //敵人的Renderer

    public GameObject g_atkCollider;

    protected GameObject g_player; //取得主角位置
    protected float g_attack_dis; //攻擊距離

     //讓元件出現在Inspector
     [SerializeField] 
    protected AudioClip[] g_clip;

    protected AudioSource g_audio;
    
    protected bool isFirstIldle,  //第一次攻擊
        isAfterBattle,  //剛攻擊完
        isAlive; //敵人還活著
    protected virtual void Awake() {
        g_anim = GetComponent<Animator>();
        //紀錄殭屍原本的位置
        g_origin_pos  = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isFirstIldle  = true;
        isAfterBattle = false;

        g_attack_dis  = 1.3f;
        //g_enemyHealth = 3;
        isAlive       = true;
        
         g_player   = GameObject.Find("Player");       //取得玩家位置
         g_enemyCol = GetComponent<BoxCollider2D>();   //取得敵人的碰撞體
         g_sr       = GetComponent<SpriteRenderer>();  //取得敵人的渲染器
         g_audio    = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {  
        MoveAndAttack();  //敵人移動和攻擊
    }


    //敵人移動和攻擊
    //加上protected virtual讓人可繼承, 並修改方法
    protected virtual void MoveAndAttack(){

        if (isAlive)
        {
            //當主角和殭屍的距離小於1.6單位, 進行攻擊
            //要面向主角才攻擊
            if (Vector3.Distance(g_player.transform.position, transform.position) < g_attack_dis) {   

                // //判斷敵人面向
                // if ( g_player.transform.position.x <= transform.position.x) {
                //     transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                // } else {
                //     transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                // }

                //判斷敵人面向
                if ( transform.position.x <=g_player.transform.position.x ) { //主角在敵人右邊
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                } else { //主角在敵人左邊
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }


                //避免攻擊動畫多出現一次
                if (g_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || 
                    g_anim.GetCurrentAnimatorStateInfo(0).IsName("AttackWait")) {
                    return;
                }
                    //播放攻擊音效
                g_audio.PlayOneShot(g_clip[0]);
                g_anim.SetTrigger("Attack");
                isAfterBattle = true;
                return; //不執行下面的程式, 也就是攻擊的時候不移動

            } else { //主角不在敵人攻擊範圍內, 敵人照原本方向移動

                if (isAfterBattle) //敵人剛攻擊完主角才會執行
                {
                    //如果敵人走向目標點
                    if (g_turn_pos == g_target_pos) {
                        //延後轉身的時間
                        StartCoroutine(TurnLeft(false));         
                    } else if(g_turn_pos == g_origin_pos) {  //敵人走向原點
                        StartCoroutine(TurnLeft(true));                
                    }
                    
                    isAfterBattle = false;
                } 
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
            } else if(transform.position.x == g_origin_pos.x) {

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
            if (g_anim.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                //往定點移動
                //給現在位置, 目標位置, 和移動速度
                transform.position = Vector3.MoveTowards(transform.position, g_turn_pos, g_speed * Time.deltaTime);
            } 
        }    
    }

    //延後轉身的時間
    protected IEnumerator TurnLeft(bool trunLeft){
        yield return new WaitForSeconds(2.0f) ; 
        //殭屍轉向
        if (trunLeft) {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        } else if (!trunLeft) {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }      
    }
    
    //敵人受傷
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        
        if (other.tag == "PlayerAttack")
        {   
             //播放受傷音效
            g_audio.PlayOneShot(g_clip[1]);
            g_enemyHealth --;

            if (g_enemyHealth > 0)
            {
                g_anim.SetTrigger("Hurt");
                
            } else {
                g_enemyCol.enabled = false; //關掉敵人的collider
                isAlive            = false; //敵人不會動
                g_anim.SetTrigger("Die");
                StartCoroutine("AfterDie"); //敵人死亡後變透明, 消失
                
                //敵人消失?
            }
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

    //敵人死亡後變透明, 消失
    IEnumerator AfterDie(){
        yield return new WaitForSeconds(1.0f); 
        //因為敵人受傷時, 已經有修改渲染器的阿法值, 所以就不能再用 SpriteRenderer.color這個API
        //g_sr.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);  //敵人死亡後變透明, 
        g_sr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);  //敵人死亡後變透明, 

        yield return new WaitForSeconds(1.0f); 
        g_sr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);  //敵人死亡後變透明, 

        yield return new WaitForSeconds(1.0f);                    //敵人消失, 
        Destroy(gameObject);
    }
}
