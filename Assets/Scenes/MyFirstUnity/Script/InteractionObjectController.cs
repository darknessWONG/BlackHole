using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjectController : MonoBehaviour
{
    public item.BulletType needType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Item"))
        {
            if (other.gameObject.GetComponent<item>().type == needType)
            {
                GameObject newGo = Resources.Load("Prefabs/eff_explosion_star") as GameObject;
                newGo = Instantiate(newGo);

                Vector3 newPos = this.transform.position;
                newPos.y = 2f;
                newGo.transform.position = newPos;
                newGo.GetComponent<bombController>().liveFrame = 50f;

                for (int i = 0; i < transform.childCount; i++)
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
                GameObject.Destroy(gameObject);
            }
            GameObject.Destroy(other.gameObject);

        }
    }
}
