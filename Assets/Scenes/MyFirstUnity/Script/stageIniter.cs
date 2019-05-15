using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageIniter : MonoBehaviour
{
    [Header("ステージの左点")]
    public List<Vector3> positions;
    [Header("ステージの長さ")]
    public List<float> lengths;

    // Start is called before the first frame update
    void Start()
    {
        if(positions.Count == lengths.Count)
        {
            for(int i = 0; i < positions.Count; i++)
            {
                for (int j = 0; j < lengths[i]; j++)
                {
                    GameObject go = Resources.Load("Prefabs/Block_Grass") as GameObject;
                    go = Instantiate(go);
                    go.transform.SetParent(transform);
                    go.transform.position = transform.position;

                    Vector3 newPos = go.transform.position;
                    newPos.x = positions[i].x + j;
                    newPos.y = positions[i].y;
                    newPos.z = positions[i].z;
                    go.transform.position = newPos;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
