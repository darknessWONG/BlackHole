using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class systemTimer : MonoBehaviour
{
    // 現在の秒数
    private float seconds;
    // 現在の分
    private int minute;

    // タイプ指定 分秒
    public enum E_TimerType
    {
        Index,
        TimerType_S,
        TimerType_M,
        TextType_S,
        TextType_M,
        Max,
    }
    [SerializeField]
    [Header("自分のタイプ")]
    private E_TimerType type;

    // Start is called before the first frame update
    void Start()
    {
        seconds = 0.0f;
        minute = 0;

        switch (type)
        {
            case E_TimerType.TextType_M:
                GetComponent<Text>().enabled = false;
                break;
            case E_TimerType.TimerType_M:
                GetComponent<Text>().enabled = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        seconds += Time.deltaTime;

            switch(type)
            {
                case E_TimerType.TimerType_S:
                    GetComponent<Text>().text = seconds.ToString("F0");
                    if (seconds > 59)
                    {
                        seconds = 0;
                    }
                    break;
                case E_TimerType.TimerType_M:
                    if (seconds > 59)
                    {
                        seconds = 0;
                        minute++;
                        if (!GetComponent<Text>().enabled)
                            GetComponent<Text>().enabled = true;
                    }
                    GetComponent<Text>().text = minute.ToString();
                    break;
                case E_TimerType.TextType_M:
                    if (seconds > 59)
                    {
                        if(!GetComponent<Text>().enabled)
                            GetComponent<Text>().enabled = true;
                    }
                    break;
        }
     }

}
