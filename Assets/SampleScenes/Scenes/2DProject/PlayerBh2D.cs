using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBh2D : MonoBehaviour
{
    public Transform bh;

    private Material mat;
    // Use this for initialization
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        /*===============   追跡の範囲チェック  ===============*/
        int status = (int)GameObject.FindGameObjectWithTag("Player").transform.GetComponent<playerController>().status;
        if (status == 2)    // ブラックホールOnの時処理
        {
            mat.SetVector("_BlackHolePos", bh.position);
        }

    }
}
