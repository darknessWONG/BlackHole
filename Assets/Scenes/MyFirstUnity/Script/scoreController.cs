using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreController : MonoBehaviour
{

    private int score;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GetComponent<gameController>().gameObject;
        //score = go.Score;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = score.ToString();
    }
}
