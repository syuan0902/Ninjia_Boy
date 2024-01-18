using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class SafeAreaPanel : MonoBehaviour
{
    //取得錨點座標
    RectTransform g_panelRect;
    private void Awake() {
        //取得safeArea起點座標(左下角)
        Vector2 l_safeAreaPos = Screen.safeArea.position;
        //取得safeArea最大座標(右上角)
        //size可以取得寬和高
        Vector2 l_safeAreaMaxPos = Screen.safeArea.position + Screen.safeArea.size;

        //錨點位置的值為0~1
        //Screen.safeArea.position取得的座標位置是根據手機解析度決定
        //所以要把取得的座標位置除以手機解析度, 得到錨點的值
        l_safeAreaPos.x  = l_safeAreaPos.x / Screen.width;
        l_safeAreaPos.y  = l_safeAreaPos.y / Screen.height;
        l_safeAreaMaxPos.x = l_safeAreaMaxPos.x / Screen.width;
        l_safeAreaMaxPos.y = l_safeAreaMaxPos.y / Screen.height;

        g_panelRect = GetComponent<RectTransform>();
        //把最小錨點設在安全區域左下角
        g_panelRect.anchorMin = l_safeAreaPos;
         //把最大錨點設在安全區域右上角
        g_panelRect.anchorMax = l_safeAreaMaxPos;

    }
}
