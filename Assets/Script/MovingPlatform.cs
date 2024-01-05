using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 g_targetPos;
    Vector3 g_turnPos, g_originPos;
    public float g_speed;   // Start is called before the first frame update

    private void Awake() {
        //紀錄起始位置
        //把終點設為轉折點
        g_originPos = transform.position;
        g_turnPos   = g_targetPos;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, g_turnPos, g_speed * Time.deltaTime);
        //如果移動到終點
        //把起點設為轉折點
        if (transform.position == g_targetPos)
        {
            g_turnPos = g_originPos;

        //如果移動到起點
        //把終點設為轉折點
        } else if (transform.position == g_originPos)
        {
            g_turnPos = g_targetPos;
        }
    }
}
