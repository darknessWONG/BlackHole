using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireSeController : MonoBehaviour
{

    private AudioSource m_fire;
    private bool m_isPlay;

    // Start is called before the first frame update
    void Start()
    {
        m_fire = gameObject.GetComponent<AudioSource>();
        m_isPlay = true;
    }

    // Update is called once per frame
    void Update()
    {
        playerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        // 距離計算
        float disTemp = Vector3.Distance(transform.position, pc.GetComponent<playerController>().transform.position);
        if (!m_isPlay)
        {
            // チェック範囲内に入ると
            if (disTemp < 12.5f)
            {
                /*===============   音の処理   ===============*/
                //Debug.Log("fuck fire");
                m_isPlay = true;
                m_fire.Play();
            }
        }
        else
        {
            // チェック範囲以外と
            if (disTemp >= 12.5f)
            {
                /*===============   音の処理   ===============*/
                //Debug.Log("fuck fire");
                m_isPlay = false;
                m_fire.Stop();
            }
        }

    }
}
