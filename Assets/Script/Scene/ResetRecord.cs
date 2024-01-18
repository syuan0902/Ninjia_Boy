using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRecord : MonoBehaviour
{
    private void Awake() {
        //刪除紀錄
        PlayerPrefs.DeleteAll();
    }
}
