using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonCollider : MonoBehaviour
{

    Player g_playerScript;
    // Start is called before the first frame update
    void Awake()
    {
       g_playerScript = GetComponentInParent<Player>();     
    }
   
   //用子物件控制父物件的動畫元件和跳躍變數
   void OnTriggerEnter2D(Collider2D other) {

    if (other.tag == "Ground")
    {
         g_playerScript.canJump = true;
         g_playerScript.g_anim.SetBool("Jump", false);
    }   
   }
}
