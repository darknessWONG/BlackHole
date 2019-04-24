using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;



public class playerController : MonoBehaviour
{
    // ブラックホール状態Enum
    public enum E_BlackHoleStatus
    {
        E_Index,
        E_BlackHoleOff,
        E_BlackHoleOn,
        E_Max,
    }
    // ブラックホール状態変数
    public E_BlackHoleStatus status
    {
        get;
        set;
    }

    [SerializeField]
    [Header("弾丸の最大数")]
    public int bulletMax;

    public int nowSelectBullet { get; set; }

    public bool isDie { get; set; }

    [SerializeField]
    [Header("プレイヤーの移動速度")]
    private float MoveSpeed = 0.1f;

    //private List<GameObject> bulletList;
    private GameObject[] bulletList;


    void Start()
    {
        // ブラックホール状態を初期化
        status = E_BlackHoleStatus.E_BlackHoleOff;

        //bulletList = new List<GameObject>();
        bulletList = new GameObject[bulletMax];
        nowSelectBullet = 0;

        isDie = false;
    }

    void Update()
    {
        // Z軸を0にする
        Vector3 temp = transform.localPosition;
        temp.z = 0;
        //temp.y = 1.5f;
        transform.localPosition = temp;
    }

    // 入力するたびに更新する
    void FixedUpdate()
    {
        // 入力ゲット
        float tempX = Input.GetAxis("Horizontal");
        float tempZ = Input.GetAxis("Vertical");

        // 同一のGameObjectが持つRigidbodyコンポーネントを取得
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        // rigidbodyのx軸（横）とz軸（奥）に力を加える
        //rigidbody.AddForce(tempX * MoveSpeed, 0, tempZ * MoveSpeed);
        /*==========        アニメーション         ==========*/
        motionChanger mc = GameObject.FindGameObjectWithTag("Player").GetComponent<motionChanger>();
        if (mc.IsMoveAnimation())
        {
            if (Input.GetKey(KeyCode.A))
            {
                //this.transform.Translate(Vector3.left * MoveSpeed);
                transform.Translate(0, 0, MoveSpeed);
                this.transform.forward = Vector3.Lerp(transform.forward, Vector3.left, 1);
            }
            if (Input.GetKey(KeyCode.D))
            {

                transform.Translate(0, 0, MoveSpeed);
                this.transform.forward = Vector3.Lerp(transform.forward, Vector3.right, 1);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.S))
            {

                //transform.Translate(0, 0, MoveSpeed);
                this.transform.forward = Vector3.Lerp(transform.forward, Vector3.back, 1);
            }
        }

        // spaceキーでブラックホール開始
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //switch (status)
            //{
            //    case E_BlackHoleStatus.E_BlackHoleOn:
            //        status = E_BlackHoleStatus.E_BlackHoleOff;
            //        break;
            //    case E_BlackHoleStatus.E_BlackHoleOff:
            //        status = E_BlackHoleStatus.E_BlackHoleOn;
            //        break;
            //    default:
            //        status = E_BlackHoleStatus.E_BlackHoleOff;
            //        break;
            //}
            status = E_BlackHoleStatus.E_BlackHoleOn;

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            //switch (status)
            //{
            //    case E_BlackHoleStatus.E_BlackHoleOn:
            //        status = E_BlackHoleStatus.E_BlackHoleOff;
            //        break;
            //    case E_BlackHoleStatus.E_BlackHoleOff:
            //        status = E_BlackHoleStatus.E_BlackHoleOn;
            //        break;
            //    default:
            //        status = E_BlackHoleStatus.E_BlackHoleOff;
            //        break;
            //}
            status = E_BlackHoleStatus.E_BlackHoleOff;

        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Shot();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            nowSelectBullet = (nowSelectBullet + 1) % bulletMax;
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            nowSelectBullet = (nowSelectBullet - 1) >= 0 ? nowSelectBullet - 1 : bulletMax - 1;
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            GameObject newGo = Resources.Load("Prefabs/eff_fire_ball") as GameObject;
            Instantiate(newGo);
            newGo.transform.position = new Vector3(0, 1.5f, 0);
        }
 
    }

    public bool CanPrime()
    {
        if (GetBulletCount() > 10)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool Prime(GameObject go)
    {
        if (GetBulletCount() > 10)
        {
            return false;
        }
        else
        {
            if (!go.CompareTag("Enemy"))
            {
                bulletList[getFristAvlivableIndex()] = go;

            }
            return true;
        }
    }

    public int GetBulletCount()
    {
        int sum = 0;
        for (int i = 0; i < bulletMax; i++)
        {
            if (checkBullet(i))
            {
                sum++;
            }
        }
        return sum;
    }

    public bool checkBullet(int index)
    {
        if (null == bulletList[index])
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int getFristAvlivableIndex()
    {
        for (int i = 0; i < bulletMax; i++)
        {
            if (!checkBullet(i))
            {
                return i;
            }
        }
        return -1;
    }

    private void Shot()
    {
        if (GetBulletCount() > 0)
        {
            if (checkBullet(nowSelectBullet))
            {
                GameObject go = bulletList[nowSelectBullet];
                bulletList[nowSelectBullet] = null;
                if (go.GetComponent<item>().type == item.BulletType.fire)
                {
                    var psmm = go.GetComponent<ParticleSystem>().main;
                    psmm.gravityModifier = new ParticleSystem.MinMaxCurve(0);
                }

                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Vector3 direct = Vector3.Normalize(transform.forward);
                newPos += direct;
                newPos.y += GetComponent<BoxCollider>().size.y / 2;
                Vector3 newV = Vector3.Normalize(transform.forward) * MoveSpeed * 50;
                newV.z = 0;
                newV.y = 0;
                go.GetComponent<Transform>().position = newPos;
                go.GetComponent<Rigidbody>().velocity = newV;
                go.GetComponent<Rigidbody>().useGravity = false;
                //go.GetComponent<BoxCollider>().enabled = false;
                item it = go.GetComponent<item>();
                it.expSpeed = MoveSpeed + 1;
                it.expDistance = Vector3.Normalize(newV);
                it.bulletStatus = item.BulletStatus.bullet;

                go.transform.parent = null;
                go.SetActive(true);
            }
        }
    }
}
