using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectPanelController : MonoBehaviour
{
   public GameObject g_selectPanel,
                     g_PauseBtn;

   public void SetSelectPanelOn(){
        g_selectPanel.SetActive(true);
        Time.timeScale = 0f;
   }

    public void SetSelectPanelOff(){
        g_selectPanel.SetActive(false);
        Time.timeScale = 1.0f;
   }

     public void SetPauseBtnOn(){
        g_PauseBtn.SetActive(true);    
   }

    public void SetPauseBtnOff(){
        g_PauseBtn.SetActive(false);
   }

     //按下Play鍵
      public void MainMenuPlayBtnPress(){
          //取得場上的主角物件
          GameObject l_mainMenuPlayer = GameObject.Find("MainMenuPlayer");
          //取得主角物件的動畫元件
          Animator l_playerAnim = l_mainMenuPlayer.GetComponent<Animator>();
          l_playerAnim.SetBool("Run", true);

          //取得Play按鈕物件
          GameObject l_playBtn = GameObject.Find("Canvas/SafeAreaPanel/PlayButton");
          //Play按鈕消失
          l_playBtn.SetActive(false);
          //切換到選擇關卡場景
          SceneManager.LoadScene(1);
   }
}
