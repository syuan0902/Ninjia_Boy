using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other) {
    
        if (other.name == "Player")
        {   
            //避免在每一個場景都設定不同的離開場景腳本
            //動態抓場景名稱作為通關數字

            //取得當前場景的名字
            string l_levelName = SceneManager.GetActiveScene().name;
            //用字串的索引取得當前場景的數字
            //取得第五個字以後的字串也就是Level之後的字串
            string l_levelNum = l_levelName.Substring(5);
            //把字串數字轉為整數數字
            int l_levelTo = int.Parse(l_levelNum); 

            //當場景數字大於原本的場景數字才儲存
            //避免從level2回到level時, level3的按鈕恢復原狀
            int l_levelFrom = PlayerPrefs.GetInt("g_clearLevel");  

            if (l_levelTo > l_levelFrom)
            {
                //儲存通關資訊
                //給一個字串變數作為參數ID
                //再給一個整數值(這裡的整數代表通過的關卡)
                PlayerPrefs.SetInt("g_clearLevel", l_levelTo);
            }

            //切換到選擇關卡場景
            SceneManager.LoadScene(1);
        }

   }
}
