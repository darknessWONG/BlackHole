using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class dangerWall : MonoBehaviour
{
    // 当たり判定
    void OnCollisionEnter(Collision hit)
    {
        // 当たると
        if(hit.gameObject.CompareTag("Player"))
        {
            // 処理
            // 現在のシーン番号を取得
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 現在のシーンを再読込する
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
