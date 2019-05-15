using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
    [SerializeField]
    [Header("ターゲット")]
    private Transform target;

    //得点
    public int Score { get; set; }

    // 参照　UIに表示される数テキスト
    public UnityEngine.UI.Text scoreLabel;
    // 参照　Winのテキスト
    //public GameObject winnerLabelObject;
    // 参照　UIに表示されるブラックホール状態テキスト
    public UnityEngine.UI.Text blackHoleLabel;


    // 開始状態のターゲット設定
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        // 追跡のターゲットのTAGSをPlayerに変更
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    // アイテム数を更新
    void Update()
    {
        enterKeyController.E_SceneIndex sceneIndex = (enterKeyController.E_SceneIndex)SceneManager.GetActiveScene().buildIndex;

        if (enterKeyController.E_SceneIndex.SceneStageOne == sceneIndex)
        {
            GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
            int i = 0;
            for (; i < playerList.Length; i++)
            {
                if (!playerList[i].GetComponent<playerController>().isDie)
                {
                    break;
                }
            }
            if (i >= playerList.Length)
            {
                FadeManager.FadeOut("Result", 1.5f);
            }

            // 現在数量をTAGからゲット
            int count = GameObject.FindGameObjectsWithTag("Item").Length;

            // 数量をセット
            scoreLabel.text = count.ToString();

            if (count == 0)
            {
                // クリア時の処理
                //winnerLabelObject.SetActive(true);
            }

            // ブラックホール状態テキスト
            int status = (int)target.GetComponent<playerController>().status;
            switch (status)
            {
                case 1:
                    blackHoleLabel.text = "Black Hole Off";
                    break;
                case 2:
                    blackHoleLabel.text = "Black Hole On";
                    break;
            }
        }
    }

    private void Awake()
    {
        Score = 0;
    }
}
