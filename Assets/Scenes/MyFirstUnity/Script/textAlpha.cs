using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textAlpha : MonoBehaviour
{
    public Text Qtext;
    private float addAlpha;
    private bool isAddAlpha;
    // Start is called before the first frame update
    void Start()
    {
        addAlpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //テキストの透明度を変更する
        Qtext.color = new Color(255, 255, 255, addAlpha);
        if (isAddAlpha)
        {
            addAlpha += Time.deltaTime;
        }   
        else
        {
            addAlpha -= Time.deltaTime;
        }
         
        if (addAlpha < 0)
        {
            addAlpha = 0;
            isAddAlpha = true;
        }
        else if (addAlpha > 1)
        {
            addAlpha = 1;
            isAddAlpha = false;
        }
    }

}
