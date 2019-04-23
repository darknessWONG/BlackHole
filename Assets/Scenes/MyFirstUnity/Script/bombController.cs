using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombController : MonoBehaviour
{
    public float liveFrame;

    private float startFrame;

    // Start is called before the first frame update
    void Start()
    {
        startFrame = Time.frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount > liveFrame + startFrame)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
