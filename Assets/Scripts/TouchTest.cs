using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TouchManager.Moved += (info) =>
        {
            Debug.Log("座標は" + info.screenPoint);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
