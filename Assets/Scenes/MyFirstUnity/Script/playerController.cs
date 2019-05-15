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
    // ブラックホール状態Enum
    public enum E_JumpStatus
    {
        E_normal,
        E_raise,
        E_fall,
        E_Max
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

    [SerializeField]
    [Header("プレイヤーの移動速度")]
    private float MoveSpeed = 0.1f;

    [SerializeField]
    [Header("ジャンプの高さ")]
    private float jumpSpeed = 200;

    public int nowSelectBullet { get; set; }
    public bool isDie { get; set; }
    public E_JumpStatus js { get; set; }

    //private List<GameObject> bulletList;
    private GameObject[] bulletList;

    private const float margin = 0.1f;

    void Start()
    {
        // ブラックホール状態を初期化
        status = E_BlackHoleStatus.E_BlackHoleOff;

        //bulletList = new List<GameObject>();
        bulletList = new GameObject[bulletMax];
        for (int i = 0; i < bulletMax; i++) bulletList[i] = null;
        nowSelectBullet = 0;

        isDie = false;

        GetComponent<Rigidbody>().freezeRotation = true;
    }

    void Update()
    {
        // Z軸を0にする
        Vector3 temp = transform.localPosition;
        temp.z = 0;
        //temp.y = 1.5f;
        transform.localPosition = temp;

        JumpStatusUpdate();
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
            /*===== コントローラー =====*/
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                transform.Translate(0, 0, MoveSpeed);
                this.transform.forward = Vector3.Lerp(transform.forward, Vector3.left, 1);
            }
            else if (Input.GetAxis("Horizontal") > 0.1f)
            {
                transform.Translate(0, 0, MoveSpeed);
                this.transform.forward = Vector3.Lerp(transform.forward, Vector3.right, 1);
            }

            if(Input.GetButton("Jump") && E_JumpStatus.E_normal == js)
            {
                js = E_JumpStatus.E_raise;
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpSpeed * 3.0f);
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

        //if(Input.GetKeyDown(KeyCode.J))
        //{
        //    GameObject newGo = Resources.Load("Prefabs/eff_fire_ball") as GameObject;
        //    Instantiate(newGo);
        //    newGo.transform.position = new Vector3(0, 1.5f, 0);
        //}

        /*===== コントローラー =====*/
        if (Input.GetButtonDown("Bh"))
        {
            BlackHoleController(true);
            //Debug.Log("bh true");
        }
        else if (Input.GetButtonUp("Bh"))
        {
            BlackHoleController(false);
            //Debug.Log("bh false");
        }
        if (Input.GetButtonUp("Shot"))
        {
            Shot();
            //Debug.Log("show sb");
        }
        if (Input.GetButtonUp("bulletToLeft"))
        {
            nowSelectBullet = (nowSelectBullet + 1) % bulletMax;
        }
        if (Input.GetButtonUp("bulletToRight"))
        {
            nowSelectBullet = (nowSelectBullet - 1) >= 0 ? nowSelectBullet - 1 : bulletMax - 1;
        }
    }

    public bool CanPrime()
    {
        if (GetBulletCount() >= bulletMax)
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
        if (GetBulletCount() >= bulletMax)
        {
            return false;
        }
        else
        {
            if (!go.CompareTag("Enemy"))
            {
                //Debug.Log(getFristAvlivableIndex().ToString());
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

    public item.BulletType GetBulletType(int index)
    {
        if(null == bulletList[index])
        {
            return item.BulletType.bullet3;
        }
        else
        {
            return bulletList[index].GetComponent<item>().type;
        }
    }

    public int getFristAvlivableIndex()
    {
        for (int i = 0; i < bulletMax; i++)
        {
            //Debug.Log(i.ToString());
            if (!checkBullet(i))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// find the frist unavlivable index
    /// </summary>
    /// <param name="head">search start from this index</param>
    /// <returns>the index of the frist unablivable list, will return -1 went the list is emety</returns>
    public int GetFristUnavlivableIndex(int head)
    {
        int i = head;
        i = (i + 1) % bulletMax;
        while (i != head)
        {
            if (checkBullet(i))
            {
                return i;
            }
            i = (i + 1) % bulletMax;
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
                SetBulletToShot(go);

                int index = GetFristUnavlivableIndex(nowSelectBullet);
                nowSelectBullet = index == - 1 ? 0 : index;
            }
        }
    }

    private void SetBulletToShot(GameObject go)
    {
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
        item it = go.GetComponent<item>();
        it.expSpeed = MoveSpeed + 1;
        it.expDistance = Vector3.Normalize(newV);
        it.bulletStatus = item.BulletStatus.bullet;

        go.transform.parent = null;
        go.SetActive(true);
    }

    private void BlackHoleController(bool switchs)
    {
        if (switchs && CanPrime())
        {
            status = E_BlackHoleStatus.E_BlackHoleOn;
        }
        else
        {
            status = E_BlackHoleStatus.E_BlackHoleOff;
        }
    }

    private void JumpStatusUpdate()
    {
        if (IsOnGround() && E_JumpStatus.E_normal != js)
        {
            js = E_JumpStatus.E_normal;
        }
    }

    private bool IsOnGround()
    {
        return Physics.Raycast(transform.position + Vector3.up * margin, -Vector3.up, margin * 2, 1 << LayerMask.NameToLayer("ground"));
    }
}
