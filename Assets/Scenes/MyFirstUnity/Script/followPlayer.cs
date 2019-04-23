using System.Collections;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    // ターゲットへの参照
    public Transform Target;
    // 相対座標 Playerの後ろに
    private Vector3 Offset;


    void Start()
    {
        //自分自身とtargetとの相対距離を求める
        Offset = GetComponent<Transform>().position - Target.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 自分の座標にtargetの座標を代入する
        GetComponent<Transform>().position = Target.position + Offset;
    }
}
