using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    [SerializeField]
    [Header("生成するitemの種類")]
    private item.BulletType itemType;

    [SerializeField]
    [Header("更新間隔")]
    private int refreshFream;

    [SerializeField]
    [Header("更新間隔")]
    private int refreashDistance;

    private bool hasFreeChild;
    private int hasNoChildCount;

    // Start is called before the first frame update
    void Start()
    {
        hasNoChildCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        hasFreeChild = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<item>().bulletStatus == item.BulletStatus.item)
            {
                hasFreeChild = true;
                break;
            }
        }

        if (hasFreeChild)
        {
            hasNoChildCount = 0;
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float dis = Vector3.Distance(transform.position, player.transform.position);
            if (dis >= refreashDistance)
            {
                hasNoChildCount++;
            }
        }


        GameObject go;
        if (hasNoChildCount >= refreshFream)
        {
            switch (itemType)
            {
                case item.BulletType.normal:
                    go = Resources.Load("Prefabs/Item") as GameObject;
                    go = Instantiate(go);
                    break;
                case item.BulletType.fire:
                    go = Resources.Load("Prefabs/eff_fire_nor") as GameObject;
                    go = Instantiate(go);
                    break;
                default:
                    go = Resources.Load("Prefabs/Item") as GameObject;
                    go = Instantiate(go);
                    break;
            }

            go.transform.SetParent(transform);
            go.transform.position = transform.position;

            item it = go.GetComponent<item>();
            it.target = GameObject.FindGameObjectWithTag("Player") .transform;
            it.mytransform = go.transform;
            blackHole bh = go.GetComponent<blackHole>();
            bh.bh = GameObject.FindGameObjectWithTag("Player").transform;

            hasNoChildCount = 0;
        }
    }
}
