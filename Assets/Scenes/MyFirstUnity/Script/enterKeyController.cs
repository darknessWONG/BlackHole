using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class enterKeyController : MonoBehaviour
{
    public enum E_SceneIndex
    {
        SceneTitle = 0,
        SceneStageOne,
        SceneResult,
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        E_SceneIndex level = (E_SceneIndex)SceneManager.GetActiveScene().buildIndex;


        switch(level)
        {
            case E_SceneIndex.SceneTitle:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    FadeManager.FadeOut("MyFirstUnity", 1.5f);
                }
                break;
            case E_SceneIndex.SceneStageOne:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    FadeManager.FadeOut("Result", 1.5f);
                }
                break;
            case E_SceneIndex.SceneResult:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    FadeManager.FadeOut("Title", 1.5f);
                }
                break;
        }

        if (GameObject.FindGameObjectWithTag("Player").transform.position.x >= 151)
        {
            FadeManager.FadeOut("Result", 1.5f);
        }

        if(GameObject.FindGameObjectWithTag("Player").transform.position.y <= -10)
        {
            FadeManager.FadeOut("Result", 1.5f);
        }
    }
}
