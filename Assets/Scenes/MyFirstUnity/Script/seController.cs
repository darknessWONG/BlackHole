using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seController : MonoBehaviour
{
    private enum E_SeLIST
    {
        run,
        absorption,
        max,
    }

    public struct S_SE
    {
        public AudioSource se;
        public bool isPlay;
    }

    private S_SE m_run;
    private S_SE m_absorption;

    // Start is called before the first frame update
    void Start()
    {
        m_run.se = gameObject.GetComponents<AudioSource>()[(int)E_SeLIST.run];
        m_absorption.se = gameObject.GetComponents<AudioSource>()[(int)E_SeLIST.absorption];
    }

    // Update is called once per frame
    void Update()
    {
        // BHのSEチェック
        if(!m_absorption.isPlay)
        {
            playerController.E_BlackHoleStatus bhStatus =
                GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>().status;
            if (bhStatus == playerController.E_BlackHoleStatus.E_BlackHoleOn)
            {
                m_absorption.isPlay = true;
                m_absorption.se.Play();
            }
        }
        else
        {
            playerController.E_BlackHoleStatus bhStatus =
                GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>().status;
            if (bhStatus == playerController.E_BlackHoleStatus.E_BlackHoleOff)
            {
                m_absorption.isPlay = false;
                m_absorption.se.Stop();
            }
        }

        // 足音を止めるチェック
        if (Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.D))
        {
            if (m_run.isPlay)
            {
                m_run.isPlay = false;
                m_run.se.Stop();
            }

        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.D))
        {
            if(!m_run.isPlay)
            {
                m_run.isPlay = true;
                m_run.se.Play();
            }
        }


    }

}
