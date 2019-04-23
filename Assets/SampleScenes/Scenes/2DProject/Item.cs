using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    [Header("ターゲット")]
    private Transform target;

    [SerializeField]
    [Header("移動速度")]
    private float moveSpeed = 5.0f;

    [SerializeField]
    [Header("回転速度")]
    private float rotateSpeed = 5.0f;

    [SerializeField]
    [Header("自分のオブジェクト")]
    private Transform mytransform;

    [SerializeField]
    [Header("追跡範囲チェックの半径")]
    private float distance = 5;

    void Start()
    {
        // 追跡のターゲットのTAGSをPlayerに変更
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 距離計算
        float disTemp = Vector3.Distance(transform.position, target.position);

        // チェック範囲内に入ると
        if (disTemp < distance)
        {
            /*===============   追跡の処理   ===============*/
            Debug.DrawLine(target.transform.position, this.transform.position, Color.yellow);
            mytransform.rotation = Quaternion.Slerp(mytransform.rotation,
                Quaternion.LookRotation(target.position - mytransform.position), rotateSpeed * Time.deltaTime);
            mytransform.position += mytransform.forward * moveSpeed * Time.deltaTime;
        }
    }


    // OnTriggerEnter　：　判定のみのTriggerとの接触判定
    // OnCollisionEnter　：　物理的な反射を持つColliderとの接触判定
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player"))
        {
            // 何らかの処理
            Destroy(gameObject);
        }
    }
}
