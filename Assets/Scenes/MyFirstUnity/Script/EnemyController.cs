using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public List<Vector2> list;
    public int nowDes;

    private NavMeshAgent nav;
    // Start is called before the first frame update
    void Start()
    {
        nowDes = 0;
        nav = GetComponent<NavMeshAgent>();
        nav.SetDestination(new Vector3(list[nowDes].x, list[nowDes].y, 0));
    }

    void FixedUpdate()
    {
        if (nav.remainingDistance <= 0.05)
        {
            nowDes = (nowDes + 1) % list.Count;
            nav.SetDestination(new Vector3(list[nowDes].x, list[nowDes].y, 0));
        }

        if(GetComponent<item>().bulletStatus == item.BulletStatus.bullet)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && other.GetComponent<playerController>().status == playerController.E_BlackHoleStatus.E_BlackHoleOff)
        {
            other.GetComponent<playerController>().isDie = true;
            other.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
