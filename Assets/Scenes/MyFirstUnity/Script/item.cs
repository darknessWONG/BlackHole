using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class item : MonoBehaviour
{
    public enum BulletStatus
    {
        item,
        bullet
    }

    public enum BulletType
    {
        normal,
        fire,
        bullet3
    }

    public Vector3 expDistance { get; set; }
    public float expSpeed { get; set; }
    public BulletStatus bulletStatus { get; set; }

    [SerializeField]
    [Header("物体の種類")]
    public BulletType type;

    [SerializeField]
    [Header("ターゲット")]
    public Transform target;

    [SerializeField]
    [Header("移動速度")]
    private float moveSpeed = 5.0f;

    [SerializeField]
    [Header("回転速度")]
    private float rotateSpeed = 5.0f;

    [SerializeField]
    [Header("自分のオブジェクト")]
    public Transform mytransform;

    [SerializeField]
    [Header("追跡範囲チェックの半径")]
    private float distance = 5;

    void Start()
    {
        // 追跡のターゲットのTAGSをPlayerに変更
        target = GameObject.FindGameObjectWithTag("Player").transform;

        expDistance = new Vector3(0, 0, 0);
        expSpeed = 0f;
        bulletStatus = BulletStatus.item;
    }


    void Update()
    {
        /*===============   追跡の範囲チェック  ===============*/
        int status = (int)target.GetComponent<playerController>().status;
        playerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        if (BulletStatus.item == bulletStatus)
        {
            if (status == 2 && pc.CanPrime())    // ブラックホールOnの時処理
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
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(expDistance.x * expSpeed, 0, 0);
            Debug.Log(expDistance.ToString());
        }

    }

    // OnTriggerEnter　：　判定のみのTriggerとの接触判定
    // OnCollisionEnter　：　物理的な反射を持つColliderとの接触判定
    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Player") && (int)hit.GetComponent<playerController>().status == 2
            && bulletStatus == BulletStatus.item)
        {
            // 何らかの処理
            if(hit.gameObject.GetComponent<playerController>().Prime(this.gameObject))
            {
                this.gameObject.SetActive(false);
                transform.SetParent(hit.transform);
            }
        }
    }
}
