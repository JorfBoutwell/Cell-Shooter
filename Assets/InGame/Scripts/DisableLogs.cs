using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableLogs : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Debug.unityLogger.logEnabled = false;
    }

}
