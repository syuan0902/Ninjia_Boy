using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonCollider : MonoBehaviour
{

   Player g_player;
   // Start is called before the first frame update
   void Awake()
   {
      //取得父物件(主角)的參考
      g_player = GetComponentInParent<Player>();
   }

   //用子物件控制父物件的動畫元件和跳躍變數
   void OnTriggerEnter2D(Collider2D other)
   {

      if (other.tag == "Ground")
      {
         g_player.canJump = true;
         g_player.g_anim.SetBool("Jump", false);
      }

      //爬上移動平台
      if (other.tag == "MovingPlatform")
      {
         g_player.canJump = true;
         g_player.g_anim.SetBool("Jump", false);
         //讓平台變成主角的父物件, 主角才能跟著平台移動
         g_player.transform.parent = other.transform;
      }
   }
   //離開移動平台
   private void OnTriggerExit2D(Collider2D other)
   {
      //解除主角的父物件
      if (other.tag == "MovingPlatform")
      {
         g_player.transform.parent = null;
      }
   }
}
