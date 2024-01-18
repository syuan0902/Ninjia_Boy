using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //控制UI
using UnityEngine.SceneManagement; //控制場景

public class SelectSceneController : MonoBehaviour
{
    public Sprite[] g_btnSprites; //通關後的關卡IMG

    int g_clearLevel; //通關數

    Image g_btnImg1,  g_btnImg2, g_btnImg3; //關卡IMG物件
    
    private void Awake() {

        //取得場上按鈕的圖案
        g_btnImg1 = GameObject.Find("Canvas/SafeAreaPanel/Level1Btn").GetComponent<Image>();
        g_btnImg2 = GameObject.Find("Canvas/SafeAreaPanel/Level2Btn").GetComponent<Image>();
        g_btnImg3 = GameObject.Find("Canvas/SafeAreaPanel/Level3Btn").GetComponent<Image>();

          
          /*
          通關數0 -> 解鎖第1關
          通關數1 -> 解鎖第1,2關
          通關數2 -> 解鎖第1,2,3關
          */

        //根據通過關卡改變選擇按鈕圖案
        //給預設值是避免找不到key值的情境
        g_clearLevel = PlayerPrefs.GetInt("g_clearLevel", 0);  
        
        switch (g_clearLevel)
        {   
            case 0:
                g_btnImg1.sprite = g_btnSprites[0];            
                break;

            case 1:
                g_btnImg1.sprite = g_btnSprites[0];
                g_btnImg2.sprite = g_btnSprites[1];
                break;

            default:
                g_btnImg1.sprite = g_btnSprites[0];
                g_btnImg2.sprite = g_btnSprites[1];
                g_btnImg3.sprite = g_btnSprites[2];
                break;              
        }  
    }

    public void GoToLevel1(){
         //切換到關卡1場景
        SceneManager.LoadScene(2);
    }

     public void GoToLevel2(){
        if (g_clearLevel >= 1)
        {
            //切換到關卡2場景
            SceneManager.LoadScene(3);
        }  
    }

    public void GoToLevel3(){
         
         if (g_clearLevel >= 2)
         {
            //切換到關卡3場景
            SceneManager.LoadScene(4);
         }
    }
}
