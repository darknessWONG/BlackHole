using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCounterController : MonoBehaviour
{
    public Sprite a;
    public Sprite b;
    public Sprite c;

    private GameObject[] bulletList;
    private GameObject bulletSelector;
    // Start is called before the first frame update
    void Start()
    {
        bulletList = GameObject.FindGameObjectsWithTag("BulletCounter");
        bulletSelector = GameObject.FindGameObjectWithTag("BulletSelector");

        playerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        for (int i = 0; i < pc.bulletMax; i++)
        {
            bulletList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-30f * i, 0f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
        for (int i = 0; i < pc.bulletMax; i++)
        {
            bulletList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-30f * i, 0f, 0f);
        }


        for(int i = 0; i < pc.bulletMax; i++)
        {
            if(pc.checkBullet(i))
            {
                Debug.Log(pc.GetBulletType(i).ToString());
                switch (pc.GetBulletType(i))
                {
                    case item.BulletType.fire:
                        bulletList[i].GetComponent<Image>().sprite = b;
                        break;
                    case item.BulletType.normal:
                        bulletList[i].GetComponent<Image>().sprite = c;
                        break;
                    //default:
                    //    bulletList[i].GetComponent<Image>().sprite = a;
                    //    break;
                }

            }
            else
            {
                bulletList[i].GetComponent<Image>().sprite = a;
            }
        }

        bulletSelector.GetComponent<RectTransform>().anchoredPosition = new Vector3(-30f * pc.nowSelectBullet, 30f, 0f);
    }
}
